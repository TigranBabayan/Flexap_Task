using Flexap_Task.Data;
using Flexap_Task.Helpers;
using Flexap_Task.Infrastructure;
using Flexap_Task.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;

namespace Flexap_Task.Controllers
{
    [Route("cart")]
    public class CartController : Controller
    {
        private readonly IProduct _product;
        private readonly ApplicationContext _context;

        public CartController(IProduct product,ApplicationContext context)
        {
            _product = product;
            _context = context;
        }

        [Route("index")]
        public IActionResult Index(int quantity)
        {
            var cart = SessionHelper.GetObjectFromJson<List<Cart>>(HttpContext.Session, "cart");
            if (cart != null)
                ViewBag.total = cart.Sum(item => item.CartProduct.Price * item.Quantity);
            return View(cart);
        }

        [Route("add/{id}")]
        public IActionResult Add(int id)
        {
            Product comp = new Product();
            if (SessionHelper.GetObjectFromJson<List<Cart>>(HttpContext.Session, "cart") == null)
            {
                List<Cart> cart = new List<Cart>();
                cart.Add(new Cart { CartProduct = _context.Products.Find(id), Quantity = 1 });
                SessionHelper.SetObjectAsJson<List<Cart>>(HttpContext.Session, "cart", cart);
            }
            else
            {
                List<Cart> cart = SessionHelper.GetObjectFromJson<List<Cart>>(HttpContext.Session, "cart");
                int index = isExist(id);
                if (index != -1)
                {
                    cart[index].Quantity++;
                }
                else
                {
                    cart.Add(new Cart { CartProduct = _context.Products.Find(id), Quantity = 1 });
                }
                SessionHelper.SetObjectAsJson<List<Cart>>(HttpContext.Session, "cart", cart);
            }
            return RedirectToAction("Index");
        }

        [Route("remove/{id}")]
        public IActionResult Remove(int id)
        {
            List<Cart> cart = SessionHelper.GetObjectFromJson<List<Cart>>(HttpContext.Session, "cart");
            int index = isExist(id);
            cart.RemoveAt(index);
            SessionHelper.SetObjectAsJson<List<Cart>>(HttpContext.Session, "cart", cart);
            return RedirectToAction("Index");
        }

        private int isExist(int id)
        {
            List<Cart> cart = SessionHelper.GetObjectFromJson<List<Cart>>(HttpContext.Session, "cart");
            for (int i = 0; i < cart.Count; i++)
            {
                if (cart[i].CartProduct.Id.Equals(id))
                {
                    return i;
                }
            }
            return -1;
        }

        public IActionResult Update(int[] quantity)
        {
            List<Cart> cart = SessionHelper.GetObjectFromJson<List<Cart>>(HttpContext.Session, "cart");
            for (int i = 0; i < cart.Count; i++)
            {
                cart[i].Quantity = quantity[i];
                SessionHelper.SetObjectAsJson<List<Cart>>(HttpContext.Session, "cart", cart);
            }
            if (cart != null)
            {
                ViewBag.total = cart.Sum(item => item.CartProduct.Price * item.Quantity);
            }
            return View("Index", cart);
        }

        
    }
}
