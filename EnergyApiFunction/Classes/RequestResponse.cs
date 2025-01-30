using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Energy_Consumption_Function.Classes
{
    public class RequestResponse
    {
       public  List<MergedResponse> Electricity { get; set; }
       public  List<MergedResponse> Gas { get; set; }
    }
}
