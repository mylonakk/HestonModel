using System;
using HestonModel.Interfaces;

namespace HestonModel.Classes.InterfaceClasses
{
    /// <summary>
    /// Class representing variance paramenters for Heston Model
    /// </summary>
    public class VarianceProcessParameters : IVarianceProcessParameters
    {
        // Variance Process Parameters
        protected double _Kappa;
        protected double _Theta;
        protected double _Sigma;
        protected double _V0;
        protected double _Rho;

        // Equation Vars
        protected double[] _u = new double[2];
        protected double _alpha;
        protected double[] _b = new double[2];

        public double Kappa
        {
            get { return _Kappa; }
        }

        public double Theta
        {
            get { return _Theta; }
        }

        public double Sigma
        {
            get { return _Sigma; }
        }

        public double V0
        {
            get { return _V0; }
        }

        public double Rho
        {
            get { return _Rho; }
        }

        public double Alpha
        {
            get { return _alpha; }
        }

        public double[] U
        {
            get { return _u; }
        }

        public double[] B
        {
            get { return _b; }
        }

        /// <summary>
        /// Initializes a new instance of the
        /// <see cref="T:HestonModel.Classes.InterfaceClasses.VarianceProcessParameters"/> class.
        /// </summary>
        /// <param name="Kappa">Kappa.</param>
        /// <param name="Theta">Theta.</param>
        /// <param name="Sigma">Sigma.</param>
        /// <param name="V0">V0.</param>
        /// <param name="Rho">Rho.</param>
        public VarianceProcessParameters(double Kappa, double Theta,
                                         double Sigma, double V0,
                                         double Rho)
        {
            // Check Sanity for parameters
            if (Sigma < 0)
            {
                throw new Exception("Parameter sigma cannot be a negative number.");
            }
            if (V0 < 0)
            {
                throw new Exception("Volatility cannot be a negative number.");
            }

            // Initialize Heston
            _Kappa = Kappa;
            _Theta = Theta;
            _Sigma = Sigma;
            _V0 = V0;
            _Rho = Rho;

            // Initialize Equation helping vars
            _alpha = _Kappa * _Theta;
            _b[0] = _Kappa - _Rho * _Sigma;
            _b[1] = _Kappa;
            _u[0] = 0.5;
            _u[1] = -0.5;
        }
    }
}
