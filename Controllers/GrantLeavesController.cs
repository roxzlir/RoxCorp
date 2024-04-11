﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using RoxCorp.Data;
using RoxCorp.Models;

namespace RoxCorp.Controllers
{
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
            var applicationDbContext = _context.GrantLeaves.Include(g => g.ApplyForLeave);
            return View(await applicationDbContext.ToListAsync());
        }

        // GET: GrantLeaves/Details/5
        public async Task<IActionResult> Details(int? id)
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

        // GET: GrantLeaves/Create
        public IActionResult Create()
        {
            ViewData["FkApplyForLeaveId"] = new SelectList(_context.ApplyForLeaves, "ApplyForLeaveId", "ApplyForLeaveId");
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