using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Energy_Consumption_Function.Classes
{
    /// <summary>
    /// Represents energy data with a date range and energy type.
    /// </summary>
    public class Energy
    {
        /// <summary>
        /// Gets or sets the start date of the energy data.
        /// </summary>
        public DateTime from { get; set; }

        /// <summary>
        /// Gets or sets the end date of the energy data.
        /// </summary>
        public DateTime to { get; set; }

        /// <summary>
        /// Gets or sets the type of energy.
        /// </summary>
        public string energyType { get; set; }
    }
}
