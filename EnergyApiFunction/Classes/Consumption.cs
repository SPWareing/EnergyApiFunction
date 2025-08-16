using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices.Marshalling;
using System.Text;
using System.Threading.Tasks;
using Energy_Consumption_Function.Interfaces;

namespace Energy_Consumption_Function.Classes
{
    /// <summary>
    /// Represents gas consumption data.
    /// </summary>
    public class Consumption: IPaging
    {
        /// <summary>
        /// Gets or sets the count of gas consumption records.
        /// </summary>
        public int count { get; set; }

        /// <summary>
        /// Gets or sets the URL for the next set of gas consumption records.
        /// </summary>
        public string next { get; set; }

        /// <summary>
        /// Gets or sets the URL for the previous set of gas consumption records.
        /// </summary>
        public object previous { get; set; }

        /// <summary>
        /// Gets or sets the array of gas consumption results.
        /// </summary>
        public Result[] results { get; set; }
    }
}
