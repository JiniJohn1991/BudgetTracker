using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using BudgetTracker.Data;
using BudgetTracker.Models.BillDetailsViewModels;

namespace BudgetTracker.Controllers
{
    public class BillDisplay : Controller
    {
        private readonly ApplicationDbContext _context;

        public BillDisplay(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: BillDisplay
        public async Task<IActionResult> Index()
        {
            return View(await _context.BillDetails.ToListAsync());
        }

        // GET: BillDisplay/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var billDetailsModel = await _context.BillDetails
                .SingleOrDefaultAsync(m => m.BillDetailId == id);
            if (billDetailsModel == null)
            {
                return NotFound();
            }

            return View(billDetailsModel);
        }

        // GET: BillDisplay/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: BillDisplay/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("BillDetailId,BillId,Amount,Notes,BillDate")] BillDetailsModel billDetailsModel)
        {
            if (ModelState.IsValid)
            {
                _context.Add(billDetailsModel);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(billDetailsModel);
        }

        // GET: BillDisplay/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var billDetailsModel = await _context.BillDetails.SingleOrDefaultAsync(m => m.BillDetailId == id);
            if (billDetailsModel == null)
            {
                return NotFound();
            }
            return View(billDetailsModel);
        }

        // POST: BillDisplay/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("BillDetailId,BillId,Amount,Notes,BillDate")] BillDetailsModel billDetailsModel)
        {
            if (id != billDetailsModel.BillDetailId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(billDetailsModel);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!BillDetailsModelExists(billDetailsModel.BillDetailId))
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
            return View(billDetailsModel);
        }

        // GET: BillDisplay/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var billDetailsModel = await _context.BillDetails
                .SingleOrDefaultAsync(m => m.BillDetailId == id);
            if (billDetailsModel == null)
            {
                return NotFound();
            }

            return View(billDetailsModel);
        }

        // POST: BillDisplay/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var billDetailsModel = await _context.BillDetails.SingleOrDefaultAsync(m => m.BillDetailId == id);
            _context.BillDetails.Remove(billDetailsModel);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool BillDetailsModelExists(int id)
        {
            return _context.BillDetails.Any(e => e.BillDetailId == id);
        }
    }
}
