using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using BudgetTracker.Data;
using BudgetTracker.Models.LoginViewModels;

namespace BudgetTracker.Controllers
{
    public class LoginController : Controller
    {
        private readonly ApplicationDbContext _context;

        public LoginController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Login
        public async Task<IActionResult> Index()
        {
            return View(await _context.UserDetails.ToListAsync());
        }

        // GET: Login/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var userDetailsModel = await _context.UserDetails
                .SingleOrDefaultAsync(m => m.UserId == id);
            if (userDetailsModel == null)
            {
                return NotFound();
            }

            return View(userDetailsModel);
        }

        // GET: Login/Create
        public IActionResult Register()
        {
            return View();
        }

        // POST: Login/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Register([Bind("UserId,UserName,Password")] UserDetailsModel userDetailsModel)
        {
            if (ModelState.IsValid)
            {
                _context.Add(userDetailsModel);
                await _context.SaveChangesAsync();
                return RedirectToAction("Index", "BillDetails");
                
            }
            return View(userDetailsModel);
        }

        // GET: Login/Edit/5
        public async Task<IActionResult> Login(UserDetailsModel userDetails)
        {
            if (ModelState.IsValid)
            {
                var userDetailsModel = await _context.UserDetails.SingleOrDefaultAsync(m => m.UserName == userDetails.UserName
                                                                                        && m.Password == userDetails.Password);



                if (userDetailsModel != null)
                {
                    TempData["userdetails"] = userDetailsModel.UserId;
                    return RedirectToAction("Index", "BillDetails");
                }
                else
                {
                    return View(userDetails);
                }
            }
            else
            {
                return View(userDetails);
            }

        }

        // POST: Login/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]

        // GET: Login/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var userDetailsModel = await _context.UserDetails
                .SingleOrDefaultAsync(m => m.UserId == id);
            if (userDetailsModel == null)
            {
                return NotFound();
            }

            return View(userDetailsModel);
        }

        // POST: Login/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var userDetailsModel = await _context.UserDetails.SingleOrDefaultAsync(m => m.UserId == id);
            _context.UserDetails.Remove(userDetailsModel);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool UserDetailsModelExists(int id)
        {
            return _context.UserDetails.Any(e => e.UserId == id);
        }
    }
}
