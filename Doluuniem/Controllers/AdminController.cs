using Doluuniem.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Doluuniem.Controllers
{
    public class AdminController : Controller
    {
        private TR90Entities db = new TR90Entities();
        // GET: Admin
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult Trangchu()
        {
            return View();
        }
        public ActionResult QuanLySanPham()
        {
            if (Session["VaiTro"] == null || Session["VaiTro"].ToString() != "Admin")
            {
                return RedirectToAction("DangNhap");
            }

            var sanPhams = db.SanPham.ToList();
            return View(sanPhams);
        }
        // GET: Thêm sản phẩm
        public ActionResult ThemSanPham()
        {
            if (Session["VaiTro"] == null || Session["VaiTro"].ToString() != "Admin")
            {
                return RedirectToAction("DangNhap");
            }

            ViewBag.LoaiSanPham = db.LoaiSanPham.ToList();
            return View();
        }


        // POST: Thêm sản phẩm
        [HttpPost]
        public ActionResult ThemSanPham(string TenSanPham, string MoTa, decimal Gia, int SoLuong,
                                       HttpPostedFileBase HinhAnh, int MaLoaiSanPham,
                                       string MauSac, string KichThuoc)
        {
            if (ModelState.IsValid)
            {
                string imagePath = null;

                // Upload ảnh
                if (HinhAnh != null && HinhAnh.ContentLength > 0)
                {
                    string fileName = Path.GetFileName(HinhAnh.FileName);
                    string folderPath = Server.MapPath("~/anh/anh/");

                    if (!Directory.Exists(folderPath))
                    {
                        Directory.CreateDirectory(folderPath);
                    }

                    string path = Path.Combine(folderPath, fileName);
                    HinhAnh.SaveAs(path);
                    imagePath = fileName;
                }

                // Tạo sản phẩm mới
                SanPham sp = new SanPham
                {
                    TenSanPham = TenSanPham,
                    MoTa = MoTa,
                    DonGia = (double?)Gia,
                    HinhAnh = imagePath,
                    SoLuongTon = SoLuong,
                    MaLoaiSanPham = MaLoaiSanPham,

                    // Thuộc tính phù hợp đồ lưu niệm
                    MauSac = MauSac,
                    KichThuoc = KichThuoc
                };

                db.SanPham.Add(sp);
                db.SaveChanges();

                return RedirectToAction("QuanLySanPham");
            }

            ViewBag.LoaiSanPham = db.LoaiSanPham.ToList();
            return View();
        }
        public ActionResult XoaSanPham(int id)
        {
            var sp = db.SanPham.Find(id);

            if (sp == null)
            {
                return HttpNotFound();
            }

            db.SanPham.Remove(sp);
            db.SaveChanges();

            return RedirectToAction("QuanLySanPham");
        }
        // GET
        public ActionResult SuaSanPham(int id)
        {
            var sp = db.SanPham.Find(id);
            if (sp == null)
            {
                return HttpNotFound();
            }

            ViewBag.LoaiSanPham = db.LoaiSanPham.ToList();
            return View(sp);
        }


        // POST
        [HttpPost]
        public ActionResult SuaSanPham(SanPham sanPham, HttpPostedFileBase uploadAnh)
        {
            if (ModelState.IsValid)
            {
                var sp = db.SanPham.Find(sanPham.MaSanPham);
                if (sp == null)
                {
                    return HttpNotFound();
                }

                // cập nhật dữ liệu
                sp.TenSanPham = sanPham.TenSanPham;
                sp.MoTa = sanPham.MoTa;
                sp.DonGia = sanPham.DonGia;
                sp.SoLuongTon = sanPham.SoLuongTon;
                sp.MaLoaiSanPham = sanPham.MaLoaiSanPham;
                sp.MauSac = sanPham.MauSac;
                sp.KichThuoc = sanPham.KichThuoc;

                // upload ảnh mới
                if (uploadAnh != null && uploadAnh.ContentLength > 0)
                {
                    string fileName = Guid.NewGuid() + Path.GetExtension(uploadAnh.FileName);
                    string path = Path.Combine(Server.MapPath("~/anh/anh/"), fileName);

                    uploadAnh.SaveAs(path);
                    sp.HinhAnh = fileName;
                }

                db.SaveChanges();

                TempData["SuccessMessage"] = "Sửa sản phẩm thành công!";
                return RedirectToAction("QuanLySanPham");
            }

            ViewBag.LoaiSanPham = db.LoaiSanPham.ToList();
            return View(sanPham);
        }

        public ActionResult QuanLyKhachHang(string keyword)
        {
            var list = db.NguoiDung.AsQueryable();

            if (!string.IsNullOrEmpty(keyword))
            {
                list = list.Where(x => x.HoTen.Contains(keyword) || x.Email.Contains(keyword));
            }

            return View(list.ToList());
        }

        // XÓA
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult XoaKhachHang(int id)
        {
            var user = db.NguoiDung.Find(id);
            if (user != null)
            {
                db.NguoiDung.Remove(user);
                db.SaveChanges();
            }
            return RedirectToAction("QuanLyKhachHang");
        }

        // KHÓA
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult KhoaKhachHang(int id)
        {
            var user = db.NguoiDung.Find(id);
            if (user != null)
            {
                user.TrangThai = false;
                db.SaveChanges();
            }
            return RedirectToAction("QuanLyKhachHang");
        }

        // MỞ KHÓA
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult MoKhoaKhachHang(int id)
        {
            var user = db.NguoiDung.Find(id);
            if (user != null)
            {
                user.TrangThai = true;
                db.SaveChanges();
            }
            return RedirectToAction("QuanLyKhachHang");
        }










        // Quản lý đơn hàng (Admin)
        public ActionResult DonHang()
        {
            if (Session["VaiTro"] == null || Session["VaiTro"].ToString() != "Admin")
            {
                return RedirectToAction("DangNhap");
            }

            var donhang = db.DonHang.ToList();
            return View(donhang);
        }

        // Chi tiết đơn hàng (Admin)
        public ActionResult ChiTietDonHang(int id)
        {
            var donHang = db.DonHang.Include("NguoiDung").FirstOrDefault(d => d.MaDonHang == id);
            var chiTiet = db.ChiTietDonHang.Include("SanPham").Where(c => c.MaDonHang == id).ToList();

            var viewModel = new ChiTietDonHangViewModel
            {
                DonHang = donHang,
                ChiTietDonHangs = chiTiet
            };

            return View(viewModel);
        }

        // Duyệt đơn hàng (Admin)
        public ActionResult DuyetDonHang()
        {
            var donHangChoDuyet = db.DonHang.ToList();
            return View(donHangChoDuyet);
        }

        public ActionResult DuyetDonHangXacNhan(int id)
        {
            var donHang = db.DonHang.Find(id);
            if (donHang != null && donHang.TrangThai == "Chờ xử lý")
            {
                donHang.TrangThai = "Đã xác nhận";
                db.SaveChanges();
            }
            return RedirectToAction("DonHang");
        }
        public ActionResult ChiTietDonHangKhachHang(int id)
        {
            var donHang = db.DonHang.Include("NguoiDung").FirstOrDefault(d => d.MaDonHang == id);
            var chiTiet = db.ChiTietDonHang.Include("SanPham").Where(c => c.MaDonHang == id).ToList();

            var viewModel = new ChiTietDonHangViewModel
            {
                DonHang = donHang,
                ChiTietDonHangs = chiTiet
            };

            return View(viewModel);
        }
        // HỦY ĐƠN HÀNG
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult HuyDonHang(int id)
        {
            var dh = db.DonHang.Find(id);

            if (dh == null)
            {
                TempData["Error"] = "Đơn hàng không tồn tại!";
                return RedirectToAction("DonHang");
            }

            // ❌ Không cho hủy nếu đã giao
            if (dh.TrangThai == "Đã giao")
            {
                TempData["Error"] = "Không thể hủy đơn đã giao!";
                return RedirectToAction("DonHang");
            }

            // ✔ Hủy đơn
            dh.TrangThai = "Đã hủy";
            db.SaveChanges();

            TempData["Success"] = "Hủy đơn hàng thành công!";
            return RedirectToAction("DonHang");
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult XacNhanDonHang(int id)
        {
            var dh = db.DonHang.Find(id);

            if (dh == null)
            {
                TempData["Error"] = "Đơn hàng không tồn tại!";
                return RedirectToAction("DonHang");
            }

            // ❌ Không xác nhận nếu đã giao hoặc đã hủy
            if (dh.TrangThai == "Đã giao" || dh.TrangThai == "Đã hủy")
            {
                TempData["Error"] = "Không thể xác nhận đơn này!";
                return RedirectToAction("DonHang");
            }

            // ✔ Chỉ xác nhận khi đang chờ
            if (dh.TrangThai == "Chờ xử lý")
            {
                dh.TrangThai = "Đã xác nhận";
                db.SaveChanges();

                TempData["Success"] = "Đã xác nhận đơn hàng!";
            }

            return RedirectToAction("DonHang");
        }
       
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult HuyDonHang1(int id)
        {
            if (Session["UserID"] == null)
            {
                return RedirectToAction("DangNhap");
            }

            int userId = Convert.ToInt32(Session["UserID"]);

            var donHang = db.DonHang.FirstOrDefault(d => d.MaDonHang == id && d.MaNguoiDung == userId);

            if (donHang == null)
            {
                TempData["Error"] = "Không tìm thấy đơn hàng.";
                return RedirectToAction("LichSuDonHang");
            }

            string trangThai = (donHang.TrangThai ?? "").Trim().ToLower();

            bool coTheHuy =
                trangThai == "chờ xác nhận" ||
                trangThai == "cho xac nhan" ||
                trangThai == "đang xử lý" ||
                trangThai == "dang xu ly";

            if (!coTheHuy)
            {
                TempData["Error"] = "Đơn hàng này không thể hủy.";
                return RedirectToAction("LichSuDonHang");
            }

            donHang.TrangThai = "Đã hủy";

            db.SaveChanges();

            TempData["Success"] = "Hủy đơn hàng thành công.";
            return RedirectToAction("LichSuDonHang");
        }




        public ActionResult ThongKeDoanhThu()
        {
            if (Session["UserID"] == null || Session["VaiTro"] == null || Session["VaiTro"].ToString() != "Admin")
            {
                TempData["Error"] = "Bạn không có quyền truy cập trang quản trị.";
                return RedirectToAction("DangNhap", "Home");
            }

            var model = new AdminThongKeViewModel();

            // Tổng quan
            model.TongSanPham = db.SanPham.Count();
            model.TongKhachHang = db.NguoiDung.Count(x => x.VaiTro == "KhachHang");
            model.TongDonHang = db.DonHang.Count();
            model.TongDoanhThu = db.DonHang
                .Where(x => x.TrangThai != "Đã hủy")
                .Sum(x => (double?)x.TongTien) ?? 0;

            // Trạng thái đơn hàng
            model.DonChoXacNhan = db.DonHang.Count(x => x.TrangThai == "Chờ xác nhận" || x.TrangThai == "Chờ xử lý");
            model.DonDangXuLy = db.DonHang.Count(x => x.TrangThai == "Đang xử lý");
            model.DonDangGiao = db.DonHang.Count(x => x.TrangThai == "Đang giao");
            model.DonHoanThanh = db.DonHang.Count(x => x.TrangThai == "Hoàn thành");
            model.DonDaHuy = db.DonHang.Count(x => x.TrangThai == "Đã hủy");

            // Doanh thu 12 tháng gần nhất
            var now = DateTime.Now;
            for (int i = 11; i >= 0; i--)
            {
                var thang = now.AddMonths(-i);
                int month = thang.Month;
                int year = thang.Year;

                double doanhThuThang = db.DonHang
                    .Where(x => x.NgayDat.HasValue
                                && x.NgayDat.Value.Month == month
                                && x.NgayDat.Value.Year == year
                                && x.TrangThai != "Đã hủy")
                    .Sum(x => (double?)x.TongTien) ?? 0;

                model.ThangLabels.Add(thang.ToString("MM/yyyy"));
                model.DoanhThuTheoThang.Add(doanhThuThang);
            }

            // Top sản phẩm bán chạy
            model.TopSanPhamBanChay = db.ChiTietDonHang
                .Include("SanPham")
                .GroupBy(x => new
                {
                    MaSanPham = x.MaSanPham,
                    TenSanPham = x.SanPham.TenSanPham
                })
                .Select(g => new SanPhamBanChayItem
                {
                    MaSanPham = (int)g.Key.MaSanPham,
                    TenSanPham = g.Key.TenSanPham,
                    TongSoLuongBan = g.Sum(x => (int?)x.SoLuong) ?? 0,
                    TongDoanhThu = g.Sum(x => (double?)(x.SoLuong * x.DonGia)) ?? 0
                })
                .OrderByDescending(x => x.TongSoLuongBan)
                .Take(5)
                .ToList();

            // Top khách hàng mua nhiều
            model.TopKhachHang = db.DonHang
                .Include("NguoiDung")
                .Where(x => x.MaNguoiDung != null && x.TrangThai != "Đã hủy")
                .GroupBy(x => new
                {
                    MaNguoiDung = x.MaNguoiDung,
                    HoTen = x.NguoiDung.HoTen,
                    TenDangNhap = x.NguoiDung.TenDangNhap
                })
                .Select(g => new KhachHangMuaNhieuItem
                {
                    MaNguoiDung = g.Key.MaNguoiDung.HasValue ? g.Key.MaNguoiDung.Value : 0,
                    HoTen = g.Key.HoTen,
                    TenDangNhap = g.Key.TenDangNhap,
                    SoDonHang = g.Count(),
                    TongChiTieu = g.Sum(x => (double?)x.TongTien) ?? 0
                })
                .OrderByDescending(x => x.TongChiTieu)
                .Take(5)
                .ToList();

            // Sản phẩm sắp hết hàng
            model.SanPhamSapHet = db.SanPham
                .Include("LoaiSanPham")
                .OrderBy(x => x.SoLuongTon)
                .Take(8)
                .Select(x => new SanPhamTonKhoThapItem
                {
                    MaSanPham = x.MaSanPham,
                    TenSanPham = x.TenSanPham,
                    SoLuongTon = x.SoLuongTon.HasValue ? x.SoLuongTon.Value : 0,
                    TenLoaiSanPham = x.LoaiSanPham != null ? x.LoaiSanPham.TenLoai : ""
                })
                .ToList();

            // Đánh giá mới nhất
            model.DanhGiaMoiNhat = db.DanhGia
                .Include("NguoiDung")
                .Include("SanPham")
                .OrderByDescending(x => x.MaDanhGia)
                .Take(6)
                .Select(x => new DanhGiaMoiItem
                {
                    TenNguoiDung = x.NguoiDung != null ? x.NguoiDung.TenDangNhap : "Ẩn danh",
                    TenSanPham = x.SanPham != null ? x.SanPham.TenSanPham : "Sản phẩm",
                    BinhLuan = x.BinhLuan,
                    SoSao = x.SoSao
                })
                .ToList();

            return View(model);
        }
    }
}