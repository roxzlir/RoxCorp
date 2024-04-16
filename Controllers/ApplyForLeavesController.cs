using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using RoxCorp.Data;
using RoxCorp.Models;
using RoxCorp.Utility;

namespace RoxCorp.Controllers
{
    public class ApplyForLeavesController : Controller
    {
        private readonly ApplicationDbContext _context;

        public ApplyForLeavesController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: ApplyForLeaves
        public async Task<IActionResult> Index()
        {
            var applicationDbContext = _context.ApplyForLeaves.Include(a => a.Employee).Include(a => a.Leave).OrderByDescending(x => x.ApplyRegisteredDate);
            return View(await applicationDbContext.ToListAsync());
        }

        // GET: ApplyForLeaves/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var applyForLeave = await _context.ApplyForLeaves
                .Include(a => a.Employee)
                .Include(a => a.Leave)
                .FirstOrDefaultAsync(m => m.ApplyForLeaveId == id);
            if (applyForLeave == null)
            {
                return NotFound();
            }

            return View(applyForLeave);
        }

        // GET: ApplyForLeaves/Create
        public IActionResult Create()
        {
            ViewData["FkEmployeeId"] = new SelectList(_context.Employees, "EmployeeId", "EmployeeName");
            ViewData["FkLeaveId"] = new SelectList(_context.Leaves, "LeaveId", "LeaveType");
            return View();
        }

        // POST: ApplyForLeaves/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("ApplyForLeaveId,FkLeaveId,FkEmployeeId,ApplyFromDate,ApplyToDate,ApplyNote")] ApplyForLeave applyForLeave)
        {
            if (ModelState.IsValid)
            {
                applyForLeave.ApplyRegisteredDate = DateTime.Now; // Sätter att ApplyRegisteredDate får dagens datum
                _context.Add(applyForLeave);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["FkEmployeeId"] = new SelectList(_context.Employees, "EmployeeId", "EmployeeName", applyForLeave.FkEmployeeId);
            ViewData["FkLeaveId"] = new SelectList(_context.Leaves, "LeaveId", "LeaveType", applyForLeave.FkLeaveId);
            return View(applyForLeave);
        }

        // GET: ApplyForLeaves/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var applyForLeave = await _context.ApplyForLeaves.FindAsync(id);
            if (applyForLeave == null)
            {
                return NotFound();
            }
            ViewData["FkEmployeeId"] = new SelectList(_context.Employees, "EmployeeId", "EmployeeName", applyForLeave.FkEmployeeId);
            ViewData["FkLeaveId"] = new SelectList(_context.Leaves, "LeaveId", "LeaveType", applyForLeave.FkLeaveId);
            return View(applyForLeave);
        }

        // POST: ApplyForLeaves/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("ApplyForLeaveId,FkLeaveId,FkEmployeeId,ApplyFromDate,ApplyToDate,ApplyNote")] ApplyForLeave applyForLeave)
        {
            if (id != applyForLeave.ApplyForLeaveId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    applyForLeave.ApplyRegisteredDate = DateTime.Now; // Sätter att ApplyRegisteredDate får dagens datum
                    _context.Update(applyForLeave);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ApplyForLeaveExists(applyForLeave.ApplyForLeaveId))
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
            ViewData["FkEmployeeId"] = new SelectList(_context.Employees, "EmployeeId", "EmployeeName", applyForLeave.FkEmployeeId);
            ViewData["FkLeaveId"] = new SelectList(_context.Leaves, "LeaveId", "LeaveType", applyForLeave.FkLeaveId);
            return View(applyForLeave);
        }

        // GET: ApplyForLeaves/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var applyForLeave = await _context.ApplyForLeaves
                .Include(a => a.Employee)
                .Include(a => a.Leave)
                .FirstOrDefaultAsync(m => m.ApplyForLeaveId == id);
            if (applyForLeave == null)
            {
                return NotFound();
            }

            return View(applyForLeave);
        }

        // POST: ApplyForLeaves/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var applyForLeave = await _context.ApplyForLeaves.FindAsync(id);
            if (applyForLeave != null)
            {
                _context.ApplyForLeaves.Remove(applyForLeave);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ApplyForLeaveExists(int id)
        {
            return _context.ApplyForLeaves.Any(e => e.ApplyForLeaveId == id);
        }


        //Här lägger jag så man ska kunna kolla efter en månad om vilka som sökt ledighet
        public async Task<IActionResult> MonthlySummary(int? year, int? month)
        {
            if (!year.HasValue || !month.HasValue)
            {
                return View(new MonthlySummaryViewModel { Year = DateTime.Now.Year, Month = DateTime.Now.Month, Applies = new List<ApplyForLeave>() });
            }

            var applies = await _context.ApplyForLeaves
                .Include(x => x.Employee)
                .Include(x => x.Leave)
                .Where(x => x.ApplyFromDate.Year == year.Value && x.ApplyFromDate.Month == month.Value)
                .ToListAsync();

            var viewModel = new MonthlySummaryViewModel
            {
                Year = year.Value,
                Month = month.Value,
                Applies = applies
            };

            return View(viewModel);
        }

    }
}
