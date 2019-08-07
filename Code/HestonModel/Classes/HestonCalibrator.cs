using System;
using System.Collections.Generic;
using HestonModel.Classes.InterfaceClasses;

namespace HestonModel.Classes
{
    /// <summary>
    /// Class that implements the Calibration failed exception.
    /// </summary>
    public class CalibrationFailedException : Exception
    {
        public CalibrationFailedException()
        {
        }
        /// <summary>
        /// Initializes a new instance of the <see cref="T:HestonModel.Classes.CalibrationFailedException"/> class.
        /// </summary>
        /// <param name="message">Message.</param>
        public CalibrationFailedException(string message)
            : base(message)
        {
        }
    }

    /// <summary>
    /// Class implementing a calibrator for Heston Model.
    /// </summary>
    public class HestonCalibrator
    {
        // Model Params
        private double _InitialStockPrice;
        private double _RiskFreeRate;

        // Market Data
        private LinkedList<OptionMarketData<EuropeanOption>> marketOptionsList;

        // Calibration Settings
        private const double defaultAccuracy = 10e-3;
        private const int defaultMaxIterations = 500;
        private double accuracy;
        private int maxIterations;

        // Calibration Vars
        private CalibrationOutcome outcome;
        private double[] calibratedParams;

        /// <summary>
        /// Initializes a new instance of the HestonCalibrator class.
        /// </summary>
        public HestonCalibrator()
        {
            // Heston Model Params
            _RiskFreeRate = 0;
            _InitialStockPrice = 100;

            // Calibration Settings
            accuracy = defaultAccuracy;
            maxIterations = defaultMaxIterations;

            // Market Data
            marketOptionsList = new LinkedList<OptionMarketData<EuropeanOption>>();

            // Set default guess parameters
            calibratedParams = new double[] { 0.1, 0.05, 0.05, 0.5, 0.5 };
        }

        /// <summary>
        /// Initializes a new instance of the HestonCalibrator class.
        /// </summary>
        /// <param name="parameters">Heston Model Parameters</param>
        /// <param name="marketOptions">Observed Option prices</param>
        /// <param name="calibrationSettings">Calibration settings</param>
        public HestonCalibrator(HestonModelParameters parameters,
                                LinkedList<OptionMarketData<EuropeanOption>> marketOptions,
                                CalibrationSettings calibrationSettings)
        {
            // Heston Model Params
            _RiskFreeRate = parameters.RiskFreeRate;
            _InitialStockPrice = parameters.InitialStockPrice;

            // Copy market data
            marketOptionsList = new LinkedList<OptionMarketData<EuropeanOption>>(marketOptions);

            // Calibration Settings
            accuracy = calibrationSettings.Accuracy;
            maxIterations = calibrationSettings.MaximumNumberOfIterations;

            // Set default guess parameters
            calibratedParams = parameters.CalibrationParamsToArray();
        }

        /// <summary>
        /// Add new market data.
        /// </summary>
        /// <param name="euOption">European option.</param>
        /// <param name="price">European option Price</param>
        public void AddObservedOption(EuropeanOption euOption, double price)
        {
            OptionMarketData<EuropeanOption> newMarketOption = new OptionMarketData<EuropeanOption>(euOption, price);
            marketOptionsList.AddLast(newMarketOption);
        }

        /// <summary>
        /// Calculates the mean square error between model and market.
        /// </summary>
        /// <returns>The mean square error between model and market.</returns>
        /// <param name="m">Current heston parameters</param>
        public double CalcMeanSquareErrorBetweenModelAndMarket(HestonModelParameters m)
        {
            double meanSqErr = 0;
            foreach (OptionMarketData<EuropeanOption> marketData in marketOptionsList)
            {
                EuropeanOptionFormula euFormula = new EuropeanOptionFormula(m, marketData.Option);
                double modelPrice = euFormula.Price();

                double difference = modelPrice - marketData.Price;
                meanSqErr += difference * difference;
            }

            return meanSqErr;
        }

        /// <summary>
        /// Calibrations the objective function.
        /// </summary>
        /// <param name="paramsArray">Parameters array.</param>
        /// <param name="func">Objective Funtion.</param>
        /// <param name="obj">Object.</param>
        public void CalibrationObjectiveFunction(double[] paramsArray, ref double func, object obj)
        {
            VarianceProcessParameters varianceParams = new VarianceProcessParameters
            (paramsArray[0], paramsArray[1], paramsArray[2], paramsArray[3], paramsArray[4]);
            HestonModelParameters m = new HestonModelParameters(_InitialStockPrice, _RiskFreeRate, varianceParams);
                                      
            func = CalcMeanSquareErrorBetweenModelAndMarket(m);
        }

        /// <summary>
        /// Calibration process.
        /// </summary>
        public void Calibrate()
        {
            outcome = CalibrationOutcome.NotStarted;

            double[] initialParams = new double[calibratedParams.Length];
            calibratedParams.CopyTo(initialParams, 0);  // a reasonable starting guess

            double epsg = accuracy;
            double epsf = accuracy;
            double epsx = accuracy;
            double diffstep = 1e-6;
            int maxits = maxIterations;
            double stpmax = 0.05;


            alglib.minlbfgsstate state;
            alglib.minlbfgsreport rep;
            alglib.minlbfgscreatef(5, initialParams, diffstep, out state);
            alglib.minlbfgssetcond(state, epsg, epsf, epsx, maxits);
            alglib.minlbfgssetstpmax(state, stpmax);

            // this will do the work
            alglib.minlbfgsoptimize(state, CalibrationObjectiveFunction, null, null);
            double[] resultParams = new double[calibratedParams.Length];
            alglib.minlbfgsresults(state, out resultParams, out rep);

            System.Console.WriteLine("Termination type: {0}", rep.terminationtype);
            System.Console.WriteLine("Num iterations {0}", rep.iterationscount);
            System.Console.WriteLine("{0}", alglib.ap.format(resultParams, 5));

            if (rep.terminationtype == 1            // relative function improvement is no more than EpsF.
                || rep.terminationtype == 2         // relative step is no more than EpsX.
                || rep.terminationtype == 4)
            {       // gradient norm is no more than EpsG
                outcome = CalibrationOutcome.FinishedOK;
                // we update the ''inital parameters''
                calibratedParams = resultParams;
            }
            else if (rep.terminationtype == 5)
            {   // MaxIts steps was taken
                outcome = CalibrationOutcome.FailedMaxItReached;
                // we update the ''inital parameters'' even in this case
                calibratedParams = resultParams;

            }
            else
            {
                outcome = CalibrationOutcome.FailedOtherReason;
                throw new CalibrationFailedException("Heston model calibration failed badly.");
            }
        }

        /// <summary>
        /// Gets the calibration status.
        /// </summary>
        /// <param name="calibOutcome">Calibration outcome.</param>
        /// <param name="pricingError">Pricing error.</param>
        public void GetCalibrationStatus(ref CalibrationOutcome calibOutcome, ref double pricingError)
        {
            calibOutcome = outcome;

            VarianceProcessParameters varianceParams = new VarianceProcessParameters
            (calibratedParams[0], calibratedParams[1], calibratedParams[2], calibratedParams[3], calibratedParams[4]);
            HestonModelParameters m = new HestonModelParameters(_InitialStockPrice, _RiskFreeRate, varianceParams);

            pricingError = CalcMeanSquareErrorBetweenModelAndMarket(m);
        }

        /// <summary>
        /// Gets the calibrated model.
        /// </summary>
        /// <returns>The calibrated model.</returns>
        public HestonModelParameters GetCalibratedModel()
        {
            VarianceProcessParameters varianceParams = new VarianceProcessParameters
            (calibratedParams[0], calibratedParams[1], calibratedParams[2], calibratedParams[3], calibratedParams[4]);
            HestonModelParameters m = new HestonModelParameters(_InitialStockPrice, _RiskFreeRate, varianceParams);
            return m;
        }
    }
}