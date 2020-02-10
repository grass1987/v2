using System;
using System.Data;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Runtime.Serialization;

namespace v2.Data
{
    public class oneDayHoldingCount
    {


        public DateTime holdingDate  { get; set; }
        public Decimal holdingCount { get; set; }
        public Decimal holdingDiff { get; set; }
        //public DateTime keepdata { get; set; }
        //public string stockid { get; set; }
        //
        //public string govcompanyid { get; set; }
        

    }
}
