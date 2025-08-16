using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Energy_Consumption_Function.Interfaces
{
    public interface IPaging
    {
        public int count { get; set; }
        public object previous { get; set; }
    }
}
