# EcoSystem

EcoSystem es una API REST en ASP.NET Core con arquitectura en capas:

- `EcoSystem.API` expone los endpoints y Swagger UI.
- `EcoSystem.Business` contiene la lógica de negocio, validaciones y contratos.
- `EcoSystem.Data` concentra el contexto de Entity Framework Core, entidades y configuraciones de persistencia.

## Requisitos

- .NET 10 SDK
- Una base de datos PostgreSQL en la nube, como Supabase o Azure SQL

## Configuración

Define la cadena de conexión en `EcoSystem.API/appsettings.json` o como variable de entorno:

```text
ConnectionStrings__DefaultConnection
```

Si prefieres usar entorno local, coloca la cadena de conexión real en ese valor antes de ejecutar la API.

## Ejecutar

```bash
dotnet restore
dotnet build
dotnet run --project EcoSystem.API/EcoSystem.API.csproj
```

Swagger UI queda disponible en:

```text
/swagger
```

## Endpoints

- `GET /api/categorias`
- `GET /api/categorias/{id}`
- `POST /api/categorias`
- `PUT /api/categorias/{id}`
- `DELETE /api/categorias/{id}`
- `GET /api/productos`
- `GET /api/productos/{id}`
- `POST /api/productos`
- `PUT /api/productos/{id}`
- `DELETE /api/productos/{id}`
- `GET /api/clientes`
- `GET /api/clientes/{id}`
- `POST /api/clientes`
- `PUT /api/clientes/{id}`
- `DELETE /api/clientes/{id}`
- `GET /api/ordenes`
- `GET /api/ordenes/{id}`
- `POST /api/ordenes`
- `PUT /api/ordenes/{id}`
- `DELETE /api/ordenes/{id}`

## Notas

- La solución principal es `EcoSystem.sln`.
- La capa API no accede directamente a `EcoSystem.Data`.
- Las respuestas incluyen mensajes claros y códigos HTTP adecuados.
