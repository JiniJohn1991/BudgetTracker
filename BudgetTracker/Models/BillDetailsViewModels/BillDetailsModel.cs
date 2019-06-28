using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;


namespace BudgetTracker.Models.BillDetailsViewModels
{
    public class BillDetailsModel
    {
        [Key]
        public int BillDetailId { get; set; }
        public int BillId { get; set; }
        public decimal Amount { get; set; }
        public string Notes { get; set; }
        public DateTime BillDate { get; set; }
        public int UserId { get; set; }
        [NotMapped]
        public string BillType { get; set; }
        
    }
}
