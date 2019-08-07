using System;
using System.Numerics;
using HestonModel.Interfaces;
using HestonModel.Classes.InterfaceClasses;
using MathNet.Numerics.Integration;

namespace HestonModel.Classes
{
    /// <summary>
    /// Class to represent an European Option in Heston model. This class 
    /// inherits the HestonOption class and the IEuropeanOption interface.
    /// </summary>
    /// Todo: add exceptions, better documentation, put pricing"
    public class EuropeanOptionFormula : HestonModelParameters, IEuropeanOption
    {
        // Properties
        private double _StrikePrice;
        private PayoffType _Type;
        private double _Maturity;

        // Interface Implementation
        public double StrikePrice
        {
            get { return _StrikePrice; }
        }

        public PayoffType Type
        {
            get { return _Type; }
        }

        public double Maturity
        {
            get { return _Maturity; }
        }

        /// <summary>
        /// Initializes a new instance of the European option
        /// </summary>
        /// <param name="parameters">Heston Model Parameter Object</param>
        /// <param name="europeanOption">European option Parameter Object</param>
        public EuropeanOptionFormula(HestonModelParameters parameters,
                                     EuropeanOption europeanOption) 
            : base(parameters.InitialStockPrice, parameters.RiskFreeRate,
                   parameters.GetVariance())
        {
            _StrikePrice = europeanOption.StrikePrice;
            _Type = europeanOption.Type;
            _Maturity = europeanOption.Maturity;
        }

        /// <summary>
        /// G the specified j and phi.
        /// </summary>
        /// <returns>The g.</returns>
        /// <param name="j">J.</param>
        /// <param name="phi">Phi.</param>
        private Complex g(int j, double phi)
        {
            double Rho = _VarianceParameters.Rho;
            double Sigma = _VarianceParameters.Sigma;
            double[] b = _VarianceParameters.B;

            Complex A = new Complex(b[j - 1], - Rho * Sigma * phi) - d(j, phi);
            Complex B = new Complex(b[j - 1], - Rho * Sigma * phi) + d(j, phi);
            return A / B;
        }

        /// <summary>
        /// D the specified j and phi.
        /// </summary>
        /// <returns>The d.</returns>
        /// <param name="j">J.</param>
        /// <param name="phi">Phi.</param>
        private Complex d(int j, double phi)
        {
            double Rho = _VarianceParameters.Rho;
            double Sigma = _VarianceParameters.Sigma;
            double[] b = _VarianceParameters.B;
            double[] u = _VarianceParameters.U;

            Complex A = new Complex(-b[j - 1], Rho * Sigma * phi);
            Complex B = Math.Pow(Sigma, 2) *
                        new Complex(-Math.Pow(phi, 2), 2 * u[j - 1] * phi);

            return Complex.Sqrt(Complex.Pow(A, 2) - B);
        }
        /// <summary>
        /// C the specified j, t and phi.
        /// </summary>
        /// <returns>The c.</returns>
        /// <param name="j">J.</param>
        /// <param name="phi">Phi.</param>
        private Complex C(int j, double phi)
        {

            double Rho = _VarianceParameters.Rho;
            double Sigma = _VarianceParameters.Sigma;
            double[] _b = _VarianceParameters.B;
            double alpha = _VarianceParameters.Alpha;
            double tau = _Maturity;

            Complex A = new Complex(0, _RiskFreeRate * phi * tau);

            // ln[\frac{1 - g_j (\phi)*exp^{\tau}}{-r*d_j(\phi)}{1 - g_j(\phi)}]
            Complex B2 = 2 * Complex.Log((1 - g(j, phi) *
                              Complex.Exp(-tau * d(j, phi))) /
                              (1 - g(j, phi)));

            Complex B = alpha / Math.Pow(Sigma, 2) *
                        (new Complex(_b[j - 1], -Rho * Sigma * phi) * tau -
                         d(j, phi) * tau - B2);

            return A + B;
        }

        /// <summary>
        /// D the specified j, t and phi.
        /// </summary>
        /// <returns>The d.</returns>
        /// <param name="j">J.</param>
        /// <param name="phi">Phi.</param>
        private Complex D(int j, double phi)
        {
            double Rho = _VarianceParameters.Rho;
            double Sigma = _VarianceParameters.Sigma;
            double[] _b = _VarianceParameters.B;
            double tau = _Maturity;

            Complex A = new Complex(_b[j-1], -Rho*Sigma*phi) - d(j, phi);
            Complex B1 = 1 - Complex.Exp(-tau * d(j, phi));
            Complex B2 = 1 - g(j, phi)*Complex.Exp(-tau * d(j, phi));
            return  A * (B1 / B2) / Math.Pow(Sigma, 2);
        }

        /// <summary>
        /// Calculates the p.
        /// </summary>
        /// <returns>The p.</returns>
        private double P(int j)
        {
            double v = _VarianceParameters.V0;
            double x = Math.Log(_InitialStockPrice);

            // Integrand
            Func<double, double> integrad = phi => 
            ((Complex.Exp(new Complex(0, -phi * Math.Log(_StrikePrice))) *
            (Complex.Exp(C(j, phi) + D(j, phi) * v + new Complex(0, phi * x)))) /
            new Complex(0, phi)).Real;

            double integral = NewtonCotesTrapeziumRule.IntegrateAdaptive(integrad, 1e-6, 1000, 1e-5);

            return 0.5 + (integral / Math.PI);
        }

        /// <summary>
        /// Price this instance.
        /// </summary>
        /// <returns>The price.</returns>
        public double Price()
        {
            double A = _InitialStockPrice * P(1);
            double B = _StrikePrice * Math.Exp(-_RiskFreeRate * _Maturity ) * P(2);

            if (Type == PayoffType.Call)
            {
                return A - B;
            }
            else
            {   
                // Put - Call Parity
                return (A - B) - InitialStockPrice + StrikePrice *
                Math.Exp(-RiskFreeRate * Maturity);
            }

        }
    }
}
