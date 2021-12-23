using Flexap_Task.Infrastructure;
using Flexap_Task.Data;
using Flexap_Task.Models;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;

namespace Flexap_Task.Repository
{
    public class ProductRepo:IProduct
    {
        private readonly ApplicationContext _context;
        public ProductRepo(ApplicationContext context)
        {
            _context = context;
        }
        public void Insert(Product product)
        {
            _context.Products.Add(product);
            _context.SaveChanges();
        }

        public void Delete(Product product)
        {
            _context.Products.Remove(product);
            _context.SaveChanges();
        }
        public IEnumerable<Product> GetAll()
        {
            return _context.Products.Include(img => img.ProductGaleries).ToList();
        }
        public Product GetById(int? id)
        {
            return _context.Products.Include(img => img.ProductGaleries).Where(i => i.Id == id).FirstOrDefault();
        }
        public void Update(Product product)
        {
            _context.Products.Update(product);
            _context.SaveChanges();
        }
    }
}
