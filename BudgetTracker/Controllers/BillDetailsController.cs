using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using BudgetTracker.Data;
using BudgetTracker.Models.BillDetailsViewModels;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using BudgetTracker.Models.MasterViewModels;
using System.Data.SqlClient;
using BudgetTracker.Models.LoginViewModels;

namespace BudgetTracker.Controllers
{
    public class BillDetailsController : Controller
    {
        private readonly ApplicationDbContext _context;
        public List<BillMasterModel> BillMaster;
        public BillDetailsController(ApplicationDbContext context)
        {
            _context = context;
            BillMaster = _context.BillMaster.ToList();
            //var billMasterDetails = _context.BillMaster;
            //List<BillMasterModel> billMasterList = new List<BillMasterModel>();
            //if (billMasterDetails != null)
            //{

            //    foreach (var bills in billMasterDetails)
            //    {
            //        BillMasterModel billMaster = new BillMasterModel();
            //        billMaster.BillId = bills.BillId;
            //        billMaster.BillName = bills.BillName;
            //        billMasterList.Add(billMaster);
            //    }
            //}
            //ViewData["BillMaterDdl"] = new SelectList(billMasterList, "BillId", "BillName");
        }
        public async Task<IActionResult> Index()
        {
         //   var sorted = _context.BillDetails.GroupBy(y => y.Amount)..Sum(y=>y.Amount)
            if (TempData["userdetails"] != null)
            {

                string Id = TempData["userdetails"].ToString();
                List<ExpenseDetailsModel>ExpenseDetailsList = new List<ExpenseDetailsModel>();
                int UId = Int32.Parse(Id);
                var details = await _context.BillDetails.Where(x => x.UserId == UId).ToListAsync();
                foreach (var item in details)
                {
                    item.BillType = BillMaster.Where(y => y.BillId == item.BillId).Select(x => x.BillName).FirstOrDefault();
                }
                var UserId = new SqlParameter("UserId", UId);
                var ExpenseDetails = await _context.ExpenseDetails
                                 .FromSql("EXECUTE dbo.GetExpenseDetails @UserId", UserId)
                                  .ToListAsync();
                if (ExpenseDetails !=null)
                {
                    ExpenseDetailsList = ExpenseDetails.ToList();
                }
                ViewData["ExpenseDetails"] = ExpenseDetails;
                return View(details);

            }
            else
            {
                return View(await _context.BillDetails.ToListAsync());
            }
        }



        // GET: BillDetails
        //ViewData to populate ddl values
        public IActionResult Transactions(int BillMasterId)
        {
            var billMasterDetails = _context.BillMaster;
            List<BillMasterModel> billMasterList = new List<BillMasterModel>();
            if (billMasterDetails != null)
            {

                foreach (var bills in billMasterDetails)
                {
                    BillMasterModel billMaster = new BillMasterModel();
                    billMaster.BillId = bills.BillId;
                    billMaster.BillName = bills.BillName;
                    billMasterList.Add(billMaster);
                }
            }
            ViewData["BillMaterDdl"] = new SelectList(billMasterList, "BillId", "BillName");
            return View();
        }

        // GET: BillDetails/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            //var BillDetailId = new SqlParameter("BillDetailId", id);
            //var billDetails = await _context.BillDetails
            //                 .FromSql("EXECUTE dbo.GetBillDetails @BillDetailId", BillDetailId)
            //                  .ToListAsync();
            //List<BillDetailsModel> billDetailsList = new List<BillDetailsModel>();


            var billDetails =  _context.BillDetails.Where(x =>x.BillDetailId == id).FirstOrDefault();
            var billMaster = await _context.BillMaster.ToListAsync();

            if (billDetails != null)
            {

                billDetails.BillType = billMaster.Where(x => x.BillId == billDetails.BillId).Select(y => y.BillName).FirstOrDefault();
                
            }
            if (billDetails == null)
            {
                return NotFound();
            }
            return View(billDetails);
        }

        // GET: BillDetails/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: BillDetails/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Transactions([Bind("BillDetailId,BillId,Amount,Notes,BillDate")] BillDetailsModel billDetailsModel)
        {

            if (ModelState.IsValid)
            {
                string dDLValue = Request.Form["BillName"].ToString();
                if (!string.IsNullOrEmpty(dDLValue))
                {
                    billDetailsModel.BillId = Int32.Parse(dDLValue);
                }
                UserDetailsModel details = TempData["userdetails"] as UserDetailsModel;
                if (details != null)
                {
                    billDetailsModel.UserId = details.UserId;
                }
                _context.Add(billDetailsModel);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(billDetailsModel);
        }

        // GET: BillDetails/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            var billMasterDetails = _context.BillMaster;
            List<BillMasterModel> billMasterList = new List<BillMasterModel>();
            if (billMasterDetails != null)
            {

                foreach (var bills in billMasterDetails)
                {
                    BillMasterModel billMaster = new BillMasterModel();
                    billMaster.BillId = bills.BillId;
                    billMaster.BillName = bills.BillName;
                    billMasterList.Add(billMaster);
                }
            }
            ViewData["BillMaterDdl"] = new SelectList(billMasterList, "BillId", "BillName");
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

        // POST: BillDetails/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("BillDetailId,BillId,Amount,Notes")] BillDetailsModel billDetailsModel)
        {
            if (id != billDetailsModel.BillDetailId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    string dDLValue = Request.Form["BillName"].ToString();
                    if (!string.IsNullOrEmpty(dDLValue))
                    {
                        billDetailsModel.BillId = Int32.Parse(dDLValue);
                    }
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

        // GET: BillDetails/Delete/5
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

        // POST: BillDetails/Delete/5
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
