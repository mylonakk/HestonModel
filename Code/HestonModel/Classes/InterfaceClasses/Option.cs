using System;
using HestonModel.Interfaces;

namespace HestonModel.Classes.InterfaceClasses
{
    /// <summary>
    /// Class implementing Option interface.
    /// </summary>
    public class Option : IOption
    {
        private double _Maturity;

        // Interface Implementation
        public double Maturity
        {
            get { return _Maturity; }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="T:HestonModel.Classes.InterfaceClasses.Option"/> class.
        /// </summary>
        /// <param name="Maturity">Maturity.</param>
        public Option(double Maturity)
        {
            // Sanity Check
            if (Maturity <= 0)
            {
                throw new Exception("Maturity must be a positive number.");
            }
            _Maturity = Maturity;
        }
    }
}
