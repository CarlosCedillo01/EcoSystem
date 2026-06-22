using EcoSystem.Business.Common;
using EcoSystem.Business.Dtos.Clientes;

namespace EcoSystem.Business.Interfaces;

public interface IClienteService
{
    Task<IReadOnlyList<ClienteReadDto>> GetAllAsync(CancellationToken cancellationToken = default);

    Task<ServiceResult<ClienteReadDto>> GetByIdAsync(int id, CancellationToken cancellationToken = default);

    Task<ServiceResult<ClienteReadDto>> CreateAsync(ClienteCreateDto dto, CancellationToken cancellationToken = default);

    Task<ServiceResult<ClienteReadDto>> UpdateAsync(int id, ClienteUpdateDto dto, CancellationToken cancellationToken = default);

    Task<ServiceResult<bool>> DeleteAsync(int id, CancellationToken cancellationToken = default);
}
