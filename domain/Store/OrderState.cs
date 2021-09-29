using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Store
{
    public enum OrderState
    {
        Created = 1,

        ProcessStarted,

        CellPhoneConfirmed,
    }
}
