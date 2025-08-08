using Application.Interfaces;
using Domain.Entities;
using Domain.Interfaces;

namespace Application.Services
{
    public class KhachHangService : IKhachHangService
    {
        private readonly IUnitOfWork _unitOfWork;

        public KhachHangService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<KhachHang?> GetByIdAsync(int id)
        {
            return await _unitOfWork.Repository<KhachHang>().GetByIdAsync(id);
        }

        public async Task<IEnumerable<KhachHang>> GetAllAsync()
        {
            return await _unitOfWork.Repository<KhachHang>().GetAllAsync();
        }

        public async Task<KhachHang> CreateAsync(KhachHang khachHang)
        {
            var result = await _unitOfWork.Repository<KhachHang>().AddAsync(khachHang);
            await _unitOfWork.SaveChangesAsync();
            return result;
        }

        public async Task UpdateAsync(KhachHang khachHang)
        {
            await _unitOfWork.Repository<KhachHang>().UpdateAsync(khachHang);
            await _unitOfWork.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var khachHang = await GetByIdAsync(id);
            if (khachHang != null)
            {
                await _unitOfWork.Repository<KhachHang>().DeleteAsync(khachHang);
                await _unitOfWork.SaveChangesAsync();
            }
        }

        public async Task<bool> ExistsAsync(int id)
        {
            return await _unitOfWork.Repository<KhachHang>().ExistsAsync(id);
        }
    }
}
