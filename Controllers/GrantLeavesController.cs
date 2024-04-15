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
    [Authorize(Roles = SD.Role_Admin)]
    public class GrantLeavesController : Controller
    {
        private readonly ApplicationDbContext _context;

        public GrantLeavesController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: GrantLeaves
        public async Task<IActionResult> Index()
        {
            //Jag lägger till och kör inclode på 2 tabeller till då jag vill kunna visa data från dem med
            var applicationDbContext = _context.GrantLeaves
                .Include(g => g.ApplyForLeave)
                .ThenInclude(a => a.Leave)
                .Include(g => g.ApplyForLeave)
                .ThenInclude(a => a.Employee);

            return View(await applicationDbContext.ToListAsync());
            
        }

        // GET: GrantLeaves/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            //Lägger även till dem 2 tabellerna här för att kunna visa med saker under Details
            var grantLeave = await _context.GrantLeaves
                .Include(g => g.ApplyForLeave)
                .ThenInclude(a => a.Leave)
                .Include(g => g.ApplyForLeave)
                .ThenInclude(a => a.Employee)
                .FirstOrDefaultAsync(m => m.GrantLeaveId == id);
            if (grantLeave == null)
            {
                return NotFound();
            }

            return View(grantLeave);
        }

        // GET: GrantLeaves/Create
        public IActionResult Create()
        {
            //ViewData["FkApplyForLeaveId"] = new SelectList(_context.ApplyForLeaves, "ApplyForLeaveId", "ApplyForLeaveId");
            ViewBag.ApplyForLeaveInfo = _context.ApplyForLeaves.Select(a => new { ApplyForLeaveId = a.ApplyForLeaveId, DisplayText =
                $"{a.Leave.LeaveType} for {a.Employee.EmployeeName}   ({a.ApplyFromDate}-{a.ApplyToDate})" }).ToList();
            return View();
        }

        // POST: GrantLeaves/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("GrantLeaveId,FkApplyForLeaveId,Granted,DecisionDate")] GrantLeave grantLeave)
        {
            if (ModelState.IsValid)
            {
                _context.Add(grantLeave);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["FkApplyForLeaveId"] = new SelectList(_context.ApplyForLeaves, "ApplyForLeaveId", "ApplyForLeaveId", grantLeave.FkApplyForLeaveId);
            return View(grantLeave);
        }

        // GET: GrantLeaves/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var grantLeave = await _context.GrantLeaves.FindAsync(id);
            if (grantLeave == null)
            {
                return NotFound();
            }
            ViewData["FkApplyForLeaveId"] = new SelectList(_context.ApplyForLeaves, "ApplyForLeaveId", "ApplyForLeaveId", grantLeave.FkApplyForLeaveId);
            return View(grantLeave);
        }

        // POST: GrantLeaves/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("GrantLeaveId,FkApplyForLeaveId,Granted,DecisionDate")] GrantLeave grantLeave)
        {
            if (id != grantLeave.GrantLeaveId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(grantLeave);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!GrantLeaveExists(grantLeave.GrantLeaveId))
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
            ViewData["FkApplyForLeaveId"] = new SelectList(_context.ApplyForLeaves, "ApplyForLeaveId", "ApplyForLeaveId", grantLeave.FkApplyForLeaveId);
            return View(grantLeave);
        }

        // GET: GrantLeaves/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var grantLeave = await _context.GrantLeaves
                .Include(g => g.ApplyForLeave)
                .FirstOrDefaultAsync(m => m.GrantLeaveId == id);
            if (grantLeave == null)
            {
                return NotFound();
            }

            return View(grantLeave);
        }

        // POST: GrantLeaves/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var grantLeave = await _context.GrantLeaves.FindAsync(id);
            if (grantLeave != null)
            {
                _context.GrantLeaves.Remove(grantLeave);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool GrantLeaveExists(int id)
        {
            return _context.GrantLeaves.Any(e => e.GrantLeaveId == id);
        }
    }
}
