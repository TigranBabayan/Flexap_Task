using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Flexap_Task.ViewModel
{
    public class OrdersFilter
    {
        public int PLU { get; set; }

        public string Barcode { get; set; }

        public int Quantity { get; set; }

        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
    }
}
