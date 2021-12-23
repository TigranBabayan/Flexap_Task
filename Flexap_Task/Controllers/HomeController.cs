using Flexap_Task.Data;
using Flexap_Task.Helpers;
using Flexap_Task.Infrastructure;
using Flexap_Task.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using X.PagedList;

namespace Flexap_Task.Controllers
{
    public class HomeController : Controller
    {
        private readonly IProduct _product;
        private readonly ApplicationContext _context;
        public HomeController(IProduct product, ApplicationContext context)
        {
            _product= product;
            _context = context;
        }
        public async Task<IActionResult> Index(int? page)
        {
            
            var products = _product.GetAll();
            var pageNumber = page ?? 1;
            var pageSize = 25;
            var onePageOfProducts = products.OrderByDescending(i => i.Id).ToPagedList(pageNumber, pageSize);
            return View(onePageOfProducts);
        }

        public IActionResult Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            var product =  _context.Products.Include(i => i.ProductGaleries)
                .FirstOrDefault(m => m.Id == id);
            if (product == null)
            {
                return NotFound();
            }

            return View(product);
        }

        public IActionResult SearchByPluOrBarode(string source, int? page)
        {
            var products = _context.Products.Include(i => i.ProductGaleries).Where(i => i.Barcode == source || i.PLU.ToString() == source).ToList();

            var pageNumber = page ?? 1;
            var pageSize = 25;
            var onePageOfProducts = products.OrderByDescending(i => i.Id).ToPagedList(pageNumber, pageSize);

            return View("Index", onePageOfProducts);
        }

        public IActionResult Purchase()
        {
            List<Cart> cart = SessionHelper.GetObjectFromJson<List<Cart>>(HttpContext.Session, "cart");
            ViewBag.total = cart.Sum(item => item.CartProduct.Price * item.Quantity);

            return View("Purchase", cart);
        }

        public IActionResult Buy()
        {
            List<Cart> cart = SessionHelper.GetObjectFromJson<List<Cart>>(HttpContext.Session, "cart");
            for (int i = 0; i < cart.Count(); i++)
            {
                Order Orders = new Order
                {
                    ProductId = cart[i].CartProduct.Id,
                    Datatime = DateTime.Now,
                    Quantity = cart[i].Quantity,
                    Barcode = cart[i].CartProduct.Barcode,
                    PLU = cart[i].CartProduct.PLU,
                };
                _context.Orders.Add(Orders);
                HttpContext.Session.SetString("Order",Orders.Barcode);

            }
            _context.SaveChanges();

            return Ok("Success");
        }
    }
}
