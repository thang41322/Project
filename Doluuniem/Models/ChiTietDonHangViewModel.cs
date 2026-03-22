using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Doluuniem.Models
{
    public class ChiTietDonHangViewModel
    {
        public DonHang DonHang { get; set; }
        public List<ChiTietDonHang> ChiTietDonHangs { get; set; }   
    }
}