using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RoxCorp.Data;

namespace RoxCorp.Controllers
{
    public class EmployeeLeavesController : Controller
    {
        //Lägger med min kontext för att få kontakt med databasen
        private readonly ApplicationDbContext _context;

        public EmployeeLeavesController(ApplicationDbContext context)
        {
            _context = context;
        }

        string selectedLeave = null;

        public async Task<IActionResult> Index(int? leaveId)
        {
            var leaves = await _context.Leaves.ToListAsync();




            return View();
        }
    }
}
