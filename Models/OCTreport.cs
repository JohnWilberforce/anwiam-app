using System.ComponentModel.DataAnnotations;

namespace VFT.OCT.Models
{
    public class OCTreport
    {
        [Key]
        public string? referredDrName { get; set; }
        //public string? reFfacility { get; set; }
        public int ONHcount { get; set; }
        public int MaculaCount { get; set; }
        public int PachCount { get; set; }  
        public decimal Amount { get; set; }

    }
}
