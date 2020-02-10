using System;
using System.Collections.Generic;

namespace v2.Model
{
    public partial class Stockcompany
    {
        public string SNo { get; set; }
        public string SName { get; set; }
        public DateTime? Credate { get; set; }
        public short Id { get; set; }
        public DateTime? StartDate { get; set; }
        public char? Act { get; set; }
    }
}
