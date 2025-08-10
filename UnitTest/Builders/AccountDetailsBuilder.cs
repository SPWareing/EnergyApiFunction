using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnitTest.Builders.Base;
using Energy_Consumption_Function.Classes;

namespace UnitTest.Builders
{
    public class AccountDetailsBuilder : BuilderBaseClass< AccountDetails>
    {
        public AccountDetailsBuilder()
        {
            _class = new AccountDetails
            {
                number = "123456789",
                properties =
                [
                    new Property
                    {
                        id = 1,
                        moved_in_at = DateTime.Now.AddYears(-1),
                        moved_out_at = null,
                        address_line_1 = "123 Main St",
                        address_line_2 = "Apt 4B",
                        address_line_3 = "Building A",
                        town = "Springfield",
                        county = "IL",
                        postcode = "62701",
                        electricity_meter_points = new Electricity_Meter_Points[0],
                        gas_meter_points = new Gas_Meter_Points[0]
                    }
                ]
            };
        }
    }
}
