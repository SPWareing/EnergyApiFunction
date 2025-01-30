using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Energy_Consumption_Function.Classes
{
    public class Register
    {
        public string identifier { get; set; }
        public string rate { get; set; }
        public bool is_settlement_register { get; set; }
    }
}
