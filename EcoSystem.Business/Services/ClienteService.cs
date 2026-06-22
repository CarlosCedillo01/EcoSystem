using System.Net.Mail;
using EcoSystem.Business.Common;
using EcoSystem.Business.Dtos.Clientes;
using EcoSystem.Business.Interfaces;
using EcoSystem.Data.Context;
using EcoSystem.Data.Entities;
using Microsoft.EntityFrameworkCore;

namespace EcoSystem.Business.Services;

public sealed class ClienteService(EcoSystemDbContext context) : IClienteService
{
    public async Task<IReadOnlyList<ClienteReadDto>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        return await context.Clientes
            .AsNoTracking()
            .OrderBy(c => c.Nombre)
            .Select(c => new ClienteReadDto(c.IdCliente, c.Nombre, c.Email, c.Ciudad, c.FechaRegistro))
            .ToListAsync(cancellationToken);
    }

    public async Task<ServiceResult<ClienteReadDto>> GetByIdAsync(int id, CancellationToken cancellationToken = default)
    {
        var cliente = await context.Clientes
            .AsNoTracking()
            .FirstOrDefaultAsync(c => c.IdCliente == id, cancellationToken);

        return cliente is null
            ? ServiceResult<ClienteReadDto>.Failure(404, "El cliente no existe.")
            : ServiceResult<ClienteReadDto>.Success(Map(cliente), "Cliente encontrado.");
    }

    public async Task<ServiceResult<ClienteReadDto>> CreateAsync(ClienteCreateDto dto, CancellationToken cancellationToken = default)
    {
        var errors = Validate(dto);
        if (errors.Count > 0)
        {
            return ServiceResult<ClienteReadDto>.Failure(400, "No se pudo crear el cliente.", errors.ToArray());
        }

        var cliente = new Cliente
        {
            Nombre = dto.Nombre.Trim(),
            Email = string.IsNullOrWhiteSpace(dto.Email) ? null : dto.Email.Trim(),
            Ciudad = string.IsNullOrWhiteSpace(dto.Ciudad) ? null : dto.Ciudad.Trim(),
            FechaRegistro = dto.FechaRegistro ?? DateOnly.FromDateTime(DateTime.UtcNow)
        };

        context.Clientes.Add(cliente);
        await context.SaveChangesAsync(cancellationToken);

        return ServiceResult<ClienteReadDto>.Created(Map(cliente), "Cliente creado correctamente.");
    }

    public async Task<ServiceResult<ClienteReadDto>> UpdateAsync(int id, ClienteUpdateDto dto, CancellationToken cancellationToken = default)
    {
        var errors = Validate(dto);
        if (errors.Count > 0)
        {
            return ServiceResult<ClienteReadDto>.Failure(400, "No se pudo actualizar el cliente.", errors.ToArray());
        }

        var cliente = await context.Clientes.FirstOrDefaultAsync(c => c.IdCliente == id, cancellationToken);
        if (cliente is null)
        {
            return ServiceResult<ClienteReadDto>.Failure(404, "El cliente no existe.");
        }

        cliente.Nombre = dto.Nombre.Trim();
        cliente.Email = string.IsNullOrWhiteSpace(dto.Email) ? null : dto.Email.Trim();
        cliente.Ciudad = string.IsNullOrWhiteSpace(dto.Ciudad) ? null : dto.Ciudad.Trim();
        cliente.FechaRegistro = dto.FechaRegistro ?? cliente.FechaRegistro;

        await context.SaveChangesAsync(cancellationToken);
        return ServiceResult<ClienteReadDto>.Success(Map(cliente), "Cliente actualizado correctamente.");
    }

    public async Task<ServiceResult<bool>> DeleteAsync(int id, CancellationToken cancellationToken = default)
    {
        var cliente = await context.Clientes.FirstOrDefaultAsync(c => c.IdCliente == id, cancellationToken);
        if (cliente is null)
        {
            return ServiceResult<bool>.Failure(404, "El cliente no existe.");
        }

        context.Clientes.Remove(cliente);
        await context.SaveChangesAsync(cancellationToken);
        return ServiceResult<bool>.NoContent("Cliente eliminado correctamente.");
    }

    private static ClienteReadDto Map(Cliente cliente)
        => new(cliente.IdCliente, cliente.Nombre, cliente.Email, cliente.Ciudad, cliente.FechaRegistro);

    private static List<string> Validate(ClienteCreateDto dto)
        => Validate(dto.Nombre, dto.Email, dto.Ciudad);

    private static List<string> Validate(ClienteUpdateDto dto)
        => Validate(dto.Nombre, dto.Email, dto.Ciudad);

    private static List<string> Validate(string nombre, string? email, string? ciudad)
    {
        var errors = new List<string>();

        if (string.IsNullOrWhiteSpace(nombre))
        {
            errors.Add("El nombre del cliente es obligatorio.");
        }
        else if (nombre.Trim().Length > 100)
        {
            errors.Add("El nombre del cliente no puede superar 100 caracteres.");
        }

        if (!string.IsNullOrWhiteSpace(email))
        {
            try
            {
                _ = new MailAddress(email.Trim());
            }
            catch
            {
                errors.Add("El correo electrónico no tiene un formato válido.");
            }
        }

        if (!string.IsNullOrWhiteSpace(ciudad) && ciudad.Trim().Length > 60)
        {
            errors.Add("La ciudad no puede superar 60 caracteres.");
        }

        return errors;
    }
}
