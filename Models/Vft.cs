using System;
using System.Collections.Generic;

namespace VFT.OCT.Models
{
    public partial class Vft
    {
        public string ScanId { get; set; } = null!;
        public string? PatientName { get; set; }
        public string? ReferredDrName { get; set; }
        public string? ReFfacility { get; set; }
        public DateTime? Date { get; set; }
    }
}
