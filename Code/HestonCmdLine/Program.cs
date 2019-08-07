using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using HestonModel;
using HestonModel.Classes;
using HestonModel.Classes.InterfaceClasses;
using System.Diagnostics;
using HestonModel.Interfaces;


namespace HestonCmdLine
{
    class MainClass
    {
        public static void Main(string[] args)
        {

            // Task 2
            //Task2();

            // Task 3
            //Task3();

            // Task 4
            //Task4();

            // Task 5
            //Task5();

            // Task 6
            Task6();

            // Task 7 - Asian Arithmetic
            //Task7();

            // Task 8 - Lookback option
            //Task8();

            // Task Thread Scaling
            //TaskThreadScaling();

            Console.WriteLine("Finished!");
            Console.ReadKey();
        }

        public static void Task2()
        {
            // Task 2
            // Variance Process Values
            Console.WriteLine("Task 2");
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
            int[] Maturity = new int[5] { 1, 2, 3, 4, 15 };

            VarianceProcessParameters varParams =
            new VarianceProcessParameters(Kappa, Theta, Sigma, V0, Rho);


            HestonModelParameters hestonModel =
            new HestonModelParameters(InitialStockPrice, RiskFreeRate, varParams);
            // ************Task 2.1 Print****************
            System.Console.WriteLine("*********************");
            HestonModelParamPrint(varParams, hestonModel);

            //Prepare csv
            var csv = new StringBuilder();
            String newLine = string.Format("K, T, Price");
            csv.AppendLine(newLine);

            for (int i = 0; i < 5; i++)
            {
                EuropeanOption euOption =
                new EuropeanOption(StrikePrice, Type, Maturity[i]);
                double price = Heston.HestonEuropeanOptionPrice(hestonModel, euOption);

                System.Console.WriteLine("K={0}, T={1}, C={2}", StrikePrice,
                 Maturity[i], price);
                newLine = string.Format("{0}, {1}, {2}", StrikePrice, Maturity[i], price);
                csv.AppendLine(newLine);
            }
            //Write to csv
            File.WriteAllText(@"./task2.csv", csv.ToString());
            System.Console.WriteLine("*********************");
            System.Console.WriteLine("\n\n");
        
        }

        public static void Task3()
        {
            // Variance Process Values
            Console.WriteLine("Task 3");
            // Variance Process Values
            double Kappa = 2.0;
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
            double[] Maturity = new double[5] { 1, 2, 3, 4, 15 };

            VarianceProcessParameters varParams =
            new VarianceProcessParameters(Kappa, Theta, Sigma, V0, Rho);

            HestonModelParameters hestonModel =
            new HestonModelParameters(InitialStockPrice, RiskFreeRate, varParams);

            // ************Task 3 Print****************
            System.Console.WriteLine("*********************");
            HestonModelParamPrint(varParams, hestonModel);

            //Prepare csv
            var csv = new StringBuilder();
            String newLine = string.Format("K, T, Price, refPrice");
            csv.AppendLine(newLine);

            for (int i = 0; i < 5; i++)
            {
                // MC Simulation Params
                int NumberOfTrials = (int)1e5;
                int NumberOfTimeSteps = (int)Math.Ceiling(365 * Maturity[i]);

                EuropeanOption euOption =
                new EuropeanOption(StrikePrice, Type, Maturity[i]);

                MonteCarloSettings monteCarloSettings =
                new MonteCarloSettings(NumberOfTrials, NumberOfTimeSteps);
                double priceForm = Heston.HestonEuropeanOptionPrice(hestonModel, euOption);

                double price = Heston.HestonEuropeanOptionPriceMC(hestonModel, euOption, monteCarloSettings);
                System.Console.WriteLine("K={0}, T={1}, C_MC={2}, C_form={3}", StrikePrice,
                 Maturity[i], price, priceForm);

                newLine = string.Format("{0}, {1}, {2}, {3}", StrikePrice, Maturity[i], price, priceForm);
                csv.AppendLine(newLine);
            }

            //Write to csv
            File.WriteAllText(@"./task3.csv", csv.ToString());

            System.Console.WriteLine("*********************");
            System.Console.WriteLine("\n\n");
        }


        public static void Task4()
        {
            // Variance Process Values
            Console.WriteLine("Task 4");
            // Variance Process Values
            double Kappa = 2.0;
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

            VarianceProcessParameters varParams =
            new VarianceProcessParameters(Kappa, Theta, Sigma, V0, Rho);

            HestonModelParameters hestonModel =
            new HestonModelParameters(InitialStockPrice, RiskFreeRate, varParams);

            // ************Task 4 Print****************
            System.Console.WriteLine("*********************");
            HestonModelParamPrint(varParams, hestonModel);

            EuropeanOption euOption =
                new EuropeanOption(StrikePrice, Type, Maturity);

            double priceForm = Heston.HestonEuropeanOptionPrice(hestonModel, euOption);
            System.Console.WriteLine("K={0}, T={1}, refPrice={2}", StrikePrice,
                 Maturity, priceForm);

            //Prepare csv
            var csv = new StringBuilder();
            String newLine = string.Format("Trials, Time Steps, relError");
            csv.AppendLine(newLine);

            for (int i = 3; i < 5; i++)
            {
                // MC Simulation Params
                int NumberOfTrials = (int) Math.Pow(10, i);
                double[] factor = new double[3] { 0.5, 1, 2 };
                for (int j = 0; j < 3; j++)
                {
                    int NumberOfTimeSteps = (int)Math.Ceiling(factor[j] * 365 * Maturity);
                    MonteCarloSettings monteCarloSettings =
                    new MonteCarloSettings(NumberOfTrials, NumberOfTimeSteps);

                    double price = Heston.HestonEuropeanOptionPriceMC(hestonModel, euOption, monteCarloSettings);
                    System.Console.WriteLine("K={0}, T={1}, #Trials={2}, #TimeSteps={3}, rel_error={4}", StrikePrice,
                     Maturity, NumberOfTrials, NumberOfTimeSteps, Math.Abs(priceForm - price) / priceForm);
                    newLine = string.Format("{0}, {1}, {2}", NumberOfTrials, NumberOfTimeSteps, Math.Abs(priceForm - price) / priceForm);
                    csv.AppendLine(newLine);
                }

            }

            //Write to csv
            File.WriteAllText(@"./task4.csv", csv.ToString());

            System.Console.WriteLine("*********************");
            System.Console.WriteLine("\n\n");
        }


        public static void Task5()
        {
            // Variance Process Values
            double Kappa = 1.5768;
            double Theta = 0.398;
            double Sigma = 0.5751;
            double V0 = 1.0175;
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
            LinkedList<IOptionMarketData<IEuropeanOption>> observedOptions =
                new LinkedList<IOptionMarketData<IEuropeanOption>>();

            EuropeanOption eu1 = new EuropeanOption(80, PayoffType.Call, 1);
            OptionMarketData<IEuropeanOption> marketData1 =
            new OptionMarketData<IEuropeanOption>(eu1, 25.72);
            observedOptions.AddLast(marketData1);

            EuropeanOption eu2 = new EuropeanOption(90, PayoffType.Call, 1);
            OptionMarketData<IEuropeanOption> marketData2 =
            new OptionMarketData<IEuropeanOption>(eu2, 18.93);
            observedOptions.AddLast(marketData2);

            EuropeanOption eu3 = new EuropeanOption(80, PayoffType.Call, 2);
            OptionMarketData<IEuropeanOption> marketData3 =
            new OptionMarketData<IEuropeanOption>(eu3, 30.49);
            observedOptions.AddLast(marketData3);

            EuropeanOption eu4 = new EuropeanOption(100, PayoffType.Call, 2);
            OptionMarketData<IEuropeanOption> marketData4 =
            new OptionMarketData<IEuropeanOption>(eu4, 19.36);
            observedOptions.AddLast(marketData4);

            EuropeanOption eu5 = new EuropeanOption(100, PayoffType.Call, 1.5);
            OptionMarketData<IEuropeanOption> marketData5 =
            new OptionMarketData<IEuropeanOption>(eu5, 16.58);
            observedOptions.AddLast(marketData5);

            HestonCalibrationResult result;

            result = (HestonCalibrationResult)
            Heston.CalibrateHestonParameters(hestonModel, observedOptions, calibrationSettings);

            Console.WriteLine("Calibration outcome: {0} and error: {1}", result.MinimizerStatus, result.PricingError);

        }

        public static void Task6()
        {
            Console.WriteLine("********Task 6*********");
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
            double accuracy = 1.0e-3;
            int maxIter = 1000;

            VarianceProcessParameters varParams =
            new VarianceProcessParameters(Kappa, Theta, Sigma, V0, Rho);

            HestonModelParameters hestonModel =
            new HestonModelParameters(InitialStockPrice, RiskFreeRate, varParams);

            HestonModelParameters guess =
            new HestonModelParameters(InitialStockPrice, RiskFreeRate,
            new VarianceProcessParameters(1.55, 0.88, 1.5999, 0.4, -0.55));

            CalibrationSettings calibrationSettings = new CalibrationSettings(accuracy, maxIter);

            // Market Data
            LinkedList<IOptionMarketData<IEuropeanOption>> observedOptions =
                new LinkedList<IOptionMarketData<IEuropeanOption>>();

            EuropeanOption eu1 = new EuropeanOption(80, PayoffType.Call, 1);
            EuropeanOptionFormula eu1Form = new EuropeanOptionFormula(hestonModel, eu1);
            OptionMarketData<IEuropeanOption> marketData1 =
            new OptionMarketData<IEuropeanOption>(eu1, eu1Form.Price());
            observedOptions.AddLast(marketData1);

            EuropeanOption eu2 = new EuropeanOption(90, PayoffType.Call, 1);
            EuropeanOptionFormula eu2Form = new EuropeanOptionFormula(hestonModel, eu2);
            OptionMarketData<IEuropeanOption> marketData2 =
            new OptionMarketData<IEuropeanOption>(eu2, eu2Form.Price());
            observedOptions.AddLast(marketData2);

            EuropeanOption eu3 = new EuropeanOption(80, PayoffType.Call, 2);
            EuropeanOptionFormula eu3Form = new EuropeanOptionFormula(hestonModel, eu3);
            OptionMarketData<IEuropeanOption> marketData3 =
            new OptionMarketData<IEuropeanOption>(eu3, eu3Form.Price());
            observedOptions.AddLast(marketData3);

            EuropeanOption eu4 = new EuropeanOption(100, PayoffType.Call, 2);
            EuropeanOptionFormula eu4Form = new EuropeanOptionFormula(hestonModel, eu4);
            OptionMarketData<IEuropeanOption> marketData4 =
            new OptionMarketData<IEuropeanOption>(eu4, eu4Form.Price());
            observedOptions.AddLast(marketData4);

            EuropeanOption eu5 = new EuropeanOption(100, PayoffType.Call, 1.5);
            EuropeanOptionFormula eu5Form = new EuropeanOptionFormula(hestonModel, eu5);
            OptionMarketData<IEuropeanOption> marketData5 =
            new OptionMarketData<IEuropeanOption>(eu5, eu5Form.Price());
            observedOptions.AddLast(marketData5);

            HestonCalibrationResult result;

            result = (HestonCalibrationResult)
            Heston.CalibrateHestonParameters(guess, observedOptions, calibrationSettings);

            Console.WriteLine("Calibration outcome: {0} and error: {1}", result.MinimizerStatus, result.PricingError);

        }

        public static void Task7()
        {
            // Variance Process Values
            Console.WriteLine("Task 7");
            // Variance Process Values
            double Kappa = 2.0;
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
            double[] Maturity = new double[3] { 1, 2, 3 };

            VarianceProcessParameters varParams =
            new VarianceProcessParameters(Kappa, Theta, Sigma, V0, Rho);

            HestonModelParameters hestonModel =
            new HestonModelParameters(InitialStockPrice, RiskFreeRate, varParams);

            // ************Task 7 Print****************
            System.Console.WriteLine("*********************");
            HestonModelParamPrint(varParams, hestonModel);

            // Monitoring Times
            List<double> monTimes1 = new List<double>
            {
                0.75,
                1.00
            };
            List<double> monTimes2 = new List<double>
            {
                0.25, 0.5, 0.75, 1.00, 1.25, 1.5, 1.75
            };
            List<double> monTimes3 = new List<double>
            {
                1.00, 2.00, 3.00
            };
            List<List<double>> MonitoringTimes = new List<List<double>>
            {
                monTimes1, monTimes2, monTimes3
            };

            //Prepare csv
            var csv = new StringBuilder();
            String newLine = string.Format("K, T, Price");
            csv.AppendLine(newLine);

            for (int i = 0; i < 3; i++)
            {
                // MC Simulation Params
                int NumberOfTrials = (int)1e5;
                int NumberOfTimeSteps = (int)Math.Ceiling(365 * Maturity[i]);

                AsianOption asianOption =
                new AsianOption(StrikePrice, Type, Maturity[i], MonitoringTimes[i]);

                MonteCarloSettings monteCarloSettings =
                new MonteCarloSettings(NumberOfTrials, NumberOfTimeSteps);

                double price = Heston.HestonAsianOptionPriceMC(hestonModel,
                asianOption, monteCarloSettings);

                System.Console.WriteLine("K={0}, T={1}, C_MC={2}", StrikePrice,
                 Maturity[i], price);

                newLine = string.Format("{0}, {1}, {2}", StrikePrice, Maturity[i], price);
                csv.AppendLine(newLine);
            }

            //Write to csv
            File.WriteAllText(@"./task7.csv", csv.ToString());

            System.Console.WriteLine("*********************");
            System.Console.WriteLine("\n\n");
        }

        public static void Task8()
        {
            // Variance Process Values
            Console.WriteLine("Task 8");
            // Variance Process Values
            double Kappa = 2.0;
            double Theta = 0.06;
            double Sigma = 0.4;
            double V0 = 0.04;
            double Rho = 0.5;

            // Heston Model Params
            double InitialStockPrice = 100;
            double RiskFreeRate = 0.1;

            // Option Params
            double[] Maturity = new double[5] { 1, 3, 5, 7, 9 };

            VarianceProcessParameters varParams =
            new VarianceProcessParameters(Kappa, Theta, Sigma, V0, Rho);

            HestonModelParameters hestonModel =
            new HestonModelParameters(InitialStockPrice, RiskFreeRate, varParams);

            // ************Task 7 Print****************
            System.Console.WriteLine("*********************");
            HestonModelParamPrint(varParams, hestonModel);

            //Prepare csv
            var csv = new StringBuilder();
            String newLine = string.Format("T, Price");
            csv.AppendLine(newLine);

            for (int i = 0; i < 5; i++)
            {
                // MC Simulation Params
                int NumberOfTrials = (int)1e5;
                int NumberOfTimeSteps = (int)Math.Ceiling(365 * Maturity[i]);

                MonteCarloSettings monteCarloSettings =
                new MonteCarloSettings(NumberOfTrials, NumberOfTimeSteps);

                Option maturity = new Option(Maturity[i]);
                double price = Heston.HestonLookbackOptionPriceMC(hestonModel, maturity, monteCarloSettings);

                System.Console.WriteLine("T={0}, C_MC={1}",
                 Maturity[i], price);

                newLine = string.Format("{0}, {1}", Maturity[i], price);
                csv.AppendLine(newLine);
            }

            //Write to csv
            File.WriteAllText(@"./task8.csv", csv.ToString());

            System.Console.WriteLine("*********************");
            System.Console.WriteLine("\n\n");
        }

        public static void TaskThreadScaling()
        {
            // Variance Process Values
            Console.WriteLine("Task Thread Scaling");
            // Variance Process Values
            double Kappa = 2.0;
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

            VarianceProcessParameters varParams =
            new VarianceProcessParameters(Kappa, Theta, Sigma, V0, Rho);

            HestonModelParameters hestonModel =
            new HestonModelParameters(InitialStockPrice, RiskFreeRate, varParams);

            // ************Task 3 Print****************
            System.Console.WriteLine("*********************");
            HestonModelParamPrint(varParams, hestonModel);

            //Prepare csv
            var csv = new StringBuilder();
            String newLine = string.Format("Number of Trials, cores, time");
            csv.AppendLine(newLine);
            int[] cores = new int[5] { 1, 2, 4, 8, 16 };
            for (int i = 0; i < 5; i++)
            {
                // MC Simulation Params
                int NumberOfTrials = (int)1e6;
                int NumberOfTimeSteps = (int)Math.Ceiling(365 * Maturity);

                EuropeanOption euOption =
                new EuropeanOption(StrikePrice, Type, Maturity);

                MonteCarloSettings monteCarloSettings =
                new MonteCarloSettings(NumberOfTrials, NumberOfTimeSteps);
                double priceForm = Heston.HestonEuropeanOptionPrice(hestonModel, euOption);

                // Create Monte Carlo EU option object
                EuropeanOptionMC euOptionMC = new EuropeanOptionMC(hestonModel, monteCarloSettings, euOption);

                var stopwatch = new Stopwatch();
                stopwatch.Start();
                double price = euOptionMC.Price(cores[i]);
                stopwatch.Stop();
                long elapsed_time = stopwatch.ElapsedMilliseconds;

                System.Console.WriteLine("K={0}, T={1}, cores={2}, C_MC={3}, C_form={4}, time={5}",
                StrikePrice, Maturity, cores[i], price, priceForm, elapsed_time);

                newLine = string.Format("{0}, {1}, {2}", NumberOfTrials, cores[i], elapsed_time);
                csv.AppendLine(newLine);
            }

            //Write to csv
            File.WriteAllText(@"./taskThreadScaling.csv", csv.ToString());

            System.Console.WriteLine("*********************");
            System.Console.WriteLine("\n\n");
        }

        public static void HestonModelParamPrint(VarianceProcessParameters parameters,
                                                 HestonModelParameters model)
        {
            System.Console.WriteLine("k={0}, Theta={1}, Sigma={2}, V0={3}, Rho={4}, r={5}",
            parameters.Kappa, parameters.Theta, parameters.Sigma, parameters.V0,
            parameters.Rho, model.RiskFreeRate);
        }

        public static void HestonModelParamPrint(VarianceProcessParameters parameters,
                                                 HestonModelParameters model,
                                                 MonteCarloSettings monteCarloSettings)
        {
            System.Console.WriteLine("k={0}, Theta={1}, Sigma={2}, V0={3}, Rho={4}, r={5}, #Trials={6}, #Steps={7}",
            parameters.Kappa, parameters.Theta, parameters.Sigma, parameters.V0,
            parameters.Rho, model.RiskFreeRate, monteCarloSettings.NumberOfTrials,
            monteCarloSettings.NumberOfTimeSteps);
        }
    }
}
