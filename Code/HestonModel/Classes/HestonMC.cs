using System;
using HestonModel.Interfaces;
using HestonModel.Classes.InterfaceClasses;
using MathNet.Numerics.Distributions;
using System.Threading;

namespace HestonModel.Classes
{
    /// <summary>
    /// Class that implements a tread with state used in the parallel Monte Carlo.
    /// </summary>
    public class WorkerMC
    {
        private int _NumberOfTrials;
        private int _NumberOfTimeSteps;

        private double sum;
        private Func<double[], double> _payoffFunc;

        private VarianceProcessParameters _VarianceParameters;

        private double _Maturity;
        private double _InitialStockPrice;
        private double _RiskFreeRate;

        public double getSum() { return sum; }

        /// <summary>
        /// Initializes a new instance of the <see cref="T:HestonModel.Classes.WorkerMC"/> class.
        /// </summary>
        /// <param name="NumberOfTrials">Number of trials.</param>
        /// <param name="NumberOfTimeSteps">Number of time steps.</param>
        /// <param name="payoffFunc">Payoff function.</param>
        /// <param name="VarianceParameters">Variance parameters.</param>
        /// <param name="Maturity">Maturity.</param>
        /// <param name="InitialStockPrice">Initial stock price.</param>
        /// <param name="RiskFreeRate">Risk free rate.</param>
        public WorkerMC(int NumberOfTrials, int NumberOfTimeSteps, Func<double[], double> payoffFunc,
        VarianceProcessParameters VarianceParameters, double Maturity, double InitialStockPrice,
        double RiskFreeRate)
        {
            // Init MC settings
            _NumberOfTrials = NumberOfTrials;
            _NumberOfTimeSteps = NumberOfTimeSteps;
            // Init sum
            sum = 0;
            // Init Payoff function
            _payoffFunc = payoffFunc;
            // Init Variance Params
            _VarianceParameters = VarianceParameters;
            // Init Option Params
            _Maturity = Maturity;
            _InitialStockPrice = InitialStockPrice;
            _RiskFreeRate = RiskFreeRate;

        }

        /// <summary>
        /// Generates paths for the Monte Carlo simulation depending on the 
        /// state of the thread.
        /// </summary>
        public void ThreadGenPaths()
        {
            double dz1;
            double dz2;

            // Hesto Model vars
            double rho = _VarianceParameters.Rho;
            double kappa = _VarianceParameters.Kappa;
            double theta = _VarianceParameters.Theta;
            double sigma = _VarianceParameters.Sigma;

            // Time-step
            double tau = _Maturity / _NumberOfTimeSteps;
            // MC samples for 1st Wiener process
            double[] x1 = new double[_NumberOfTimeSteps];
            // MC samples for 2nd Wiener process
            double[] x2 = new double[_NumberOfTimeSteps];

            // MC Constants
            double alpha = (4 * kappa * theta - Math.Pow(sigma, 2)) / 8;
            double beta = -kappa / 2;
            double gamma = sigma / 2;

            for (int i = 0; i < _NumberOfTrials; i++)
            {
                double[] s = new double[_NumberOfTimeSteps];
                double[] y = new double[_NumberOfTimeSteps];
                s[0] = _InitialStockPrice;
                y[0] = Math.Sqrt(_VarianceParameters.V0);

                // Samples for current path
                Normal.Samples(x1, 0, 1);
                Normal.Samples(x2, 0, 1);

                for (int j = 1; j < _NumberOfTimeSteps; j++)
                {
                    dz1 = Math.Sqrt(tau) * x1[j];
                    dz2 = Math.Sqrt(tau) * (rho * x1[j] +
                          Math.Sqrt(1 - Math.Pow(rho, 2)) * x2[j]);

                    // Y increment
                    y[j] = (y[j - 1] + gamma * dz2) / (2 - 2 * beta * tau) +
                             Math.Sqrt(Math.Pow(y[j - 1] + gamma * dz2, 2) /
                             (4 * Math.Pow(1 - beta * tau, 2)) +
                             alpha * tau / (1 - beta * tau));

                    s[j] = s[j - 1] + _RiskFreeRate * s[j - 1] * tau + y[j - 1] * s[j - 1] * dz1;
                }

                // Accumulate payoff for each path
                sum += _payoffFunc(s);
            }
        }

    }

    /// <summary>
    /// Class to represent an option which is priced using Monte Carlo method 
    /// in Heston model that . This class inherits the HestonOption class and
    /// the IMonteCarloSettings interface.
    /// </summary>
    public class HestonMC : HestonModelParameters, IMonteCarloSettings, IOption
    {

        private int _NumberOfTrials;
        private int _NumberOfTimeSteps;
        private double _Maturity;

        // Implementation of Interfaces
        public int NumberOfTrials
        {
            get { return _NumberOfTrials; }
        }

        public int NumberOfTimeSteps
        {
            get { return _NumberOfTimeSteps; }
        }
        public double Maturity
        {
            get { return _Maturity; }
        }

        /// <summary>
        /// Initializes a new instance of Heston MC class.
        /// </summary>
        /// <param name="parameters">Interface holding Heston Model params.</param>
        /// <param name="monteCarloSimulationSettings">Interface holding Monte carlo simulation settings.</param>
        public HestonMC(HestonModelParameters parameters,
                        MonteCarloSettings monteCarloSimulationSettings,
                        double Maturity) :
                        base(parameters.InitialStockPrice,
                             parameters.RiskFreeRate,
                             parameters.GetVariance())
        {
            _NumberOfTrials = monteCarloSimulationSettings.NumberOfTrials;
            _NumberOfTimeSteps = monteCarloSimulationSettings.NumberOfTimeSteps;
            _Maturity = Maturity;
        }


        /// <summary>
        /// Gens the paths.
        /// </summary>
        /// <returns>Initializes and launches the threads to produce the Monte
        /// Carlo paths.</returns>
        /// <param name="payoffFunc">Payoff function.</param>
        /// <param name="workers">Number of threads.</param>
        protected double GenPaths(Func<double[], double> payoffFunc, int workers)
        {
            // Hesto Model vars
            double rho = _VarianceParameters.Rho;
            double kappa = _VarianceParameters.Kappa;
            double theta = _VarianceParameters.Theta;
            double sigma = _VarianceParameters.Sigma;

            // Check Feller Condition
            if (2 * kappa * theta <= Math.Pow(sigma, 2))
            {
                throw new Exception("Feller Condition unsatisfied.");
            }

            // Create Thread Pool
            WorkerMC[] workerArr = new WorkerMC[workers];
            ThreadStart[] threadStartArr = new ThreadStart[workers];
            Thread[] threadArr = new Thread[workers];
            for (int k = 0; k < workers; k++)
            {
                // Create Thread with state
                workerArr[k] = new WorkerMC(NumberOfTrials / workers, NumberOfTimeSteps, payoffFunc,
                _VarianceParameters, Maturity, InitialStockPrice, RiskFreeRate);
                threadStartArr[k] = new ThreadStart(workerArr[k].ThreadGenPaths);
                threadArr[k] = new Thread(threadStartArr[k]);
                // Start the thread
                threadArr[k].Start();
            }

            // Join all thread with Main thread
            for (int k = 0; k < workers; k++)
            {
                threadArr[k].Join();
            }

            // Accumulate all sums
            double sum = 0;
            for (int k = 0; k < workers; k++)
            {
                sum += workerArr[k].getSum();
            }

            // Return price
            return Math.Exp(-_RiskFreeRate * _Maturity) * (sum / NumberOfTrials);
        }

    }

}
