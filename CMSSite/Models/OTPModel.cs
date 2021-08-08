using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CRMSite.Models
{
    public class OTPModel
    {
        public DateTime ExpiredTime { get; set; }

        public string OTP { get; set; }
    }
}
