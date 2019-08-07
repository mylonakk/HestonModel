using System;
using HestonModel.Interfaces;

namespace HestonModel.Classes.InterfaceClasses
{
    /// <summary>
    /// Class representing settings for Monte Carlo
    /// </summary>
    public class MonteCarloSettings : IMonteCarloSettings
    {
        // Class Properties
        private int _NumberOfTrials;
        private int _NumberOfTimeSteps;

        // Implementation of Interface
        public int NumberOfTrials
        {
            get { return _NumberOfTrials; }
        }

        public int NumberOfTimeSteps
        {
            get { return _NumberOfTimeSteps; }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="T:HestonModel.Classes.InterfaceClasses.MonteCarloSettings"/> class.
        /// </summary>
        /// <param name="NumberOfTrials">Number of trials.</param>
        /// <param name="NumberOfTimeSteps">Number of time steps.</param>
        public MonteCarloSettings(int NumberOfTrials, int NumberOfTimeSteps)
        {
            // Sanity Check
            if (NumberOfTrials <= 0)
            {
                throw new Exception("Number of trials of MC method must be a positive number.");
            }
            if (NumberOfTimeSteps <= 0)
            {
                throw new Exception("Number of time steps of MC method must be a positive number.");
            }

            _NumberOfTrials = NumberOfTrials;
            _NumberOfTimeSteps = NumberOfTimeSteps;
        }
    }
}
