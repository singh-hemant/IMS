using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using IMSApp.Data;
using IMSApp.Models;

namespace IMSApp.Controllers
{
    public class SalesController : Controller
    {
        private readonly ApplicationDbContext _context;

        public SalesController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Sales
        public async Task<IActionResult> Index()
        {
            var totalSales = await _context.Sales.SumAsync(s => s.TotalAmount);
            var salesByBrandCategoryOrderDate = await _context.Sales
            .Include(s => s.Order)
            .ThenInclude(o => o.Product)
            .ThenInclude(p => p.Category)
            .Select(s => new
            {
                Brand = s.Order.Product.Brand,
                Category = s.Order.Product.Category.Name,
                TotalAmount = s.TotalAmount
             })
            .ToListAsync();

            ViewBag.TotalSales = totalSales;
            ViewBag.SalesByBrandCategoryOrderDate = salesByBrandCategoryOrderDate;

            var applicationDbContext = _context.Sales.Include(s => s.Order);
            return View(await applicationDbContext.ToListAsync());
        }

        
    }
}
