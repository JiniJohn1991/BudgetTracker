using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace BudgetTracker.Models.MasterViewModels
{
    public class BillMasterModel
    {
        [Key]
        public int BillId { get; set; }
        public string BillName { get; set; }
    }
}
