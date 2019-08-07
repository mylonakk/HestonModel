using System;
using HestonModel.Interfaces;

namespace HestonModel.Classes.InterfaceClasses
{
    /// <summary>
    /// Class representing calibration setting for Heston Model.
    /// </summary>
    public class CalibrationSettings : ICalibrationSettings
    {
        // Private Properties
        private double _Accuracy;
        private int _MaximumNumberOfIterations;

        // Interface Implementation
        public double Accuracy
        {
            get { return _Accuracy; }
        }
        public int MaximumNumberOfIterations
        {
            get { return _MaximumNumberOfIterations; }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="T:HestonModel.Classes.InterfaceClasses.CalibrationSettings"/> class.
        /// </summary>
        /// <param name="accuracy">Accuracy.</param>
        /// <param name="maxIter">Max iterations</param>
        public CalibrationSettings(double accuracy, int maxIter)
        {
            // Sanity Check
            if (accuracy <= 0)
            {
                throw new Exception("The accuracy for the calibrator must be positive.");
            }
            if (maxIter <= 0)
            {
                throw new Exception("The max number of iterations for the calibrator must be a positive number.");
            }

            _Accuracy = accuracy;
            _MaximumNumberOfIterations = maxIter;
        }
    }
}
