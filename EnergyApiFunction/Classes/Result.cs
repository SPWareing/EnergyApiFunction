using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Energy_Consumption_Function.Classes
{
    /// <summary>
    /// Represents a result of energy or gas consumption.
    /// </summary>
    public class Result
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
