using System;
using HestonModel.Interfaces; 
using HestonModel.Classes.InterfaceClasses;

namespace HestonModel.Classes
{
    /// <summary>
    /// Class to represent a Lookback option which is priced using Monte Carlo
    ///  method in Heston model. This class inherits the HestonMC class and the
    ///  IOption interface.
    /// </summary>
    public class LookbackOptionMC : HestonMC
    {

        /// <summary>
        /// Initializes a new instance of the LookbackOptionMC class.
        /// </summary>
        /// <param name="parameters">Interface holding Heston Model params.</param>
        /// <param name="monteCarloSimulationSettings">Interface holding Monte carlo simulation settings.</param>
        /// <param name="maturity">Interface holding maturity.</param>
        public LookbackOptionMC(HestonModelParameters parameters,
                                MonteCarloSettings monteCarloSimulationSettings,
                                double maturity)
            : base(parameters, monteCarloSimulationSettings, maturity)
        {}

        /// <summary>
        /// Payoff Function for the Lookback option.
        /// </summary>
        /// <returns>The payoff.</returns>
        /// <param name="S">S.</param>
        private double payoff(double[] S)
        {
            double min = double.MaxValue;

            for (int i = 0; i < S.Length; i++)
            {
                if (S[i] < min)
                {
                    min = S[i];
                }
            }
            return S[S.Length - 1] - min;
        }

        /// <summary>
        /// Price Lookback option..
        /// </summary>
        /// <returns>The price.</returns>
        /// <param name="workers">Number of threads.</param>
        public double Price(int workers)
        {
            return GenPaths(payoff, workers);
        }
    }
}
