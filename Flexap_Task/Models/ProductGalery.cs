namespace Flexap_Task.Models
{
    public class ProductGalery
    {
        public int Id { get; set; }

        public int ProductId { get; set; }

        public Product Product { get; set; }

        public string ImageUrl { get; set; }
    }
}
