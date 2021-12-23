using System;
namespace Flexap_Task.Models
{
    public class Cart
    {
        public int Id { get; set; }
        public Product CartProduct { get; set; }

        public int Quantity { get; set; }


    }
}
