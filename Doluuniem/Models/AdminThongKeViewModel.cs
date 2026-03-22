using System.Collections.Generic;

namespace Doluuniem.Models
{
    public class AdminThongKeViewModel
    {
        public int TongSanPham { get; set; }
        public int TongKhachHang { get; set; }
        public int TongDonHang { get; set; }
        public double TongDoanhThu { get; set; }

        public int DonChoXacNhan { get; set; }
        public int DonDangXuLy { get; set; }
        public int DonDangGiao { get; set; }
        public int DonHoanThanh { get; set; }
        public int DonDaHuy { get; set; }

        public List<string> ThangLabels { get; set; }
        public List<double> DoanhThuTheoThang { get; set; }

        public List<SanPhamBanChayItem> TopSanPhamBanChay { get; set; }
        public List<KhachHangMuaNhieuItem> TopKhachHang { get; set; }
        public List<SanPhamTonKhoThapItem> SanPhamSapHet { get; set; }
        public List<DanhGiaMoiItem> DanhGiaMoiNhat { get; set; }

        public AdminThongKeViewModel()
        {
            ThangLabels = new List<string>();
            DoanhThuTheoThang = new List<double>();
            TopSanPhamBanChay = new List<SanPhamBanChayItem>();
            TopKhachHang = new List<KhachHangMuaNhieuItem>();
            SanPhamSapHet = new List<SanPhamTonKhoThapItem>();
            DanhGiaMoiNhat = new List<DanhGiaMoiItem>();
        }
    }

    public class SanPhamBanChayItem
    {
        public int MaSanPham { get; set; }
        public string TenSanPham { get; set; }
        public int TongSoLuongBan { get; set; }
        public double TongDoanhThu { get; set; }
    }

    public class KhachHangMuaNhieuItem
    {
        public int MaNguoiDung { get; set; }
        public string HoTen { get; set; }
        public string TenDangNhap { get; set; }
        public int SoDonHang { get; set; }
        public double TongChiTieu { get; set; }
    }

    public class SanPhamTonKhoThapItem
    {
        public int MaSanPham { get; set; }
        public string TenSanPham { get; set; }
        public int SoLuongTon { get; set; }
        public string TenLoaiSanPham { get; set; }
    }

    public class DanhGiaMoiItem
    {
        public string TenNguoiDung { get; set; }
        public string TenSanPham { get; set; }
        public string BinhLuan { get; set; }
        public int? SoSao { get; set; }
    }
}