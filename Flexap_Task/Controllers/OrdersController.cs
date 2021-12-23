using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Flexap_Task.Data;
using Flexap_Task.Models;
using Flexap_Task.ViewModel;
using LinqKit;
using X.PagedList;

namespace Flexap_Task.Controllers
{
    public class OrdersController : Controller
    {
        private readonly ApplicationContext _context;

        public OrdersController(ApplicationContext context)
        {
            _context = context;
        }

        // GET: Orders
        public IActionResult Index(int? page)
        {
            var orders = _context.Orders.ToList();
            var pageNumber = page ?? 1;
            var pageSize = 25;
            var onePageOfProducts = orders.OrderByDescending(i => i.Id).ToPagedList(pageNumber, pageSize);
            return View(onePageOfProducts);
        }


        // GET: Orders/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var order = await _context.Orders
                .Include(o => o.Product)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (order == null)
            {
                return NotFound();
            }

            return View(order);
        }

        // POST: Orders/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var order = await _context.Orders.FindAsync(id);
            _context.Orders.Remove(order);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool OrderExists(int id)
        {
            return _context.Orders.Any(e => e.Id == id);
        }

        public IActionResult Filter(OrdersFilter orders,int? page)
        {
            var order = new List<Order>();
            var predicate = PredicateBuilder.True<Order>();
            if (orders.EndDate == null)
            {
                orders.EndDate = DateTime.Now;
            }
            if (orders.PLU>0)
                predicate = predicate.And(i => i.Product.PLU == orders.PLU);
            if (!string.IsNullOrEmpty(orders.Barcode))
                predicate = predicate.And(i => i.Product.Barcode == orders.Barcode);
            if (orders.StartDate != null && orders.EndDate != null)
            {
                predicate = predicate.And(i => i.Datatime >= orders.StartDate && i.Datatime <= orders.EndDate);
            }
            var pageNumber = page ?? 1;
            var pageSize = 25;
            order = _context.Orders.Where(predicate).ToList();
            var onePageOfProducts = order.OrderByDescending(i => i.Id).ToPagedList(pageNumber, pageSize);

           

            return View(onePageOfProducts);


        }
    }
}
