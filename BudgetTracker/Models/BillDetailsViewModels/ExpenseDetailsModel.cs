using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace BudgetTracker.Models.BillDetailsViewModels
{
    public class ExpenseDetailsModel
    {
        [Key]
        public string BillType { get; set; }
        public decimal Amount { get; set; }
    }
}
