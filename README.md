# ABM de Usuarios y Personas - Blazor (.NET 8)

> Aplicación web desarrollada con **Blazor Server** y **.NET 8**, que permite gestionar usuarios y personas mediante operaciones de Alta, Baja y Modificación (ABM).

---

## Funcionalidades

- Alta, edición y eliminación de **usuarios**.
- Alta, edición y eliminación de **personas**.
- **Validaciones** en formularios para garantizar datos consistentes.
- **Navegación dinámica** utilizando componentes de Blazor.

---

## Tecnologías Utilizadas

- [.NET 8](https://dotnet.microsoft.com/en-us/download/dotnet/8.0)
- [Blazor Server](https://learn.microsoft.com/en-us/aspnet/core/blazor/)
- [SQL Server](https://www.microsoft.com/en-us/sql-server)
- [Bootstrap 5](https://getbootstrap.com/docs/5.0/getting-started/introduction/)

---

## Cómo Ejecutar el Proyecto

### 1. Clonar el repositorio

```bash
git clone https://github.com/MicaCaceres/ABM-Blazor.git
cd ABM-Blazor```

### 2. Crear la base de datos
Ejecutá el script creacion_base_locate.sql que se encuentra en el repositorio.

Este script genera la base de datos local y agrega datos de ejemplo utilizados por la aplicación.

> ⚠️ Si necesitás modificar los datos iniciales, recordá actualizar tanto el script SQL como el código del proyecto si corresponde.

### 3. Configurar la cadena de conexión
Abrí el archivo appsettings.json.

Modificá el valor de "CadenaConexion" con los datos de tu servidor:
"ConnectionStrings": {
  "CadenaConexion": "Server=TU_SERVIDOR;Database=TU_BASE_DE_DATOS;Integrated Security=True;TrustServerCertificate=True;"
}

### 4. Ejecutar la aplicación
Desde Visual Studio:

- Abrí el archivo .sln del proyecto.

- Seleccioná el proyecto como proyecto de inicio.

- Ejecutá la aplicación con F5 o haciendo clic en "Iniciar depuración".
