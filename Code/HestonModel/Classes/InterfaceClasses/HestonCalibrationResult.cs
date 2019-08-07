using System;
using HestonModel.Interfaces;

namespace HestonModel.Classes.InterfaceClasses
{
    /// <summary>
    /// Class that holds the outcome details of the Heston calibration.
    /// </summary>
    public class HestonCalibrationResult : IHestonCalibrationResult
    {
        double _PricingError;
        CalibrationOutcome _MinimizerStatus;
        HestonModelParameters _Parameters;

        // Interface implementation
        public double PricingError { get { return _PricingError; } }
        public CalibrationOutcome MinimizerStatus { get { return _MinimizerStatus; } }
        public IHestonModelParameters Parameters { get { return (IHestonModelParameters)_Parameters; } }

        /// <summary>
        /// Initializes a new instance of the
        /// <see cref="T:HestonModel.Classes.InterfaceClasses.HestonCalibrationResult"/> class.
        /// </summary>
        /// <param name="pricingError">Pricing error.</param>
        /// <param name="status">Calibration Status.</param>
        /// <param name="parameters">Result Parameters.</param>
        public HestonCalibrationResult(double pricingError,
        CalibrationOutcome status, HestonModelParameters parameters)
        {
            _PricingError = pricingError;
            _MinimizerStatus = status;
            _Parameters = parameters;
        }
    }
}
