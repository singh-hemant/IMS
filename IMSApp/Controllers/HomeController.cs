using IMSApp.Data;
using IMSApp.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;

namespace IMSApp.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly ApplicationDbContext _context;
        public HomeController(ApplicationDbContext context, ILogger<HomeController> logger)
        {
            _context = context;
            _logger = logger;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult BuyNow()
        {
            var applicationDbContext = _context.Products.Include(p => p.Category);
            return View(applicationDbContext.ToList());
        }
        public IActionResult PlaceOrderDetails(int id)
        {
            // Retrieve the product by id
            var product = _context.Products
                                .Where(p => p.ProductId == id)
                                .FirstOrDefault();

            if (product == null)
            {
                // Handle if the product is not found
                return NotFound();
            }

            return View(product);
        }

        [HttpGet]
        public IActionResult PlaceOrder(int id)
        {
            // Retrieve the product based on the id
            var product = _context.Products.Find(id);

            if (product == null)
            {
                // Handle if the product is not found
                return NotFound();
            }

            // Create a new Order object
            var order = new Order
            {
                ProductId = id,
                Product = product,
                Quantity = 1,
                OrderDate = DateTime.Now,
                Status = "Confirmed" // You can set the initial status as needed
            };

            // Add the order to the database
            _context.Orders.Add(order);
            _context.SaveChanges();

            // Create a new Sale object
            var sale = new Sale
            {
                OrderId = order.OrderId,
                Order = order,
                SaleDate = DateTime.Now,
                TotalAmount = (int)(order.Quantity * product.Price) // Assuming TotalAmount is integer type
            };

            // Add the sale to the database
            _context.Sales.Add(sale);
            _context.SaveChanges();

            // Redirect to a confirmation page or any other page
            return RedirectToAction("OrderConfirmation", new { orderId = order.OrderId });
        }
        public IActionResult OrderConfirmation(int orderId)
        {
            // Retrieve the order by orderId
            var order = _context.Orders
                                .Include(o => o.Product) // Include related product information
                                .Where(o => o.OrderId == orderId)
                                .FirstOrDefault();

            if (order == null)
            {
                // Handle if the order is not found
                return NotFound();
            }

            return View(order);
        }

        public IActionResult ContactUs()
        {
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
