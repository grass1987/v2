using System;
using System.Data;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Runtime.Serialization;

namespace v2.Data
{
    public class oneStock :ICloneable
    {


        public List<oneCompanyAllDayHolding> allCompanyHoldingCount  { get; set; }
        public String stockID { get; set; }
        public String stockName { get; set; }
        public int whichpage { get; set; }
        //public Decimal holdingcount { get; set; }
        //public DateTime keepdata { get; set; }
        public int periodWeek { get; set; }
        public int endOrNot {get;set;}
        //
        //public string govcompanyid { get; set; }
        
        public object Clone()
        {
            return this.MemberwiseClone();
        }
    }
}
