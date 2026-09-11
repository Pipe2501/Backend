# Backend.API

API .NET 10 con autenticación JWT y base de datos PostgreSQL (Docker).

## Requisitos

- [.NET SDK 10](https://dotnet.microsoft.com/download)
- [Docker Desktop](https://www.docker.com/products/docker-desktop/) (o cualquier PostgreSQL en local)

## Puesta en marcha

### 1. Levantar la base de datos (Docker)

Desde la raíz del proyecto:

```bash
docker compose up -d
```

Esto crea un contenedor PostgreSQL con la base de datos `database`, usuario `postgres` y contraseña `postgres` (configurable con la variable de entorno `POSTGRES_PASSWORD`).

### 2. Configurar los secretos locales (User Secrets)

La API no guarda secretos en el repositorio. Ejecuta una vez por máquina:

```bash
dotnet user-secrets init --project src/Backend.API/Backend.API.csproj
dotnet user-secrets set "Jwt:Key" "usa-una-clave-larga-y-aleatoria" --project src/Backend.API/Backend.API.csproj
dotnet user-secrets set "Database:Password" "postgres" --project src/Backend.API/Backend.API.csproj
```

> Si el valor de `Jwt:Key` ya existe en tu equipo, consérvalo (el equipo debe compartir la misma clave si se validan tokens entre sí). En producción se leen de variables de entorno (`Jwt__Key`, `Database__Password`).

### 3. Crear la base de datos (migraciones)

```bash
dotnet tool restore
dotnet tool run dotnet-ef database update --project src/Backend.API --startup-project src/Backend.API
```

### 4. Ejecutar la API

```bash
dotnet run --project src/Backend.API
```

La API queda disponible en `http://localhost:5255` (dev). También es posible abrir la solución `Backend.slnx` en Visual Studio y presionar F5.

## Endpoints

| Método | Ruta                  | Descripción                          |
|--------|-----------------------|--------------------------------------|
| POST   | `/api/auth/register`  | Registra un usuario y devuelve token |
| POST   | `/api/auth/login`     | Inicia sesión y devuelve token       |

**Registro**

```json
{
  "name": "Juan",
  "email": "juan@test.com",
  "password": "secreto123"
}
```

**Login**

```json
{
  "email": "juan@test.com",
  "password": "secreto123"
}
```

Ambos devuelven `{ "token": "...", "name": "...", "email": "..." }`. Usa el token en el header `Authorization: Bearer <token>` para los endpoints protegidos.

## Base de datos

- Base: `database`
- Usuario: `postgres`
- Contraseña: `postgres` (por defecto)
- Puerto: `5432`

Para conectarse desde un cliente (DBeaver, pgAdmin): host `localhost`, puerto `5432`, base `database`, usuario `postgres`, contraseña `postgres`.