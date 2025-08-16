using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Energy_Consumption_Function.Interfaces
{
  public interface IResult
    {
        /// <summary>
        /// Gets or sets the consumption value.
        /// </summary>
        float consumption { get; set; }
        /// <summary>
        /// Gets or sets the start time of the consumption interval.
        /// </summary>
        DateTime interval_start { get; set; }
        /// <summary>
        /// Gets or sets the end time of the consumption interval.
        /// </summary>
        DateTime interval_end { get; set; }
    }
}
