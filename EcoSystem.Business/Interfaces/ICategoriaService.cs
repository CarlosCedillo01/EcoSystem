using EcoSystem.Business.Common;
using EcoSystem.Business.Dtos.Categorias;

namespace EcoSystem.Business.Interfaces;

public interface ICategoriaService
{
    Task<IReadOnlyList<CategoriaReadDto>> GetAllAsync(CancellationToken cancellationToken = default);

    Task<ServiceResult<CategoriaReadDto>> GetByIdAsync(int id, CancellationToken cancellationToken = default);

    Task<ServiceResult<CategoriaReadDto>> CreateAsync(CategoriaCreateDto dto, CancellationToken cancellationToken = default);

    Task<ServiceResult<CategoriaReadDto>> UpdateAsync(int id, CategoriaUpdateDto dto, CancellationToken cancellationToken = default);

    Task<ServiceResult<bool>> DeleteAsync(int id, CancellationToken cancellationToken = default);
}
