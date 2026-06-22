using EcoSystem.Business.Common;
using EcoSystem.Business.Dtos.Productos;

namespace EcoSystem.Business.Interfaces;

public interface IProductoService
{
    Task<IReadOnlyList<ProductoReadDto>> GetAllAsync(CancellationToken cancellationToken = default);

    Task<ServiceResult<ProductoReadDto>> GetByIdAsync(int id, CancellationToken cancellationToken = default);

    Task<ServiceResult<ProductoReadDto>> CreateAsync(ProductoCreateDto dto, CancellationToken cancellationToken = default);

    Task<ServiceResult<ProductoReadDto>> UpdateAsync(int id, ProductoUpdateDto dto, CancellationToken cancellationToken = default);

    Task<ServiceResult<bool>> DeleteAsync(int id, CancellationToken cancellationToken = default);
}
