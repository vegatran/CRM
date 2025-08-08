using Application.Interfaces;
using Domain.Entities;
using Domain.Interfaces;

namespace Application.Services
{
    public class PhieuXuatKhoService : IPhieuXuatKhoService
    {
        private readonly IUnitOfWork _unitOfWork;

        public PhieuXuatKhoService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<PhieuXuatKho?> GetByIdAsync(int id)
        {
            return await _unitOfWork.Repository<PhieuXuatKho>().GetByIdAsync(id);
        }

        public async Task<IEnumerable<PhieuXuatKho>> GetAllAsync()
        {
            return await _unitOfWork.Repository<PhieuXuatKho>().GetAllAsync();
        }

        public async Task<PhieuXuatKho> CreateAsync(PhieuXuatKho phieuXuatKho)
        {
            var result = await _unitOfWork.Repository<PhieuXuatKho>().AddAsync(phieuXuatKho);
            await _unitOfWork.SaveChangesAsync();
            return result;
        }

        public async Task UpdateAsync(PhieuXuatKho phieuXuatKho)
        {
            await _unitOfWork.Repository<PhieuXuatKho>().UpdateAsync(phieuXuatKho);
            await _unitOfWork.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var phieuXuatKho = await GetByIdAsync(id);
            if (phieuXuatKho != null)
            {
                await _unitOfWork.Repository<PhieuXuatKho>().DeleteAsync(phieuXuatKho);
                await _unitOfWork.SaveChangesAsync();
            }
        }

        public async Task<bool> ExistsAsync(int id)
        {
            return await _unitOfWork.Repository<PhieuXuatKho>().ExistsAsync(id);
        }
    }
}
