using Club.Models;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using Microsoft.EntityFrameworkCore;
using Club.Data;
using Microsoft.Extensions.Logging;
using Club.Models.ViewModels;

namespace Club.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly MainContext _context;

        public HomeController(ILogger<HomeController> logger, MainContext context)
        {
            _logger = logger;
            _context = context; 
        }

        public async Task<IActionResult> Index()
        {
            var latestSessions = await _context.Sessions
                .OrderByDescending(s => s.StartTime)
                .Take(5)
                .ToListAsync();

            var latestFeedbacks = await _context.Feedbacks
                .Include(f => f.Session)
                .OrderByDescending(f => f.FeedbackId)
                .Take(5)
                .ToListAsync();

            var viewModel = new HomeViewModel
            {
                LatestSessions = latestSessions,
                LatestFeedbacks = latestFeedbacks
            };

            return View(viewModel);
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
