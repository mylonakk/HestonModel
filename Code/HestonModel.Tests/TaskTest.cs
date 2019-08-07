using System;
using NUnit.Framework;
using HestonModel.Classes.InterfaceClasses;
using HestonModel.Classes;
using System.Collections.Generic;

namespace HestonModel.Tests
{
    [TestFixture]
    public class TaskTest
    {
        [Test]
        public void Task_2_2_1()
        {
            // Variance Process Values
            double Kappa = 1.5768;
            double Theta = 0.0398;
            double Sigma = 0.5751;
            double V0 = 0.0175;
            double Rho = -0.5711;

            // Heston Model Params
            double InitialStockPrice = 100;
            double RiskFreeRate = 0.025;

            // Option Params
            double StrikePrice = 100;
            PayoffType Type = PayoffType.Call;
            double Maturity = 1;

            VarianceProcessParameters varParams =
            new VarianceProcessParameters(Kappa, Theta, Sigma, V0, Rho);

            HestonModelParameters hestonModel =
            new HestonModelParameters(InitialStockPrice, RiskFreeRate, varParams);

            EuropeanOption euOption =
            new EuropeanOption(StrikePrice, Type, Maturity);

            EuropeanOptionFormula euFormula =
                new EuropeanOptionFormula(hestonModel, euOption);

            Assert.AreEqual(7.2743, euFormula.Price(), 1e-4);
        }

        [Test]
        public void Task_2_2_2()
        {
            // Variance Process Values
            double Kappa = 1.5768;
            double Theta = 0.0398;
            double Sigma = 0.5751;
            double V0 = 0.0175;
            double Rho = -0.5711;

            // Heston Model Params
            double InitialStockPrice = 100;
            double RiskFreeRate = 0.025;

            // Option Params
            double StrikePrice = 100;
            PayoffType Type = PayoffType.Call;
            double Maturity = 2;

            VarianceProcessParameters varParams =
            new VarianceProcessParameters(Kappa, Theta, Sigma, V0, Rho);

            HestonModelParameters hestonModel =
            new HestonModelParameters(InitialStockPrice, RiskFreeRate, varParams);

            EuropeanOption euOption =
            new EuropeanOption(StrikePrice, Type, Maturity);

            EuropeanOptionFormula euFormula =
                new EuropeanOptionFormula(hestonModel, euOption);

            Assert.AreEqual(11.7373, euFormula.Price(), 1e-4);
        }

        [Test]
        public void Task_2_2_3()
        {
            // Variance Process Values
            double Kappa = 1.5768;
            double Theta = 0.0398;
            double Sigma = 0.5751;
            double V0 = 0.0175;
            double Rho = -0.5711;

            // Heston Model Params
            double InitialStockPrice = 100;
            double RiskFreeRate = 0.025;

            // Option Params
            double StrikePrice = 100;
            PayoffType Type = PayoffType.Call;
            double Maturity = 3;

            VarianceProcessParameters varParams =
            new VarianceProcessParameters(Kappa, Theta, Sigma, V0, Rho);

            HestonModelParameters hestonModel =
            new HestonModelParameters(InitialStockPrice, RiskFreeRate, varParams);

            EuropeanOption euOption =
            new EuropeanOption(StrikePrice, Type, Maturity);

            EuropeanOptionFormula euFormula =
                new EuropeanOptionFormula(hestonModel, euOption);

            Assert.AreEqual(15.4793, euFormula.Price(), 1e-4);
        }

        [Test]
        public void Task_2_2_4()
        {
            // Variance Process Values
            double Kappa = 1.5768;
            double Theta = 0.0398;
            double Sigma = 0.5751;
            double V0 = 0.0175;
            double Rho = -0.5711;

            // Heston Model Params
            double InitialStockPrice = 100;
            double RiskFreeRate = 0.025;

            // Option Params
            double StrikePrice = 100;
            PayoffType Type = PayoffType.Call;
            double Maturity = 4;

            VarianceProcessParameters varParams =
            new VarianceProcessParameters(Kappa, Theta, Sigma, V0, Rho);

            HestonModelParameters hestonModel =
            new HestonModelParameters(InitialStockPrice, RiskFreeRate, varParams);

            EuropeanOption euOption =
            new EuropeanOption(StrikePrice, Type, Maturity);

            EuropeanOptionFormula euFormula =
                new EuropeanOptionFormula(hestonModel, euOption);

            Assert.AreEqual(18.7742, euFormula.Price(), 1e-4);
        }

        [Test]
        public void Task_2_2_15()
        {
            // Variance Process Values
            double Kappa = 1.5768;
            double Theta = 0.0398;
            double Sigma = 0.5751;
            double V0 = 0.0175;
            double Rho = -0.5711;

            // Heston Model Params
            double InitialStockPrice = 100;
            double RiskFreeRate = 0.025;

            // Option Params
            double StrikePrice = 100;
            PayoffType Type = PayoffType.Call;
            double Maturity = 15;

            VarianceProcessParameters varParams =
            new VarianceProcessParameters(Kappa, Theta, Sigma, V0, Rho);

            HestonModelParameters hestonModel =
            new HestonModelParameters(InitialStockPrice, RiskFreeRate, varParams);

            EuropeanOption euOption =
            new EuropeanOption(StrikePrice, Type, Maturity);

            EuropeanOptionFormula euFormula =
                new EuropeanOptionFormula(hestonModel, euOption);

            Assert.AreEqual(43.1705, euFormula.Price(), 1e-4);
        }

        // ****************************************
        //              Task 2.3
        // ****************************************

        [Test]
        public void Task_2_3_1()
        {
            // Variance Process Values
            double Kappa = 2;
            double Theta = 0.06;
            double Sigma = 0.4;
            double V0 = 0.04;
            double Rho = 0.5;

            // Heston Model Params
            double InitialStockPrice = 100;
            double RiskFreeRate = 0.1;

            // Option Params
            double StrikePrice = 100;
            PayoffType Type = PayoffType.Call;
            double Maturity = 1;

            // MC Simulation Params
            int NumberOfTrials = (int)1e5;
            int NumberOfTimeSteps = (int)Math.Ceiling(365 * Maturity);

            VarianceProcessParameters varParams =
            new VarianceProcessParameters(Kappa, Theta, Sigma, V0, Rho);

            HestonModelParameters hestonModel =
            new HestonModelParameters(InitialStockPrice, RiskFreeRate, varParams);

            EuropeanOption euOption =
            new EuropeanOption(StrikePrice, Type, Maturity);

            MonteCarloSettings monteCarloSettings =
            new MonteCarloSettings(NumberOfTrials, NumberOfTimeSteps);

            EuropeanOptionMC euOptionMC = new EuropeanOptionMC(hestonModel,
            monteCarloSettings, euOption);

            Assert.AreEqual(13.6299, euOptionMC.Price(1), 1e-1);
        }

        [Test]
        public void Task_2_3_2()
        {
            // Variance Process Values
            double Kappa = 2;
            double Theta = 0.06;
            double Sigma = 0.4;
            double V0 = 0.04;
            double Rho = 0.5;

            // Heston Model Params
            double InitialStockPrice = 100;
            double RiskFreeRate = 0.1;

            // Option Params
            double StrikePrice = 100;
            PayoffType Type = PayoffType.Call;
            double Maturity = 2;

            // MC Simulation Params
            int NumberOfTrials = (int)1e5;
            int NumberOfTimeSteps = (int)Math.Ceiling(365 * Maturity);

            VarianceProcessParameters varParams =
            new VarianceProcessParameters(Kappa, Theta, Sigma, V0, Rho);

            HestonModelParameters hestonModel =
            new HestonModelParameters(InitialStockPrice, RiskFreeRate, varParams);

            EuropeanOption euOption =
            new EuropeanOption(StrikePrice, Type, Maturity);

            MonteCarloSettings monteCarloSettings =
            new MonteCarloSettings(NumberOfTrials, NumberOfTimeSteps);

            EuropeanOptionMC euOptionMC = new EuropeanOptionMC(hestonModel,
            monteCarloSettings, euOption);

            Assert.AreEqual(22.4529, euOptionMC.Price(1), 1e-1);
        }

        [Test]
        public void Task_2_3_3()
        {
            // Variance Process Values
            double Kappa = 2;
            double Theta = 0.06;
            double Sigma = 0.4;
            double V0 = 0.04;
            double Rho = 0.5;

            // Heston Model Params
            double InitialStockPrice = 100;
            double RiskFreeRate = 0.1;

            // Option Params
            double StrikePrice = 100;
            PayoffType Type = PayoffType.Call;
            double Maturity = 3;

            // MC Simulation Params
            int NumberOfTrials = (int)1e5;
            int NumberOfTimeSteps = (int)Math.Ceiling(365 * Maturity);

            VarianceProcessParameters varParams =
            new VarianceProcessParameters(Kappa, Theta, Sigma, V0, Rho);

            HestonModelParameters hestonModel =
            new HestonModelParameters(InitialStockPrice, RiskFreeRate, varParams);

            EuropeanOption euOption =
            new EuropeanOption(StrikePrice, Type, Maturity);

            MonteCarloSettings monteCarloSettings =
            new MonteCarloSettings(NumberOfTrials, NumberOfTimeSteps);

            EuropeanOptionMC euOptionMC = new EuropeanOptionMC(hestonModel,
            monteCarloSettings, euOption);

            Assert.AreEqual(29.9957, euOptionMC.Price(1), 1e-1);
        }

        [Test]
        public void Task_2_3_4()
        {
            // Variance Process Values
            double Kappa = 2;
            double Theta = 0.06;
            double Sigma = 0.4;
            double V0 = 0.04;
            double Rho = 0.5;

            // Heston Model Params
            double InitialStockPrice = 100;
            double RiskFreeRate = 0.1;

            // Option Params
            double StrikePrice = 100;
            PayoffType Type = PayoffType.Call;
            double Maturity = 4;

            // MC Simulation Params
            int NumberOfTrials = (int)1e5;
            int NumberOfTimeSteps = (int)Math.Ceiling(365 * Maturity);

            VarianceProcessParameters varParams =
            new VarianceProcessParameters(Kappa, Theta, Sigma, V0, Rho);

            HestonModelParameters hestonModel =
            new HestonModelParameters(InitialStockPrice, RiskFreeRate, varParams);

            EuropeanOption euOption =
            new EuropeanOption(StrikePrice, Type, Maturity);

            MonteCarloSettings monteCarloSettings =
            new MonteCarloSettings(NumberOfTrials, NumberOfTimeSteps);

            EuropeanOptionMC euOptionMC = new EuropeanOptionMC(hestonModel,
            monteCarloSettings, euOption);

            Assert.AreEqual(36.6553, euOptionMC.Price(1), 1e-1);
        }

        [Test]
        public void Task_2_3_15()
        {
            // Variance Process Values
            double Kappa = 2;
            double Theta = 0.06;
            double Sigma = 0.4;
            double V0 = 0.04;
            double Rho = 0.5;

            // Heston Model Params
            double InitialStockPrice = 100;
            double RiskFreeRate = 0.1;

            // Option Params
            double StrikePrice = 100;
            PayoffType Type = PayoffType.Call;
            double Maturity = 15;

            // MC Simulation Params
            int NumberOfTrials = (int)1e5;
            int NumberOfTimeSteps = (int)Math.Ceiling(365 * Maturity);

            VarianceProcessParameters varParams =
            new VarianceProcessParameters(Kappa, Theta, Sigma, V0, Rho);

            HestonModelParameters hestonModel =
            new HestonModelParameters(InitialStockPrice, RiskFreeRate, varParams);

            EuropeanOption euOption =
            new EuropeanOption(StrikePrice, Type, Maturity);

            MonteCarloSettings monteCarloSettings =
            new MonteCarloSettings(NumberOfTrials, NumberOfTimeSteps);

            EuropeanOptionMC euOptionMC = new EuropeanOptionMC(hestonModel,
            monteCarloSettings, euOption);

            Assert.AreEqual(78.4306, euOptionMC.Price(1), 1e-1);
        }

        // **************************
        // Task 2.5
        // **************************

        [Test]
        public void Task_2_5()
        {
            // Variance Process Values
            double Kappa = 1.5768;
            double Theta = 0.398;
            double Sigma = 0.5751;
            double V0 = 0.0175;
            double Rho = -0.5711;

            // Heston Model Params
            double InitialStockPrice = 100;
            double RiskFreeRate = 0.025;

            // Callibration Settings
            double accuracy = 0.001;
            int maxIter = 1000;

            VarianceProcessParameters varParams =
            new VarianceProcessParameters(Kappa, Theta, Sigma, V0, Rho);

            HestonModelParameters hestonModel =
            new HestonModelParameters(InitialStockPrice, RiskFreeRate, varParams);

            CalibrationSettings calibrationSettings = new CalibrationSettings(accuracy, maxIter);

            // Market Data
            LinkedList<OptionMarketData<EuropeanOption>> observedOptions =
                new LinkedList<OptionMarketData<EuropeanOption>>();

            EuropeanOption eu1 = new EuropeanOption(80, PayoffType.Call, 1);
            OptionMarketData<EuropeanOption> marketData1 =
            new OptionMarketData<EuropeanOption>(eu1, 25.72);
            observedOptions.AddLast(marketData1);

            EuropeanOption eu2 = new EuropeanOption(90, PayoffType.Call, 1);
            OptionMarketData<EuropeanOption> marketData2 =
            new OptionMarketData<EuropeanOption>(eu2, 18.93);
            observedOptions.AddLast(marketData2);

            EuropeanOption eu3 = new EuropeanOption(80, PayoffType.Call, 2);
            OptionMarketData<EuropeanOption> marketData3 =
            new OptionMarketData<EuropeanOption>(eu3, 30.49);
            observedOptions.AddLast(marketData3);

            EuropeanOption eu4 = new EuropeanOption(100, PayoffType.Call, 2);
            OptionMarketData<EuropeanOption> marketData4 =
            new OptionMarketData<EuropeanOption>(eu4, 19.36);
            observedOptions.AddLast(marketData4);

            EuropeanOption eu5 = new EuropeanOption(100, PayoffType.Call, 1.5);
            OptionMarketData<EuropeanOption> marketData5 =
            new OptionMarketData<EuropeanOption>(eu5, 16.58);
            observedOptions.AddLast(marketData5);

            // Heston Calibrator
            HestonCalibrator calibrator = new HestonCalibrator(hestonModel, observedOptions, calibrationSettings);
            calibrator.Calibrate();

            double error = 0;
            CalibrationOutcome outcome = CalibrationOutcome.NotStarted;
            calibrator.GetCalibrationStatus(ref outcome, ref error);
            Console.WriteLine("Calibration outcome: {0} and error: {1}", outcome, error);
        }

        // **************************
        // Task 2.7
        // **************************

        [Test]
        public void Task_2_7_1()
        {
            // Variance Process Values
            double Kappa = 2;
            double Theta = 0.06;
            double Sigma = 0.4;
            double V0 = 0.04;
            double Rho = 0.5;

            // Heston Model Params
            double InitialStockPrice = 100;
            double RiskFreeRate = 0.1;

            // Option Params
            double StrikePrice = 100;
            PayoffType Type = PayoffType.Call;
            double Maturity = 1;
            List<double> MonitoringTimes = new List<double>
            {
                0.75,
                1.00
            };

            // MC Simulation Params
            int NumberOfTrials = (int)1e5;
            int NumberOfTimeSteps = (int)Math.Ceiling(365 * Maturity);

            VarianceProcessParameters varParams =
            new VarianceProcessParameters(Kappa, Theta, Sigma, V0, Rho);

            HestonModelParameters hestonModel =
            new HestonModelParameters(InitialStockPrice, RiskFreeRate, varParams);

            AsianOption asianOption =
            new AsianOption(StrikePrice, Type, Maturity, MonitoringTimes);

            MonteCarloSettings monteCarloSettings =
            new MonteCarloSettings(NumberOfTrials, NumberOfTimeSteps);

            AsianOptionMC asianOptionMC =
            new AsianOptionMC(hestonModel, monteCarloSettings, asianOption);

            Assert.AreEqual(13.6299, asianOptionMC.Price(1), 1e-1);
        }

        [Test]
        public void Task_2_7_2()
        {
            // Variance Process Values
            double Kappa = 2;
            double Theta = 0.06;
            double Sigma = 0.4;
            double V0 = 0.04;
            double Rho = 0.5;

            // Heston Model Params
            double InitialStockPrice = 100;
            double RiskFreeRate = 0.1;

            // Option Params
            double StrikePrice = 100;
            PayoffType Type = PayoffType.Call;
            double Maturity = 2;
            List<double> MonitoringTimes = new List<double>
            {
                0.25, 0.5, 0.75, 1.00, 1.25, 1.5, 1.75
            };

            // MC Simulation Params
            int NumberOfTrials = (int)1e5;
            int NumberOfTimeSteps = (int)(365 * Maturity);

            VarianceProcessParameters varParams =
            new VarianceProcessParameters(Kappa, Theta, Sigma, V0, Rho);

            HestonModelParameters hestonModel =
            new HestonModelParameters(InitialStockPrice, RiskFreeRate, varParams);

            AsianOption asianOption =
            new AsianOption(StrikePrice, Type, Maturity, MonitoringTimes);

            MonteCarloSettings monteCarloSettings =
            new MonteCarloSettings(NumberOfTrials, NumberOfTimeSteps);

            AsianOptionMC asianOptionMC =
            new AsianOptionMC(hestonModel, monteCarloSettings, asianOption);

            Assert.AreEqual(13.6299, asianOptionMC.Price(1), 1e-1);
        }

        [Test]
        public void Task_2_7_3()
        {
            // Variance Process Values
            double Kappa = 2;
            double Theta = 0.06;
            double Sigma = 0.4;
            double V0 = 0.04;
            double Rho = 0.5;

            // Heston Model Params
            double InitialStockPrice = 100;
            double RiskFreeRate = 0.1;

            // Option Params
            double StrikePrice = 100;
            PayoffType Type = PayoffType.Call;
            double Maturity = 3;
            List<double> MonitoringTimes = new List<double>
            {
                1.00, 2.00, 3.00
            };

            // MC Simulation Params
            int NumberOfTrials = (int)1e5;
            int NumberOfTimeSteps = (int)Math.Ceiling(365 * Maturity);

            VarianceProcessParameters varParams =
            new VarianceProcessParameters(Kappa, Theta, Sigma, V0, Rho);

            HestonModelParameters hestonModel =
            new HestonModelParameters(InitialStockPrice, RiskFreeRate, varParams);

            AsianOption asianOption =
            new AsianOption(StrikePrice, Type, Maturity, MonitoringTimes);

            MonteCarloSettings monteCarloSettings =
            new MonteCarloSettings(NumberOfTrials, NumberOfTimeSteps);

            AsianOptionMC asianOptionMC =
            new AsianOptionMC(hestonModel, monteCarloSettings, asianOption);

            Assert.AreEqual(13.6299, asianOptionMC.Price(1), 1e-1);
        }

        // **************************
        // Task 2.8
        // **************************

        [Test]
        public void Task_2_8_1()
        {
            // Variance Process Values
            double Kappa = 2;
            double Theta = 0.06;
            double Sigma = 0.4;
            double V0 = 0.04;
            double Rho = 0.5;

            // Heston Model Params
            double InitialStockPrice = 100;
            double RiskFreeRate = 0.1;

            // Option Params
            double Maturity = 1;

            // MC Simulation Params
            int NumberOfTrials = (int)1e5;
            int NumberOfTimeSteps = (int)Math.Ceiling(365 * Maturity);

            VarianceProcessParameters varParams =
            new VarianceProcessParameters(Kappa, Theta, Sigma, V0, Rho);

            HestonModelParameters hestonModel =
            new HestonModelParameters(InitialStockPrice, RiskFreeRate, varParams);

            MonteCarloSettings monteCarloSettings =
            new MonteCarloSettings(NumberOfTrials, NumberOfTimeSteps);

            LookbackOptionMC lookbackOptionMC = 
            new LookbackOptionMC(hestonModel, monteCarloSettings, Maturity);

            Assert.AreEqual(13.6299, lookbackOptionMC.Price(1), 1e-1);
        }

        [Test]
        public void Task_2_8_2()
        {
            // Variance Process Values
            double Kappa = 2;
            double Theta = 0.06;
            double Sigma = 0.4;
            double V0 = 0.04;
            double Rho = 0.5;

            // Heston Model Params
            double InitialStockPrice = 100;
            double RiskFreeRate = 0.1;

            // Option Params
            double Maturity = 3;

            // MC Simulation Params
            int NumberOfTrials = (int)1e5;
            int NumberOfTimeSteps = (int)Math.Ceiling(365 * Maturity);

            VarianceProcessParameters varParams =
            new VarianceProcessParameters(Kappa, Theta, Sigma, V0, Rho);

            HestonModelParameters hestonModel =
            new HestonModelParameters(InitialStockPrice, RiskFreeRate, varParams);

            MonteCarloSettings monteCarloSettings =
            new MonteCarloSettings(NumberOfTrials, NumberOfTimeSteps);

            LookbackOptionMC lookbackOptionMC =
            new LookbackOptionMC(hestonModel, monteCarloSettings, Maturity);

            Assert.AreEqual(13.6299, lookbackOptionMC.Price(1), 1e-1);
        }

        [Test]
        public void Task_2_8_3()
        {
            // Variance Process Values
            double Kappa = 2;
            double Theta = 0.06;
            double Sigma = 0.4;
            double V0 = 0.04;
            double Rho = 0.5;

            // Heston Model Params
            double InitialStockPrice = 100;
            double RiskFreeRate = 0.1;

            // Option Params
            double Maturity = 5;

            // MC Simulation Params
            int NumberOfTrials = (int)1e5;
            int NumberOfTimeSteps = (int)Math.Ceiling(365 * Maturity);

            VarianceProcessParameters varParams =
            new VarianceProcessParameters(Kappa, Theta, Sigma, V0, Rho);

            HestonModelParameters hestonModel =
            new HestonModelParameters(InitialStockPrice, RiskFreeRate, varParams);

            MonteCarloSettings monteCarloSettings =
            new MonteCarloSettings(NumberOfTrials, NumberOfTimeSteps);

            LookbackOptionMC lookbackOptionMC =
            new LookbackOptionMC(hestonModel, monteCarloSettings, Maturity);

            Assert.AreEqual(13.6299, lookbackOptionMC.Price(1), 1e-1);
        }

        [Test]
        public void Task_2_8_4()
        {
            // Variance Process Values
            double Kappa = 2;
            double Theta = 0.06;
            double Sigma = 0.4;
            double V0 = 0.04;
            double Rho = 0.5;

            // Heston Model Params
            double InitialStockPrice = 100;
            double RiskFreeRate = 0.1;

            // Option Params
            double Maturity = 7;

            // MC Simulation Params
            int NumberOfTrials = (int)1e5;
            int NumberOfTimeSteps = (int)Math.Ceiling(365 * Maturity);

            VarianceProcessParameters varParams =
            new VarianceProcessParameters(Kappa, Theta, Sigma, V0, Rho);

            HestonModelParameters hestonModel =
            new HestonModelParameters(InitialStockPrice, RiskFreeRate, varParams);

            MonteCarloSettings monteCarloSettings =
            new MonteCarloSettings(NumberOfTrials, NumberOfTimeSteps);

            LookbackOptionMC lookbackOptionMC =
            new LookbackOptionMC(hestonModel, monteCarloSettings, Maturity);

            Assert.AreEqual(13.6299, lookbackOptionMC.Price(1), 1e-1);
        }

        [Test]
        public void Task_2_8_5()
        {
            // Variance Process Values
            double Kappa = 2;
            double Theta = 0.06;
            double Sigma = 0.4;
            double V0 = 0.04;
            double Rho = 0.5;

            // Heston Model Params
            double InitialStockPrice = 100;
            double RiskFreeRate = 0.1;

            // Option Params
            double Maturity = 9;

            // MC Simulation Params
            int NumberOfTrials = (int)1e5;
            int NumberOfTimeSteps = (int)Math.Ceiling(365 * Maturity);

            VarianceProcessParameters varParams =
            new VarianceProcessParameters(Kappa, Theta, Sigma, V0, Rho);

            HestonModelParameters hestonModel =
            new HestonModelParameters(InitialStockPrice, RiskFreeRate, varParams);

            MonteCarloSettings monteCarloSettings =
            new MonteCarloSettings(NumberOfTrials, NumberOfTimeSteps);

            LookbackOptionMC lookbackOptionMC =
            new LookbackOptionMC(hestonModel, monteCarloSettings, Maturity);

            Assert.AreEqual(13.6299, lookbackOptionMC.Price(1), 1e-1);
        }
    }
}
