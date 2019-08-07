using System;
using HestonModel.Interfaces;
using System.Collections.Generic;
using HestonModel.Classes.InterfaceClasses;

namespace HestonModel.Classes
{
    /// <summary>
    /// Class to represent an Asian option which is priced using Monte Carlo
    ///  method in Heston model. This class inherits the HestonMC class and the
    ///  IAsianOption interface.
    /// </summary>
    public class AsianOptionMC : EuropeanOptionMC, IAsianOption
    {
        // Properties
        private IEnumerable<double> _MonitoringTimes;

        // Interface implementation
        public IEnumerable<double> MonitoringTimes
        {
            get { return _MonitoringTimes; }
        }

        /// <summary>
        /// Gets the monitored avg.
        /// </summary>
        /// <returns>The monitored avg.</returns>
        /// <param name="S">S.</param>
        private double GetMonitoredAvg(double[] S)
        {
            double tau = Maturity / NumberOfTimeSteps;
            double sum = 0;
            int count = 0;
            foreach (double t in MonitoringTimes)
            {
                sum += S[(int)Math.Floor(t / tau) - 1];
                count++;
            }
            return sum / count;
        }

        /// <summary>
        /// Calls the payoff.
        /// </summary>
        /// <returns>The payoff.</returns>
        /// <param name="S">S.</param>
        private double CallPayoff(double[] S)
        {
            return Math.Max(GetMonitoredAvg(S) - StrikePrice, 0);
        }

        /// <summary>
        /// Puts the payoff.
        /// </summary>
        /// <returns>The payoff.</returns>
        /// <param name="S">S.</param>
        private double PutPayoff(double[] S)
        {
            return Math.Max(0, GetMonitoredAvg(S) - StrikePrice);
        }

        /// <summary>
        /// Initializes a new instance of the EuropeanOptionMC class.
        /// </summary>
        /// <param name="parameters">Interface holding Heston Model params.</param>
        /// <param name="monteCarloSimulationSettings">Interface holding Monte carlo simulation settings.</param>
        /// <param name="asianOption">Interface holding asian option.</param>
        public AsianOptionMC(HestonModelParameters parameters,
                             MonteCarloSettings monteCarloSimulationSettings,
                             AsianOption asianOption)
            : base(parameters, monteCarloSimulationSettings, asianOption)
        {
            _MonitoringTimes = asianOption.MonitoringTimes;
        }

        /// <summary>
        /// Price this instance.
        /// </summary>
        /// <returns>The price.</returns>
        public new double Price(int workers)
        {
            if (Type == PayoffType.Call)
            {
                return GenPaths(CallPayoff, workers);
            }
            else
            {
                return GenPaths(PutPayoff, workers);
            }
        }
    }
}
