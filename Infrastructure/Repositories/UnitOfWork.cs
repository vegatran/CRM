using Domain.Entities;
using Domain.Interfaces;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore.Storage;

namespace Infrastructure.Repositories
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly ApplicationDbContext _context;
        private readonly Dictionary<Type, object> _repositories;
        private IDbContextTransaction? _transaction;

        public UnitOfWork(ApplicationDbContext context)
        {
            _context = context;
            _repositories = new Dictionary<Type, object>();
        }

        public IRepository<T> Repository<T>() where T : class
        {
            var type = typeof(T);
            if (!_repositories.ContainsKey(type))
            {
                _repositories[type] = new Repository<T>(_context);
            }
            return (IRepository<T>)_repositories[type];
        }

        public IRepository<SanPham> SanPhamRepository => Repository<SanPham>();
        public IRepository<NhaCungCap> NhaCungCapRepository => Repository<NhaCungCap>();
        public IRepository<NguyenLieu> NguyenLieuRepository => Repository<NguyenLieu>();
        public IRepository<LoaiNguyenLieu> LoaiNguyenLieuRepository => Repository<LoaiNguyenLieu>();
        public IRepository<QuyTrinhSanXuat> QuyTrinhSanXuatRepository => Repository<QuyTrinhSanXuat>();
        public IRepository<DinhMucNguyenLieu> DinhMucNguyenLieuRepository => Repository<DinhMucNguyenLieu>();
        public IRepository<KhachHang> KhachHangRepository => Repository<KhachHang>();
        public IRepository<PhieuXuatKho> PhieuXuatKhoRepository => Repository<PhieuXuatKho>();
        public object Context => _context;

        public async Task<int> SaveChangesAsync()
        {
            return await _context.SaveChangesAsync();
        }

        public async Task BeginTransactionAsync()
        {
            _transaction = await _context.Database.BeginTransactionAsync();
        }

        public async Task CommitTransactionAsync()
        {
            if (_transaction != null)
            {
                await _transaction.CommitAsync();
                await _transaction.DisposeAsync();
                _transaction = null;
            }
        }

        public async Task RollbackTransactionAsync()
        {
            if (_transaction != null)
            {
                await _transaction.RollbackAsync();
                await _transaction.DisposeAsync();
                _transaction = null;
            }
        }

        public void Dispose()
        {
            _transaction?.Dispose();
            _context.Dispose();
        }
    }
} 