using System;
using System.Collections.Generic;
using System.Linq;
using HestonModel.Interfaces;

namespace HestonModel.Classes.InterfaceClasses
{
    /// <summary>
    /// Class Implementing Asian Option Interface.
    /// </summary>
    public class AsianOption : EuropeanOption, IAsianOption
    {
        // Properties
        private IEnumerable<double> _MonitoringTimes;

        // Interface implementation
        public IEnumerable<double> MonitoringTimes
        {
            get { return _MonitoringTimes; }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="T:HestonModel.Classes.InterfaceClasses.AsianOption"/> class.
        /// </summary>
        /// <param name="StrikePrice">Strike price.</param>
        /// <param name="Type">Type (Put / Call)</param>
        /// <param name="Maturity">Maturity</param>
        /// <param name="MonitoringTimes">Monitoring times</param>
        public AsianOption(double StrikePrice, PayoffType Type,
                           double Maturity, IEnumerable<double> MonitoringTimes) 
            : base(StrikePrice, Type, Maturity)
        {
            // Sanity Check
            if (MonitoringTimes.Any())
            {
                double max = MonitoringTimes.Max();

                if (max > Maturity)
                {
                    throw new Exception("All monitoring time have to be less of equal than the maturity time");
                }

                double min = MonitoringTimes.Min();
                if (min < 0)
                {
                    throw new Exception("All monitoring time have to be nonnegative");
                }
            }
            else
            {
                throw new Exception("Provide at least one monitoring time for the Asian option pricing");
            }

            _MonitoringTimes = MonitoringTimes;
        }
    }
}
