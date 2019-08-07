using System;
using HestonModel.Interfaces;

namespace HestonModel.Classes.InterfaceClasses
{
    /// <summary>
    /// Class Implementing European Option interface
    /// </summary>
    public class EuropeanOption : IEuropeanOption
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
        /// Initializes a new instance of the <see cref="T:HestonModel.Classes.InterfaceClasses.EuropeanOption"/> class.
        /// </summary>
        /// <param name="StrikePrice">Strike price.</param>
        /// <param name="Type">Option Type (Put/Call).</param>
        /// <param name="Maturity">Maturity.</param>
        public EuropeanOption(double StrikePrice, PayoffType Type, double Maturity)
        {
            // Sanity Check
            if (Maturity <= 0)
            {
                throw new Exception("Maturity must be a positive number.");
            }
            if (StrikePrice <= 0)
            {
                throw new Exception("Strike price must be a positive number.");
            }

            // Initialize private vars
            _StrikePrice = StrikePrice;
            _Type = Type;
            _Maturity = Maturity;
        }
    }
}
