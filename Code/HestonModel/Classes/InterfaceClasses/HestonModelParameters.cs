using System;
using HestonModel.Interfaces;

namespace HestonModel.Classes.InterfaceClasses
{
    /// <summary>
    /// Base class that represents an option in Heston Model.
    /// </summary>
    public class HestonModelParameters : IHestonModelParameters
    {
        // Heston Model Parameters
        protected double _InitialStockPrice;
        protected double _RiskFreeRate;
        protected VarianceProcessParameters _VarianceParameters;

        // Interface Implementation
        public IVarianceProcessParameters VarianceParameters
        {
            get { return _VarianceParameters; }
        }

        public double InitialStockPrice
        {
            get { return _InitialStockPrice; }
        }

        public double RiskFreeRate
        {
            get { return _RiskFreeRate; }
        }

        public VarianceProcessParameters GetVariance()
        {
            return _VarianceParameters;
        }

        /// <summary>
        /// Initializes a new instance of the Heston Option
        /// </summary>
        /// <param name="InitialStockPrice">Initial stock price.</param>
        /// <param name="RiskFreeRate">Risk free rate.</param>
        /// <param name="VarianceParameters">Variance parameters.</param>
        public HestonModelParameters(double InitialStockPrice, double RiskFreeRate,
                            VarianceProcessParameters VarianceParameters)
        {
            // Sanity Check
            if (InitialStockPrice <= 0)
            {
                throw new Exception("Initial Stock price must be positive.");
            }

            // Initialize Heston parameters
            _InitialStockPrice = InitialStockPrice;
            _RiskFreeRate = RiskFreeRate;
            _VarianceParameters = VarianceParameters;
        }

        /// <summary>
        /// Returns double array, which each element represents a different
        /// model parameter.
        /// Order: kappa, theta, sigma, rho, v0
        /// </summary>
        /// <returns>The parameters to array.</returns>
        public double[] CalibrationParamsToArray()
        {
            double[] paramArray = new double[5];
            paramArray[0] = _VarianceParameters.Kappa;
            paramArray[1] = _VarianceParameters.Theta;
            paramArray[2] = _VarianceParameters.Sigma;
            paramArray[3] = _VarianceParameters.V0;
            paramArray[4] = _VarianceParameters.Rho;

            return paramArray;
        }
    }
}
