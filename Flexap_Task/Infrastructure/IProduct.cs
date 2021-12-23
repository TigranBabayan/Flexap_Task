using Flexap_Task.Models;
using System.Collections.Generic;

namespace Flexap_Task.Infrastructure
{
    public interface IProduct
    {
        IEnumerable<Product> GetAll();
        Product GetById(int? id);
        void Insert(Product product);

        void Update(Product product);
        void Delete(Product product);
    }
}
