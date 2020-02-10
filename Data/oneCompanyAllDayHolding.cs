using System;
using System.Data;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Runtime.Serialization;

namespace v2.Data
{
    public class oneCompanyAllDayHolding: IComparable<oneCompanyAllDayHolding>
    {
        public List<oneDayHoldingCount> allDayHoldingCount  { get; set; }
        public Decimal allDayDiff { get; set; }
        //public DateTime keepdata { get; set; }
        public string companyid { get; set; }
        //
        public string companyname { get; set; }
        public int CompareTo(oneCompanyAllDayHolding other)
        {
            // Alphabetic sort if salary is equal. [A to Z]
            if (this.allDayDiff == other.allDayDiff)
            {
                return this.companyid.CompareTo(other.companyid);
            }
            // Default to salary sort. [High to low]
            return other.allDayDiff.CompareTo(this.allDayDiff);
        }

    }
}
