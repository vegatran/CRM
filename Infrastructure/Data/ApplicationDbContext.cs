using Microsoft.EntityFrameworkCore;
using Domain.Entities;

namespace Infrastructure.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
        }

        public DbSet<SanPham> SanPhams { get; set; }
        public DbSet<NhaCungCap> NhaCungCaps { get; set; }
        public DbSet<NguyenLieu> NguyenLieus { get; set; }
        public DbSet<LoaiNguyenLieu> LoaiNguyenLieus { get; set; }
        public DbSet<KhachHang> KhachHangs { get; set; }
        public DbSet<KhuVuc> KhuVucs { get; set; }
        public DbSet<PhieuNhapKho> PhieuNhapKhos { get; set; }
        public DbSet<ChiTietNhapKho> ChiTietNhapKhos { get; set; }
        public DbSet<ThanhPhanCauHinh> ThanhPhanCauHinhs { get; set; }
        public DbSet<PhieuBanHang> PhieuBanHangs { get; set; }
        public DbSet<ChiTietBanHang> ChiTietBanHangs { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Configure relationships
            modelBuilder.Entity<NguyenLieu>()
                .HasOne(nl => nl.NhaCungCap)
                .WithMany(ncc => ncc.NguyenLieus)
                .HasForeignKey(nl => nl.NhaCungCapId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<NguyenLieu>()
                .HasOne(nl => nl.LoaiNguyenLieu)
                .WithMany(lnl => lnl.NguyenLieus)
                .HasForeignKey(nl => nl.LoaiNguyenLieuId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<KhachHang>()
                .HasOne(kh => kh.KhuVuc)
                .WithMany(kv => kv.KhachHangs)
                .HasForeignKey(kh => kh.KhuVucId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<PhieuNhapKho>()
                .HasOne(pnk => pnk.NhaCungCap)
                .WithMany(ncc => ncc.PhieuNhapKhos)
                .HasForeignKey(pnk => pnk.NhaCungCapId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<ChiTietNhapKho>()
                .HasOne(ctnk => ctnk.PhieuNhapKho)
                .WithMany(pnk => pnk.ChiTietNhapKhos)
                .HasForeignKey(ctnk => ctnk.PhieuNhapKhoId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<ChiTietNhapKho>()
                .HasOne(ctnk => ctnk.SanPham)
                .WithMany(sp => sp.ChiTietNhapKhos)
                .HasForeignKey(ctnk => ctnk.SanPhamId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<ChiTietNhapKho>()
                .HasOne(ctnk => ctnk.NguyenLieu)
                .WithMany(nl => nl.ChiTietNhapKhos)
                .HasForeignKey(ctnk => ctnk.NguyenLieuId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<ThanhPhanCauHinh>()
                .HasOne(tpch => tpch.SanPham)
                .WithMany(sp => sp.ThanhPhanCauHinhs)
                .HasForeignKey(tpch => tpch.SanPhamId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<ThanhPhanCauHinh>()
                .HasOne(tpch => tpch.NguyenLieu)
                .WithMany(nl => nl.ThanhPhanCauHinhs)
                .HasForeignKey(tpch => tpch.NguyenLieuId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<PhieuBanHang>()
                .HasOne(pbh => pbh.KhachHang)
                .WithMany(kh => kh.PhieuBanHangs)
                .HasForeignKey(pbh => pbh.KhachHangId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<ChiTietBanHang>()
                .HasOne(ctbh => ctbh.PhieuBanHang)
                .WithMany(pbh => pbh.ChiTietBanHangs)
                .HasForeignKey(ctbh => ctbh.PhieuBanHangId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<ChiTietBanHang>()
                .HasOne(ctbh => ctbh.SanPham)
                .WithMany(sp => sp.ChiTietBanHangs)
                .HasForeignKey(ctbh => ctbh.SanPhamId)
                .OnDelete(DeleteBehavior.Restrict);

            // Configure indexes
            modelBuilder.Entity<SanPham>()
                .HasIndex(sp => sp.MaSanPham)
                .IsUnique();

            modelBuilder.Entity<NguyenLieu>()
                .HasIndex(nl => nl.MaNguyenLieu)
                .IsUnique();

            modelBuilder.Entity<KhachHang>()
                .HasIndex(kh => kh.MaKhachHang)
                .IsUnique();

            modelBuilder.Entity<PhieuNhapKho>()
                .HasIndex(pnk => pnk.SoPhieu)
                .IsUnique();

            modelBuilder.Entity<PhieuBanHang>()
                .HasIndex(pbh => pbh.SoPhieu)
                .IsUnique();
        }
    }
} 