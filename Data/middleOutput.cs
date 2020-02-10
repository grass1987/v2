using System;
using System.Data;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Runtime.Serialization;

namespace v2.Data
{
    public class middleOutput
    {


        public short companyid { get; set; }
        public Decimal holdingcount { get; set; }
        public DateTime keepdata { get; set; }
        public string stockid { get; set; }
        public string companyname { get; set; }

        public string govcompanyid { get; set; }
        

    }
}
