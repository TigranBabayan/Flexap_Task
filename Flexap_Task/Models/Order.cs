using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Flexap_Task.Models
{
    public class Order
    {
        public int Id { get; set; }
        public int ProductId { get; set; }
        public Product Product { get; set; }
        public int PLU { get; set; }
        public string Barcode { get; set; }
        public int Quantity { get; set; }
        public DateTime? Datatime { get; set; }
    }
}
