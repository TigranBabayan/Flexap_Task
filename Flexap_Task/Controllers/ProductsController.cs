using Flexap_Task.Data;
using Flexap_Task.Infrastructure;
using Flexap_Task.Models;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using X.PagedList;
using Flexap_Task.Helpers;

namespace Flexap_Task.Controllers
{
    public class ProductsController : Controller
    {
        private readonly ApplicationContext _context;
        private readonly IProduct _product;
        private readonly IWebHostEnvironment webHostEnvironment;
      

        public ProductsController(IProduct product, IWebHostEnvironment hostEnvironment, ApplicationContext context)
        {
            _product = product;
            _context = context;
            webHostEnvironment = hostEnvironment;
        }

        // GET: Products
        public async Task<IActionResult> Index(int? page)
        {
            if (string.IsNullOrWhiteSpace(HttpContext.Session.GetString("Email")))
            {
                return RedirectToAction("Login", "Admin");
            }
            var products = _product.GetAll();
            var pageNumber = page ?? 1;
            var pageSize = 25;
            var onePageOfProducts = products.OrderByDescending(i => i.Id).ToPagedList(pageNumber, pageSize);
            return View(onePageOfProducts);
        }

        // GET: Products/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (string.IsNullOrWhiteSpace(HttpContext.Session.GetString("Email")))
            {
                return RedirectToAction("Login", "Admin");
            }
            if (id == null)
            {
                return NotFound();
            }

            var product = await _context.Products.Include(i=>i.ProductGaleries)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (product == null)
            {
                return NotFound();
            }

            return View(product);
        }

        // GET: Products/Create
        public IActionResult Create()
        {
            if (string.IsNullOrWhiteSpace(HttpContext.Session.GetString("Email")))
            {
                return RedirectToAction("Login", "Admin");
            }
            return View();
        }

        // POST: Products/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Product product)
        {
            if (ModelState.IsValid)
            {
                string uniqImageName = null;
                var productGaleries = new List<ProductGalery>();
                List<string> uniqueFileName = UploadImage(product);

                foreach (var image in uniqueFileName)
                {
                    uniqImageName = image;

                    ProductGalery productGalery = new ProductGalery
                    {
                        ImageUrl = uniqImageName,
                        Product = product,
                    };
                    productGaleries.Add(productGalery);
                }
                _context.Galeries.AddRange(productGaleries);
                _product.Insert(product);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(product);
        }

        // GET: Products/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (string.IsNullOrWhiteSpace(HttpContext.Session.GetString("Email")))
            {
                return RedirectToAction("Login", "Admin");
            }
            if (id == null)
            {
                return NotFound();
            }

            var product = _product.GetById(id);
            if (product == null)
            {
                return NotFound();
            }
            return View(product);
        }

        // POST: Products/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id,  Product product)
        {

            if (id != product.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _product.Update(product);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ProductExists(product.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(product);
        }

        // GET: Products/Delete/5
        public IActionResult Delete(int? id)
        {
            if (string.IsNullOrWhiteSpace(HttpContext.Session.GetString("Email")))
            {
                return RedirectToAction("Login", "Admin");
            }
            if (id == null)
            {
                return NotFound();
            }
            var product = _product.GetById(id);
            if (product == null)
            {
                return NotFound();
            }

            return View(product);
        }

        // POST: Products/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public IActionResult DeleteConfirmed(int id)
        {
            var product =_product.GetById(id);
            _product.Delete(product);
            _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ProductExists(int id)
        {
            return _context.Products.Any(e => e.Id == id);
        }

        private List<string> UploadImage(Product product)
        {
            List<string> uniqueFileName = new List<string>();
            string filePath = "images";

            if (product.ProductImages != null)
            {
                string uploadsFolder = Path.Combine(webHostEnvironment.WebRootPath, filePath);
                foreach (IFormFile item in product.ProductImages)
                {

                    var FileName = Guid.NewGuid().ToString() + "_" + item.FileName;
                    uniqueFileName.Add(FileName);
                    filePath = Path.Combine(uploadsFolder, FileName);
                    using (var fileStream = new FileStream(filePath, FileMode.Create))
                    {
                        item.CopyTo(fileStream);
                    }
                }
            }
            return uniqueFileName;
        }


        public async Task<IActionResult> Generated()
        {
            for (int i = 0; i < 50000; i++)
            {
                Random random = new Random();
                int price = random.Next(10,5000);
                Product product = new Product
                {

                    ProductName = Guid.NewGuid().ToString().Substring(0,8)+i,
                    Price =price.RoundNumber(),
                    Barcode = Guid.NewGuid().ToString().Substring(0,8)+Guid.NewGuid().ToString().Substring(1,5),
                    PLU = random.Next(1,99999),
                    Generated = true
                };
                _product.Insert(product);
                _context.SaveChanges();
            }
            return RedirectToAction("Index","Products");
        }

        public  async Task<IActionResult> DeleteGenerated()
        {
            var generated = _context.Products.Where(i => i.Generated == true).ToList();
            _context.Products.RemoveRange(generated);
            _context.SaveChanges();
           return RedirectToAction("Index", "Products");
        }

        public IActionResult SearchByPluOrBarode(string source,int? page)
        {
            var products = _context.Products.Include(i => i.ProductGaleries).Where(i => i.Barcode == source || i.PLU.ToString() == source).ToList();
            
            var pageNumber = page ?? 1;
            var pageSize = 25;
            var onePageOfProducts = products.OrderByDescending(i => i.Id).ToPagedList(pageNumber, pageSize);

          
            return View("Index", onePageOfProducts);
        }


    }
}
