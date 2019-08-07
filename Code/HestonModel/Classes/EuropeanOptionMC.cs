using System;
using HestonModel.Interfaces;
using HestonModel.Classes.InterfaceClasses;

namespace HestonModel.Classes
{
    /// <summary>
    /// Class to represent an European option which is priced using Monte Carlo
    ///  method in Heston model. This class inherits the HestonMC class and the
    ///  IEuropeanOption interface.
    /// </summary>
    public class EuropeanOptionMC : HestonMC, IEuropeanOption
    {

        // Properties
        private double _StrikePrice;
        private PayoffType _Type;

        // Interface Implementation
        public double StrikePrice
        {
            get { return _StrikePrice; }
        }

        public PayoffType Type
        {
            get { return _Type; }
        }


        /// <summary>
        /// Initializes a new instance of the EuropeanOptionMC class.
        /// </summary>
        /// <param name="parameters">Interface holding Heston Model params.</param>
        /// <param name="monteCarloSimulationSettings">Interface holding Monte carlo simulation settings.</param>
        /// <param name="europeanOption">Interface holding European option params.</param>
        public EuropeanOptionMC(HestonModelParameters parameters,
                                MonteCarloSettings monteCarloSimulationSettings,
                                EuropeanOption europeanOption) 
            : base(parameters, monteCarloSimulationSettings, europeanOption.Maturity)
        {
            _StrikePrice = europeanOption.StrikePrice;
            _Type = europeanOption.Type;
        }

        /// <summary>
        /// Price European option using Monte Carlo Method.
        /// </summary>
        /// <returns>The price.</returns>
        /// <param name="workers">Number of Workers.</param>
        public double Price(int workers)
        {
            Func<double[], double> payoff;

            if (Type == PayoffType.Call)
            {
                // Call - Payoff function
                payoff = S => Math.Max(S[S.Length - 1] - _StrikePrice, 0);
            }
            else
            {
                // Put - Payoff function
                payoff = S => Math.Max(_StrikePrice - S[S.Length - 1], 0);
            }

            return GenPaths(payoff, workers);
        }
    }
}
