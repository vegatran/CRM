using Domain.Entities;

namespace Domain.Interfaces
{
    public interface IUnitOfWork : IDisposable
    {
        IRepository<T> Repository<T>() where T : class;
        IRepository<SanPham> SanPhamRepository { get; }
        IRepository<NhaCungCap> NhaCungCapRepository { get; }
        IRepository<NguyenLieu> NguyenLieuRepository { get; }
        IRepository<LoaiNguyenLieu> LoaiNguyenLieuRepository { get; }
        IRepository<QuyTrinhSanXuat> QuyTrinhSanXuatRepository { get; }
        IRepository<DinhMucNguyenLieu> DinhMucNguyenLieuRepository { get; }
        object Context { get; }
        Task<int> SaveChangesAsync();
        Task BeginTransactionAsync();
        Task CommitTransactionAsync();
        Task RollbackTransactionAsync();
    }
} 