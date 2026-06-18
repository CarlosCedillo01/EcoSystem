# EcoSystem 🚀

¡Bienvenido al proyecto **EcoSystem**! Este es un proyecto de software moderno desarrollado en **.NET 10.0** utilizando una arquitectura limpia y modular dividida en capas.

El proyecto está estructurado utilizando el nuevo formato de soluciones de Visual Studio (`.slnx`), lo que permite una configuración más ligera y fácil de mantener.

---

## 📁 Estructura del Proyecto

El sistema se compone de los siguientes subproyectos:

1. **[EcoSystem.API](file:///c:/Users/medic/OneDrive/Desktop/EcoSystem/EcoSystem.API)**:
   - Capa de presentación y servicios web basada en **ASP.NET Core Web API**.
   - Configurado con soporte nativo para **OpenAPI (Swagger)** en .NET 10 para documentar los endpoints expuestos de manera automática.
   - Contiene la configuración principal del pipeline de la aplicación, inyección de dependencias y middlewares en [Program.cs](file:///c:/Users/medic/OneDrive/Desktop/EcoSystem/EcoSystem.API/Program.cs).

2. **[EcoSystem.Data](file:///c:/Users/medic/OneDrive/Desktop/EcoSystem/EcoSystem.Data)**:
   - Capa de acceso a datos y lógica del dominio en formato de biblioteca de clases (.NET Class Library).
   - Diseñado para alojar el contexto de la base de datos (por ejemplo, Entity Framework Core), entidades del sistema, configuraciones de persistencia y repositorios.

---

## 🛠️ Tecnologías Utilizadas

* **C# 14**
* **.NET 10.0 SDK**
* **ASP.NET Core Minimal APIs**
* **Microsoft.AspNetCore.OpenApi** para la generación de especificaciones OpenAPI (v1)

---

## 🚀 Requisitos Previos

Antes de ejecutar el proyecto, asegúrate de tener instalado:

* **.NET 10.0 SDK** o superior (puedes verificarlo con `dotnet --version`).
* Un editor de código o IDE de tu preferencia:
  * **Visual Studio 2022** (con soporte para .NET 10 y formato `.slnx`).
  * **Visual Studio Code** con la extensión *C# Dev Kit*.
  * **JetBrains Rider**.

---

## ⚙️ Cómo Empezar

Sigue estos pasos para compilar y ejecutar el proyecto localmente:

### 1. Clonar o descargar el repositorio
Abre tu terminal en la carpeta del proyecto.

### 2. Restaurar dependencias
Restaura los paquetes NuGet del proyecto ejecutando:
```bash
dotnet restore
```

### 3. Compilar la solución
Compila el proyecto completo para verificar que todo esté en orden:
```bash
dotnet build
```

### 4. Ejecutar la API
Inicia la API web de desarrollo ejecutando:
```bash
dotnet run --project EcoSystem.API/EcoSystem.API.csproj
```
O también puedes iniciar en modo de escucha y recarga en caliente (*Hot Reload*) con:
```bash
dotnet watch --project EcoSystem.API/EcoSystem.API.csproj
```

Por defecto, la API estará disponible en las direcciones configuradas en los perfiles de lanzamiento, típicamente:
* **HTTPS**: `https://localhost:7196` o similar (revisar la salida de consola).

---

## 📄 Documentación de la API

La API viene configurada por defecto con soporte OpenAPI. Cuando ejecutes el proyecto en modo de desarrollo (`Development`), puedes acceder a la especificación OpenAPI en:

* **Endpoint de OpenAPI**: `https://localhost:<puerto>/openapi/v1.json`

---

## 🗺️ Roadmap / Siguientes Pasos

- [ ] Integrar un ORM como **Entity Framework Core** en la capa `EcoSystem.Data`.
- [ ] Configurar la cadena de conexión en `appsettings.json`.
- [ ] Crear las entidades del dominio y realizar la primera migración de base de datos.
- [ ] Implementar controladores o endpoints estructurados (endpoints con REPR o patrón Mediador).
- [ ] Agregar validación de datos e inyección de dependencias avanzada.
