using System;
using System.Collections.Generic;
using HestonModel.Interfaces;
using HestonModel.Classes;
using HestonModel.Classes.InterfaceClasses;

namespace HestonModel
{

    /// <summary> 
    /// This class will be used for grading. 
    /// Don't remove any of the methods and don't modify their signatures. Don't change the namespace. 
    /// Your code should be implemented in other classes (or even projects if you wish), and the relevant functionality should only be called here and outputs returned.
    /// You don't need to implement the interfaces that have been provided if you don't want to.
    /// </summary>
    public static class Heston
    {
        /// <summary>
        /// Method for calibrating the heston model.
        /// </summary>
        /// <param name="guessModelParameters">Object implementing IHestonModelParameters interface containing the risk-free rate, initial stock price
        /// and initial guess parameters to be used in the calibration.</param>
        /// <param name="referenceData">A collection of objects implementing IOptionMarketData<IEuropeanOption> interface. These should contain the reference data used for calibration.</param>
        /// <param name="calibrationSettings">An object implementing ICalibrationSettings interface.</param>
        /// <returns>Object implementing IHestonCalibrationResult interface which contains calibrated model parameters and additional diagnostic information</returns>
        public static IHestonCalibrationResult CalibrateHestonParameters(IHestonModelParameters guessModelParameters, IEnumerable<IOptionMarketData<IEuropeanOption>> referenceData, ICalibrationSettings calibrationSettings)
        {
            try
            {
                // Copy Linked List
                LinkedList<OptionMarketData<EuropeanOption>> data =
                    new LinkedList<OptionMarketData<EuropeanOption>>();
                foreach (OptionMarketData<IEuropeanOption> marketData in referenceData)
                {
                    OptionMarketData<EuropeanOption> newMarketData =
                    new OptionMarketData<EuropeanOption>((EuropeanOption)marketData.Option, marketData.Price);
                    data.AddLast(newMarketData);
                }
                // Heston Calibrator
                HestonCalibrator calibrator =
                new HestonCalibrator((HestonModelParameters)guessModelParameters,
                                     data,
                                     (CalibrationSettings)calibrationSettings);
                // Calibration procedure
                calibrator.Calibrate();
                // Retrieve outcome
                double error = 0;
                CalibrationOutcome outcome = CalibrationOutcome.NotStarted;
                calibrator.GetCalibrationStatus(ref outcome, ref error);
                HestonModelParameters parameters = calibrator.GetCalibratedModel();
                // Form calibration result object
                HestonCalibrationResult result = new HestonCalibrationResult(error, outcome, parameters);

                return result;
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }

        /// <summary>
        /// Price a European option in the Heston model using the Heston formula. This should be accurate to 5 decimal places
        /// </summary>
        /// <param name="parameters">Object implementing IHestonModelParameters interface, containing model parameters.</param>
        /// <param name="europeanOption">Object implementing IEuropeanOption interface, containing the option parameters.</param>
        /// <returns>Option price</returns>
        public static double HestonEuropeanOptionPrice(IHestonModelParameters parameters, IEuropeanOption europeanOption)
        {
            try
            {
                // Create European Option Formula object
                EuropeanOptionFormula euFormula = new EuropeanOptionFormula((HestonModelParameters)parameters, (EuropeanOption)europeanOption);
                return euFormula.Price();
            }
            catch (Exception e)
            {
                throw e;
            }

        }

        /// <summary>
        /// Price a European option in the Heston model using the Monte-Carlo method. Accuracy will depend on number of time steps and samples
        /// </summary>
        /// <param name="parameters">Object implementing IHestonModelParameters interface, containing model parameters.</param>
        /// <param name="europeanOption">Object implementing IEuropeanOption interface, containing the option parameters.</param>
        /// <param name="monteCarloSimulationSettings">An object implementing IMonteCarloSettings object and containing simulation settings.</param>
        /// <returns>Option price</returns>
        public static double HestonEuropeanOptionPriceMC(IHestonModelParameters parameters, IEuropeanOption europeanOption, IMonteCarloSettings monteCarloSimulationSettings)
        {
            try
            {
                // Create Monte Carlo EU option object
                EuropeanOptionMC euOptionMC = new EuropeanOptionMC((HestonModelParameters)parameters,
                (MonteCarloSettings)monteCarloSimulationSettings, (EuropeanOption)europeanOption);

                return euOptionMC.Price(Environment.ProcessorCount);
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }

        /// <summary>
        /// Price a Asian option in the Heston model using the 
        /// Monte-Carlo method. Accuracy will depend on number of time steps and samples</summary>
        /// <param name="parameters">Object implementing IHestonModelParameters interface, containing model parameters.</param>
        /// <param name="asianOption">Object implementing IAsian interface, containing the option parameters.</param>
        /// <param name="monteCarloSimulationSettings">An object implementing IMonteCarloSettings object and containing simulation settings.</param>
        /// <returns>Option price</returns>
        public static double HestonAsianOptionPriceMC(IHestonModelParameters parameters, IAsianOption asianOption, IMonteCarloSettings monteCarloSimulationSettings)
        {
            try
            {
                // Create Monte Carlo Asian option object
                AsianOptionMC asianOptionMC = new AsianOptionMC((HestonModelParameters)parameters,
                (MonteCarloSettings)monteCarloSimulationSettings, (AsianOption)asianOption);

                return asianOptionMC.Price(Environment.ProcessorCount);
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }

        /// <summary>
        /// Price a lookback option in the Heston model using the  
        /// a Monte-Carlo method. Accuracy will depend on number of time steps and samples </summary>
        /// <param name="parameters">Object implementing IHestonModelParameters interface, containing model parameters.</param>
        /// <param name="maturity">An object implementing IOption interface and containing option's maturity</param>
        /// <param name="monteCarloSimulationSettings">An object implementing IMonteCarloSettings object and containing simulation settings.</param>
        /// <returns>Option price</returns>
        public static double HestonLookbackOptionPriceMC(IHestonModelParameters parameters, IOption maturity, IMonteCarloSettings monteCarloSimulationSettings)
        {
            try
            {
                // Typecast maturity
                Option tempOption = (Option)maturity;

                // Create Monte Carlo Lookback option object
                LookbackOptionMC lookbackOptionMC =
                new LookbackOptionMC((HestonModelParameters)parameters,
                (MonteCarloSettings)monteCarloSimulationSettings, maturity.Maturity);

                return lookbackOptionMC.Price(Environment.ProcessorCount);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }       
    }
}
