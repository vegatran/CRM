using System.ComponentModel.DataAnnotations;

namespace Domain.Entities
{
    public enum LoaiSanPham
    {
        TuSanXuat = 1,    // Tự sản xuất
        MuaNgoai = 2      // Mua ngoài
    }

    public class SanPham : BaseEntity
    {
        [Required]
        [StringLength(100)]
        public string TenSanPham { get; set; } = string.Empty;
        
        [StringLength(500)]
        public string? MoTa { get; set; }
        
        [Required]
        [StringLength(50)]
        public string MaSanPham { get; set; } = string.Empty;
        
        public decimal GiaBan { get; set; }
        
        public decimal GiaNhap { get; set; }
        
        public decimal ChiPhiNhanCong { get; set; } = 0;
        
        public int SoLuongTon { get; set; } = 0;
        
        [StringLength(20)]
        public string? KichThuoc { get; set; }
        
        [StringLength(50)]
        public string? MauSac { get; set; }
        
        [StringLength(100)]
        public string? ChatLieu { get; set; }
        
        [StringLength(500)]
        public string? HinhAnh { get; set; }
        
        public bool TrangThai { get; set; } = true;
        
        // Loại sản phẩm: Tự sản xuất hoặc Mua ngoài
        public LoaiSanPham LoaiSanPham { get; set; } = LoaiSanPham.TuSanXuat;
        
        // Navigation properties
        public virtual ICollection<ThanhPhanCauHinh> ThanhPhanCauHinhs { get; set; } = new List<ThanhPhanCauHinh>();
        public virtual ICollection<ChiTietBanHang> ChiTietBanHangs { get; set; } = new List<ChiTietBanHang>();
        public virtual ICollection<ChiTietNhapKho> ChiTietNhapKhos { get; set; } = new List<ChiTietNhapKho>();
        public virtual ICollection<QuyTrinhSanXuat> QuyTrinhSanXuats { get; set; } = new List<QuyTrinhSanXuat>();
        public virtual ICollection<DinhMucNguyenLieu> DinhMucNguyenLieus { get; set; } = new List<DinhMucNguyenLieu>();
        
        // Computed property - Tổng chi phí nhân công từ quy trình
        public decimal TongChiPhiNhanCong 
        {
            get
            {
                if (QuyTrinhSanXuats == null) return 0;
                return QuyTrinhSanXuats.Where(q => q.TrangThai).Sum(q => q.ChiPhiNhanCong);
            }
        }
        
        // Computed property - Tổng chi phí nguyên liệu
        public decimal TongChiPhiNguyenLieu 
        {
            get
            {
                if (DinhMucNguyenLieus == null) return 0;
                return DinhMucNguyenLieus.Where(d => d.TrangThai && d.NguyenLieu != null)
                    .Sum(d => d.SoLuongCan * d.NguyenLieu.GiaNhap);
            }
        }
        
        // Computed property - Tổng chi phí sản xuất (nguyên liệu + nhân công)
        public decimal TongChiPhiSanXuat => TongChiPhiNguyenLieu + TongChiPhiNhanCong;
        
        // Computed property - Giá vốn (tùy theo loại sản phẩm)
        public decimal GiaVon
        {
            get
            {
                return LoaiSanPham == LoaiSanPham.TuSanXuat 
                    ? TongChiPhiSanXuat  // Tự sản xuất: lấy chi phí sản xuất
                    : GiaNhap;           // Mua ngoài: lấy giá nhập
            }
        }
        
        // Computed property - Giá bán đề xuất (dựa trên hệ số lợi nhuận)
        public decimal GiaBanDeXuat
        {
            get
            {
                const decimal HeSoLoiNhuan = 1.3m; // 30% lợi nhuận
                return GiaVon * HeSoLoiNhuan;
            }
        }
        
        // Computed property - Phần trăm lợi nhuận
        public decimal PhanTramLoiNhuan
        {
            get
            {
                if (GiaBan <= 0) return 0;
                return ((GiaBan - GiaVon) / GiaBan) * 100;
            }
        }
        
        // Computed property - Số lượng sản phẩm có thể sản xuất được
        public int SoLuongCoTheSanXuat
        {
            get
            {
                if (LoaiSanPham != LoaiSanPham.TuSanXuat || DinhMucNguyenLieus == null || !DinhMucNguyenLieus.Any())
                    return 0;
                
                var soLuongCoTheSanXuat = int.MaxValue;
                
                foreach (var dinhMuc in DinhMucNguyenLieus.Where(d => d.TrangThai && d.NguyenLieu != null))
                {
                    if (dinhMuc.SoLuongCan <= 0) continue;
                    
                    // Tính số lượng sản phẩm có thể sản xuất từ nguyên liệu này
                    // Sử dụng SoLuongTonThucTe thay vì SoLuongTon
                    var soLuongTonThucTe = dinhMuc.NguyenLieu.SoLuongTonThucTe;
                    var soLuongTuNguyenLieu = (int)(soLuongTonThucTe / dinhMuc.SoLuongCan);
                    
                    // Lấy số lượng nhỏ nhất (nguyên liệu nào thiếu nhất sẽ quyết định)
                    if (soLuongTuNguyenLieu < soLuongCoTheSanXuat)
                    {
                        soLuongCoTheSanXuat = soLuongTuNguyenLieu;
                    }
                }
                
                return soLuongCoTheSanXuat == int.MaxValue ? 0 : soLuongCoTheSanXuat;
            }
        }
        
        // Computed property - Danh sách nguyên liệu thiếu
        public List<string> DanhSachNguyenLieuThieu
        {
            get
            {
                var danhSachThieu = new List<string>();
                
                if (LoaiSanPham != LoaiSanPham.TuSanXuat || DinhMucNguyenLieus == null)
                    return danhSachThieu;
                
                foreach (var dinhMuc in DinhMucNguyenLieus.Where(d => d.TrangThai && d.NguyenLieu != null))
                {
                    if (dinhMuc.SoLuongCan <= 0) continue;
                    
                    var soLuongCan = dinhMuc.SoLuongCan;
                    // Sử dụng SoLuongTonThucTe thay vì SoLuongTon
                    var soLuongTonThucTe = dinhMuc.NguyenLieu.SoLuongTonThucTe;
                    
                    if (soLuongTonThucTe < soLuongCan)
                    {
                        var soLuongThieu = soLuongCan - soLuongTonThucTe;
                        danhSachThieu.Add($"{dinhMuc.NguyenLieu.TenNguyenLieu}: thiếu {soLuongThieu:N2} {dinhMuc.DonViTinh ?? "đơn vị"}");
                    }
                }
                
                return danhSachThieu;
            }
        }
    }
} 