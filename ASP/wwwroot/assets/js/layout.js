document.addEventListener("DOMContentLoaded", function () {
    console.log("Menu JS khởi động!");

    setActiveMenu();      // Highlight trang hiện tại
    initDesktopMenu();    // Chỉ xử lý mega menu Sản phẩm trên desktop
    initMobileMenu();     // Menu mobile riêng biệt, luôn hoạt động
});

// ===== HIGHLIGHT MENU HIỆN TẠI =====
function setActiveMenu() {
    let currentPath = window.location.pathname.toLowerCase();
    if (currentPath === "/" || currentPath === "" || currentPath.endsWith("/home") || currentPath.endsWith("/home/index")) {
        currentPath = "/";
    }

    // Xóa active cũ
    document.querySelectorAll(".nav-item, .nav-item-link").forEach(el => el.classList.remove("active"));

    // Active menu đơn cấp
    document.querySelectorAll(".nav-item-link").forEach(link => {
        let href = link.getAttribute("href") || "";
        href = href.toLowerCase();
        if (href === currentPath || (currentPath.startsWith(href + "/") && href !== "/")) {
            link.classList.add("active");
            link.closest(".nav-item")?.classList.add("active");
        }
    });

    // Riêng Sản phẩm: active khi đang ở trang con
    if (currentPath.includes("/product") || currentPath.includes("/san-pham")) {
        const productItem = document.querySelector(".nav-item.has-mega");
        const productLink = document.querySelector('.nav-item-link[href*="Product"], .nav-item-link[href*="product"]');
        if (productItem) productItem.classList.add("active");
        if (productLink) productLink.classList.add("active");
    }
}

// ===== DESKTOP: SẢN PHẨM CHỈ MỞ KHI CLICK (KHÔNG HOVER) =====
function initDesktopMenu() {
    if (window.innerWidth <= 767) return; // Không chạy trên mobile

    const productItem = document.querySelector(".nav-item.has-mega");
    const productLink = productItem?.querySelector(".nav-item-link");
    const megaMenu = document.querySelector(".mega-menu");

    if (!productItem || !productLink || !megaMenu) return;

    productLink.addEventListener("click", function (e) {
        e.preventDefault(); // Chặn nhảy trang trên desktop
        productItem.classList.toggle("active");
        megaMenu.classList.toggle("is-active");
    });

    // Click ngoài đóng menu
    document.addEventListener("click", function (e) {
        if (!productItem.contains(e.target)) {
            productItem.classList.remove("active");
            megaMenu.classList.remove("is-active");
        }
    });
}

// ===== MOBILE MENU: HAMBURGER MỞ/ĐÓNG (GIỮ NGUYÊN HÀNH VI CŨ, KHÔNG BỊ ẢNH HƯỞNG) =====
function initMobileMenu() {
    const menuToggle = document.querySelector(".menu-toggle");
    const navList = document.querySelector(".nav-list");
    const nav = document.querySelector(".nav");
    const body = document.body;

    if (!menuToggle || !navList || !nav) {
        console.warn("Không tìm thấy element mobile menu!");
        return;
    }

    console.log("Mobile menu sẵn sàng!");

    function openMenu() {
        navList.classList.add("active");
        nav.classList.add("active");
        body.style.overflow = "hidden";
    }

    function closeMenu() {
        navList.classList.remove("active");
        nav.classList.remove("active");
        body.style.overflow = "";
        // Đóng hết submenu
        document.querySelectorAll(".nav-item.active").forEach(item => item.classList.remove("active"));
        document.querySelector(".mega-menu")?.classList.remove("is-active");
    }

    // Click hamburger
    menuToggle.addEventListener("click", function (e) {
        e.stopPropagation();
        if (navList.classList.contains("active")) {
            closeMenu();
        } else {
            openMenu();
        }
    });

    // Click overlay đóng
    nav.addEventListener("click", function (e) {
        if (e.target === nav) closeMenu();
    });

    // Submenu accordion trên mobile (nếu có)
    document.querySelectorAll(".nav-item > .nav-item-link").forEach(link => {
        link.addEventListener("click", function (e) {
            if (window.innerWidth <= 767) {
                const parent = this.parentElement;
                if (parent.classList.contains("has-mega") || parent.querySelector(".nav-list-child")) {
                    e.preventDefault();
                    parent.classList.toggle("active");
                }
            }
        });
    });

    // Resize sang desktop thì đóng
    window.addEventListener("resize", () => {
        if (window.innerWidth > 767) closeMenu();
    });
}