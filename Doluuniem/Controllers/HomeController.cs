using Doluuniem.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Script.Serialization;

namespace Doluuniem.Controllers
{
    public class HomeController : Controller
    {
        private TR90Entities db = new TR90Entities();

        private bool ChuaDangNhap()
        {
            return Session["UserID"] == null;
        }

        private ActionResult YeuCauDangNhap(string message)
        {
            TempData["Error"] = message;
            return RedirectToAction("DangNhap", "Home");
        }

        public ActionResult Index(int page = 1)
        {
            try
            {
                int soSanPhamMoiTrang = 6;
                var danhSachSanPham = db.SanPham.AsQueryable();

                int tongSoSanPham = danhSachSanPham.Count();
                if (tongSoSanPham == 0)
                {
                    ViewBag.TrangHienTai = 1;
                    ViewBag.TongSoTrang = 1;
                    return View(new List<SanPham>());
                }

                int tongSoTrang = (int)Math.Ceiling((double)tongSoSanPham / soSanPhamMoiTrang);
                page = Math.Max(1, Math.Min(page, tongSoTrang));

                var sanPhamPhanTrang = danhSachSanPham
                    .OrderBy(sp => sp.MaSanPham)
                    .Skip((page - 1) * soSanPhamMoiTrang)
                    .Take(soSanPhamMoiTrang)
                    .ToList();

                ViewBag.TrangHienTai = page;
                ViewBag.TongSoTrang = tongSoTrang;

                return View(sanPhamPhanTrang);
            }
            catch (Exception ex)
            {
                TempData["Error"] = "Có lỗi xảy ra khi tải danh sách sản phẩm: " + ex.Message;
                return View(new List<SanPham>());
            }
        }

        public ActionResult About()
        {
            return View();
        }

        public ActionResult Contact()
        {
            return View();
        }

        public ActionResult DangNhap()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult DangNhap(string email, string matkhau)
        {
            if (string.IsNullOrEmpty(email) || string.IsNullOrEmpty(matkhau))
            {
                ViewBag.Error = "Vui lòng nhập đầy đủ!";
                return View();
            }

            var user = db.NguoiDung.FirstOrDefault(x => x.Email == email && x.MatKhau == matkhau);

            if (user == null)
            {
                ViewBag.Error = "Sai tài khoản hoặc mật khẩu!";
                return View();
            }

            if ((bool)!user.TrangThai)
            {
                ViewBag.Error = "Tài khoản bị khóa!";
                return View();
            }

            Session["UserID"] = user.MaNguoiDung;
            Session["TenDangNhap"] = user.TenDangNhap;
            Session["VaiTro"] = user.VaiTro;

            if (user.VaiTro == "Admin")
                return RedirectToAction("Trangchu", "Admin");

            return RedirectToAction("Index", "Home");
        }

        public ActionResult DangXuat()
        {
            Session.Clear();
            return RedirectToAction("Index");
        }

        public ActionResult DangKy()
        {
            return View();
        }

        [HttpPost]
        public ActionResult DangKy(string TenDangNhap, string MatKhau, string Email, string HoTen, DateTime NgaySinh, string SoDienThoai, string DiaChi)
        {
            var khachTheoTen = db.NguoiDung.FirstOrDefault(k => k.TenDangNhap == TenDangNhap);
            if (khachTheoTen != null)
            {
                ViewBag.ThongBao = "Tên đăng nhập đã tồn tại!";
                return View();
            }

            var khachTheoEmail = db.NguoiDung.FirstOrDefault(k => k.Email == Email);
            if (khachTheoEmail != null)
            {
                ViewBag.ThongBao = "Email đã tồn tại!";
                return View();
            }

            NguoiDung kh = new NguoiDung()
            {
                TenDangNhap = TenDangNhap,
                MatKhau = MatKhau,
                Email = Email,
                VaiTro = "KhachHang",
                HoTen = HoTen,
                SoDienThoai = SoDienThoai,
                DiaChi = DiaChi
            };

            db.NguoiDung.Add(kh);
            db.SaveChanges();

            TempData["ThongBao"] = "Đăng ký thành công! Mời bạn đăng nhập.";
            return RedirectToAction("DangNhap");
        }

        public ActionResult ThongTinKhachHang()
        {
            if (ChuaDangNhap())
                return YeuCauDangNhap("Bạn cần phải đăng nhập để xem thông tin tài khoản.");

            int userID = (int)Session["UserID"];
            var user = db.NguoiDung.FirstOrDefault(u => u.MaNguoiDung == userID);

            if (user == null)
                return RedirectToAction("DangNhap");

            return View(user);
        }

        [HttpGet]
        public ActionResult ChinhSuaThongTin()
        {
            if (ChuaDangNhap())
                return YeuCauDangNhap("Bạn cần phải đăng nhập để chỉnh sửa thông tin.");

            int userId = (int)Session["UserID"];
            var user = db.NguoiDung.FirstOrDefault(x => x.MaNguoiDung == userId);

            if (user == null)
                return RedirectToAction("DangNhap");

            return View(user);
        }

        [HttpPost]
        public ActionResult ChinhSuaThongTin(NguoiDung model, HttpPostedFileBase AvatarFile)
        {
            if (ChuaDangNhap())
                return YeuCauDangNhap("Bạn cần phải đăng nhập để chỉnh sửa thông tin.");

            var user = db.NguoiDung.FirstOrDefault(x => x.MaNguoiDung == model.MaNguoiDung);
            if (user != null)
            {
                user.Email = model.Email;
                user.HoTen = model.HoTen;
                user.SoDienThoai = model.SoDienThoai;
                user.DiaChi = model.DiaChi;

                if (AvatarFile != null && AvatarFile.ContentLength > 0)
                {
                    var fileName = Path.GetFileName(AvatarFile.FileName);
                    var path = Path.Combine(Server.MapPath("~/anh/avata"), fileName);
                    AvatarFile.SaveAs(path);
                    user.Avatar = fileName;
                }

                db.SaveChanges();
                TempData["ThongBao"] = "Cập nhật thông tin thành công!";
                return RedirectToAction("ThongTinKhachHang");
            }

            ViewBag.ThongBao = "Cập nhật thất bại!";
            return View(model);
        }

        [HttpPost]
        public ActionResult ThemVaoGioHang(int maSanPham)
        {
            if (ChuaDangNhap())
                return YeuCauDangNhap("Bạn cần phải đăng nhập để thêm sản phẩm vào giỏ hàng.");

            int userId = (int)Session["UserID"];

            var sanPham = db.SanPham.FirstOrDefault(sp => sp.MaSanPham == maSanPham);
            if (sanPham == null)
            {
                TempData["Error"] = "Sản phẩm không tồn tại.";
                return RedirectToAction("Index");
            }

            var gioHang = db.GioHang.FirstOrDefault(g => g.MaNguoiDung == userId && g.MaSanPham == maSanPham);
            if (gioHang != null)
            {
                gioHang.SoLuong += 1;
            }
            else
            {
                db.GioHang.Add(new GioHang
                {
                    MaNguoiDung = userId,
                    MaSanPham = maSanPham,
                    SoLuong = 1
                });
            }

            db.SaveChanges();
            TempData["Success"] = "Đã thêm sản phẩm vào giỏ hàng!";
            return RedirectToAction("GioHang");
        }

        public ActionResult GioHang()
        {
            if (ChuaDangNhap())
                return YeuCauDangNhap("Bạn cần phải đăng nhập để xem giỏ hàng.");

            int userId = (int)Session["UserID"];
            var gioHang = db.GioHang
                .Include("SanPham")
                .Where(g => g.MaNguoiDung == userId)
                .ToList();

            var user = db.NguoiDung.FirstOrDefault(u => u.MaNguoiDung == userId);
            ViewBag.DefaultDiaChi = user?.DiaChi;
            ViewBag.DefaultSoDienThoai = user?.SoDienThoai;

            return View(gioHang);
        }

        [HttpPost]
        public ActionResult CapNhatGioHang(List<GioHang> gioHang)
        {
            if (ChuaDangNhap())
                return YeuCauDangNhap("Bạn cần phải đăng nhập để cập nhật giỏ hàng.");

            if (gioHang == null || !gioHang.Any())
                return RedirectToAction("GioHang");

            int userId = (int)Session["UserID"];

            foreach (var item in gioHang)
            {
                var gio = db.GioHang.FirstOrDefault(g => g.MaNguoiDung == userId && g.MaSanPham == item.MaSanPham);
                if (gio != null)
                {
                    gio.SoLuong = item.SoLuong < 1 ? 1 : item.SoLuong;
                }
            }

            db.SaveChanges();
            TempData["Success"] = "Cập nhật giỏ hàng thành công.";
            return RedirectToAction("GioHang");
        }

        public ActionResult XoaKhoiGioHang(int maSanPham)
        {
            if (ChuaDangNhap())
                return YeuCauDangNhap("Bạn cần phải đăng nhập để thao tác với giỏ hàng.");

            int userId = (int)Session["UserID"];
            var gio = db.GioHang.FirstOrDefault(g => g.MaNguoiDung == userId && g.MaSanPham == maSanPham);

            if (gio != null)
            {
                db.GioHang.Remove(gio);
                db.SaveChanges();
                TempData["Success"] = "Đã xóa sản phẩm khỏi giỏ hàng.";
            }

            return RedirectToAction("GioHang");
        }

        public ActionResult ChiTietSanPham(int id)
        {
            try
            {
                var sanPham = db.SanPham
                    .Include("LoaiSanPham")
                    .Include("DanhGia.NguoiDung")
                    .Include("ChiTietSanPham")
                    .FirstOrDefault(sp => sp.MaSanPham == id);

                if (sanPham == null)
                {
                    TempData["Error"] = "Sản phẩm không tồn tại.";
                    return RedirectToAction("Index");
                }

                return View(sanPham);
            }
            catch (Exception ex)
            {
                TempData["Error"] = "Có lỗi xảy ra khi tải chi tiết sản phẩm: " + ex.Message;
                return RedirectToAction("Index");
            }
        }

        [HttpPost]
        public ActionResult ThemDanhGia(int maSanPham, string noiDung)
        {
            try
            {
                if (ChuaDangNhap())
                    return YeuCauDangNhap("Bạn cần phải đăng nhập để gửi đánh giá.");

                int userId = (int)Session["UserID"];
                var sanPham = db.SanPham.FirstOrDefault(sp => sp.MaSanPham == maSanPham);
                if (sanPham == null)
                {
                    TempData["Error"] = "Sản phẩm không tồn tại.";
                    return RedirectToAction("Index");
                }

                if (string.IsNullOrWhiteSpace(noiDung))
                {
                    TempData["Error"] = "Vui lòng nhập nội dung đánh giá!";
                    return RedirectToAction("ChiTietSanPham", new { id = maSanPham });
                }

                var danhGia = new DanhGia
                {
                    MaNguoiDung = userId,
                    MaSanPham = maSanPham,
                    BinhLuan = noiDung
                };

                db.DanhGia.Add(danhGia);
                db.SaveChanges();

                TempData["Success"] = "Đánh giá của bạn đã được gửi!";
                return RedirectToAction("ChiTietSanPham", new { id = maSanPham });
            }
            catch (Exception ex)
            {
                TempData["Error"] = "Có lỗi xảy ra khi gửi đánh giá: " + ex.Message;
                return RedirectToAction("ChiTietSanPham", new { id = maSanPham });
            }
        }

        [HttpGet]
        public ActionResult Search(string keyword)
        {
            var danhSachSanPham = db.SanPham.AsQueryable();

            if (!string.IsNullOrEmpty(keyword))
            {
                keyword = keyword.ToLower().Trim();

                danhSachSanPham = danhSachSanPham.Where(sp =>
                    sp.TenSanPham.ToLower().Contains(keyword) ||
                    sp.MoTa.ToLower().Contains(keyword));
            }

            var result = danhSachSanPham.ToList();
            ViewBag.Keyword = keyword;
            ViewBag.LoaiSanPham = db.LoaiSanPham.ToList();
            return View("Index", result);
        }

        [HttpGet]
        public ActionResult SearchByCategory(int? maLoaiSanPham)
        {
            var danhSachSanPham = db.SanPham.AsQueryable();

            if (maLoaiSanPham.HasValue)
            {
                danhSachSanPham = danhSachSanPham.Where(sp => sp.MaLoaiSanPham == maLoaiSanPham.Value);
            }

            var result = danhSachSanPham.ToList();
            ViewBag.MaLoaiSanPham = maLoaiSanPham;
            ViewBag.LoaiSanPham = db.LoaiSanPham.ToList();
            return View("Index", result);
        }

        public ActionResult LichSuDonHang()
        {
            if (ChuaDangNhap())
                return YeuCauDangNhap("Bạn cần phải đăng nhập để xem lịch sử đơn hàng.");

            int userId = (int)Session["UserID"];
            var donHangs = db.DonHang
                .Where(dh => dh.MaNguoiDung == userId)
                .OrderByDescending(dh => dh.NgayDat)
                .ToList();

            return View(donHangs);
        }

        public ActionResult ChiTietDonHangKhachHang(int id)
        {
            if (ChuaDangNhap())
                return YeuCauDangNhap("Bạn cần phải đăng nhập để xem chi tiết đơn hàng.");

            int userId = (int)Session["UserID"];

            var donHang = db.DonHang
                .Include("NguoiDung")
                .FirstOrDefault(d => d.MaDonHang == id && d.MaNguoiDung == userId);

            if (donHang == null)
            {
                TempData["Error"] = "Không tìm thấy đơn hàng.";
                return RedirectToAction("LichSuDonHang");
            }

            var chiTiet = db.ChiTietDonHang
                .Include("SanPham")
                .Where(c => c.MaDonHang == id)
                .ToList();

            var viewModel = new ChiTietDonHangViewModel
            {
                DonHang = donHang,
                ChiTietDonHangs = chiTiet
            };

            return View(viewModel);
        }

        [HttpGet]
        public ActionResult MuaHang(int id)
        {
            if (ChuaDangNhap())
                return YeuCauDangNhap("Bạn cần phải đăng nhập để mua hàng.");

            int userId = (int)Session["UserID"];
            var user = db.NguoiDung.FirstOrDefault(u => u.MaNguoiDung == userId);
            if (user == null)
            {
                return RedirectToAction("DangNhap");
            }

            var product = db.SanPham.FirstOrDefault(p => p.MaSanPham == id);
            if (product == null)
            {
                TempData["Error"] = "Sản phẩm không tồn tại.";
                return RedirectToAction("Index");
            }

            var viewModel = new CheckoutViewModel
            {
                MaSanPham = product.MaSanPham,
                TenSanPham = product.TenSanPham,
                MoTa = product.MoTa,
                DonGia = (decimal)product.DonGia,
                GiaCu = (decimal)product.DonGia + 10000,
                HinhAnh = product.HinhAnh,
                SoLuong = 1,
                HoTen = user.HoTen,
                DiaChi = user.DiaChi,
                SoDienThoai = user.SoDienThoai,
                City = "hcm"
            };

            return View(viewModel);
        }

        [HttpPost]
        public ActionResult MuaHang(CheckoutViewModel model)
        {
            if (ChuaDangNhap())
                return YeuCauDangNhap("Bạn cần phải đăng nhập để mua hàng.");

            int userId = (int)Session["UserID"];
            var user = db.NguoiDung.FirstOrDefault(u => u.MaNguoiDung == userId);
            if (user == null)
            {
                return RedirectToAction("DangNhap");
            }

            var product = db.SanPham.FirstOrDefault(p => p.MaSanPham == model.MaSanPham);
            if (product == null)
            {
                TempData["Error"] = "Sản phẩm không tồn tại.";
                return RedirectToAction("Index");
            }

            if (product.SoLuongTon < model.SoLuong)
            {
                TempData["Error"] = "Sản phẩm không đủ số lượng trong kho.";
                return RedirectToAction("ChiTietSanPham", new { id = model.MaSanPham });
            }

            decimal tongTien = (decimal)(product.DonGia * model.SoLuong);

            if (!string.IsNullOrEmpty(model.MaGiamGia))
            {
                tongTien *= 0.9m;
            }

            if (model.DungDiem)
            {
                tongTien -= 150000;
                if (tongTien < 0) tongTien = 0;
            }

            var yeuCauDacBiet = new
            {
                
                HoaDonCongTy = model.HoaDonCongTy,
                YeuCauKhac = model.YeuCauKhac ? model.YeuCauKhacText : null
            };

            string yeuCauDacBietJson = new JavaScriptSerializer().Serialize(yeuCauDacBiet);

            var donHang = new DonHang
            {
                MaNguoiDung = userId,
                NgayDat = DateTime.Now,
                TrangThai = "Chờ xác nhận",
                TongTien = (double?)tongTien
            };

            db.DonHang.Add(donHang);
            db.SaveChanges();

            var chiTietDonHang = new ChiTietDonHang
            {
                MaDonHang = donHang.MaDonHang,
                MaSanPham = product.MaSanPham,
                SoLuong = model.SoLuong,
                DonGia = product.DonGia
            };

            db.ChiTietDonHang.Add(chiTietDonHang);

            product.SoLuongTon -= model.SoLuong;
            db.SaveChanges();

            TempData["Success"] = "Đặt hàng thành công!";
            return RedirectToAction("LichSuDonHang");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult HuyDonHang(int id)
        {
            if (ChuaDangNhap())
                return YeuCauDangNhap("Bạn cần phải đăng nhập để hủy đơn hàng.");

            int userId = (int)Session["UserID"];

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
                trangThai == "dang xu ly" ||
                trangThai == "chờ xử lý" ||
                trangThai == "cho xu ly";

            if (!coTheHuy)
            {
                TempData["Error"] = "Đơn hàng này không thể hủy.";
                return RedirectToAction("LichSuDonHang");
            }

            var dsChiTiet = db.ChiTietDonHang.Where(ct => ct.MaDonHang == donHang.MaDonHang).ToList();

            foreach (var item in dsChiTiet)
            {
                var sanPham = db.SanPham.FirstOrDefault(sp => sp.MaSanPham == item.MaSanPham);
                if (sanPham != null)
                {
                    sanPham.SoLuongTon += item.SoLuong;
                }
            }

            donHang.TrangThai = "Đã hủy";
            db.SaveChanges();

            TempData["Success"] = "Hủy đơn hàng thành công.";
            return RedirectToAction("LichSuDonHang");
        }








        [HttpGet]
        public ActionResult SanPham(string keyword, string loai, string gia, string sort, int page = 1)
        {
            int soSanPhamMoiTrang = 12;

            var query = db.SanPham
                .Include("LoaiSanPham")
                .AsQueryable();

            // Tìm kiếm theo tên / mô tả
            if (!string.IsNullOrWhiteSpace(keyword))
            {
                keyword = keyword.Trim().ToLower();
                query = query.Where(sp =>
                    sp.TenSanPham.ToLower().Contains(keyword) ||
                    sp.MoTa.ToLower().Contains(keyword));
            }

            // Lọc theo danh mục kiểu text ở sidebar
            if (!string.IsNullOrWhiteSpace(loai))
            {
                loai = loai.Trim().ToLower();

                query = query.Where(sp =>
                    sp.LoaiSanPham != null &&
                    sp.LoaiSanPham.TenLoai.ToLower().Contains(loai));
            }

            // Lọc theo giá
            if (!string.IsNullOrWhiteSpace(gia))
            {
                switch (gia)
                {
                    case "duoi100":
                        query = query.Where(sp => sp.DonGia < 100000);
                        break;

                    case "100-300":
                        query = query.Where(sp => sp.DonGia >= 100000 && sp.DonGia <= 300000);
                        break;

                    case "300-500":
                        query = query.Where(sp => sp.DonGia > 300000 && sp.DonGia <= 500000);
                        break;

                    case "tren500":
                        query = query.Where(sp => sp.DonGia > 500000);
                        break;
                }
            }

            // Sắp xếp nếu muốn dùng sau này
            switch (sort)
            {
                case "price_asc":
                    query = query.OrderBy(sp => sp.DonGia);
                    break;

                case "price_desc":
                    query = query.OrderByDescending(sp => sp.DonGia);
                    break;

                case "name_asc":
                    query = query.OrderBy(sp => sp.TenSanPham);
                    break;

                case "name_desc":
                    query = query.OrderByDescending(sp => sp.TenSanPham);
                    break;

                default:
                    query = query.OrderByDescending(sp => sp.MaSanPham);
                    break;
            }

            int tongSoSanPham = query.Count();
            int tongSoTrang = (int)Math.Ceiling((double)tongSoSanPham / soSanPhamMoiTrang);

            if (tongSoTrang == 0) tongSoTrang = 1;
            page = Math.Max(1, Math.Min(page, tongSoTrang));

            var dsSanPham = query
                .Skip((page - 1) * soSanPhamMoiTrang)
                .Take(soSanPhamMoiTrang)
                .ToList();

            ViewBag.Keyword = keyword;
            ViewBag.LoaiDangChon = loai;
            ViewBag.GiaDangChon = gia;
            ViewBag.SortDangChon = sort;
            ViewBag.TrangHienTai = page;
            ViewBag.TongSoTrang = tongSoTrang;

            return View(dsSanPham);
        }
    }
}