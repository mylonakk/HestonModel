using System;
using HestonModel.Interfaces;

namespace HestonModel.Classes.InterfaceClasses
{
    /// <summary>
    /// Generic Class representing data set of options and treir price in the market.
    /// </summary>
    public class OptionMarketData<T> : IOptionMarketData<T> where T : IEuropeanOption
    {
        private T _Option;
        private double _Price;

        public double Price
        {
            get { return _Price; }
        }
        public T Option
        {
            get { return _Option; }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="T:HestonModel.Classes.InterfaceClasses.OptionMarketData`1"/> class.
        /// </summary>
        /// <param name="option">Option.</param>
        /// <param name="price">Price.</param>
        public OptionMarketData(T option, double price)
        {
            _Option = option;
            _Price = price;
        }
    }
}
