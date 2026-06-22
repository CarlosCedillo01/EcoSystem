using EcoSystem.Business.Common;
using EcoSystem.Business.Dtos.Ordenes;

namespace EcoSystem.Business.Interfaces;

public interface IOrdenService
{
    Task<IReadOnlyList<OrdenReadDto>> GetAllAsync(CancellationToken cancellationToken = default);

    Task<ServiceResult<OrdenReadDto>> GetByIdAsync(int id, CancellationToken cancellationToken = default);

    Task<ServiceResult<OrdenReadDto>> CreateAsync(OrdenCreateDto dto, CancellationToken cancellationToken = default);

    Task<ServiceResult<OrdenReadDto>> UpdateAsync(int id, OrdenUpdateDto dto, CancellationToken cancellationToken = default);

    Task<ServiceResult<bool>> DeleteAsync(int id, CancellationToken cancellationToken = default);
}
