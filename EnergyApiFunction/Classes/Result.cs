using System;
using Energy_Consumption_Function.Interfaces;

namespace Energy_Consumption_Function.Classes
{
    /// <summary>
    /// Represents a result of energy or gas consumption.
    /// </summary>
    public class Result: IResult
    {
        /// <summary>
        /// Gets or sets the consumption value.
        /// </summary>
        public float consumption { get; set; }

        /// <summary>
        /// Gets or sets the start time of the consumption interval.
        /// </summary>
        public DateTime interval_start { get; set; }

        /// <summary>
        /// Gets or sets the end time of the consumption interval.
        /// </summary>
        public DateTime interval_end { get; set; }
    }
}
