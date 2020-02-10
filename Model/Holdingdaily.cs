using System;
using System.Collections.Generic;

namespace v2.Model
{
    public partial class Holdingdaily
    {
        public short Companyid { get; set; }
        public decimal? Holdingcount { get; set; }
        public DateTime Keepdata { get; set; }
        public short Stockid { get; set; }
    }
}
