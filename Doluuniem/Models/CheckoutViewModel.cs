using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Doluuniem.Models
{
    public class CheckoutViewModel
    {

        public int MaSanPham { get; set; }
        public string TenSanPham { get; set; }
        public string MoTa { get; set; }
        public decimal DonGia { get; set; }
        public decimal GiaCu { get; set; }
        public string HinhAnh { get; set; }
        public int SoLuong { get; set; }

        // Thông tin người dùng
        public string HoTen { get; set; }
        public string DiaChi { get; set; }
        public string SoDienThoai { get; set; }

        // Thông tin giao hàng và thanh toán
        public string DeliveryMethod { get; set; } // "home" hoặc "store"
        public string City { get; set; }
        public string PaymentMethod { get; set; }
        public bool ChuyenDuLieu { get; set; }
        public bool HoaDonCongTy { get; set; }
        public bool YeuCauKhac { get; set; }
        public string YeuCauKhacText { get; set; }
        public string MaGiamGia { get; set; }
        public bool DungDiem { get; set; }
        public bool DongYChinhSach { get; set; }
    }
}
