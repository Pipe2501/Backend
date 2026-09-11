# Backend.API

API .NET 10 con autenticación JWT y base de datos PostgreSQL (Docker).

## Requisitos

- [.NET SDK 10](https://dotnet.microsoft.com/download)
- [Docker Desktop](https://www.docker.com/products/docker-desktop/) (o cualquier PostgreSQL en local)

## Puesta en marcha

> Puedes levantar la base de datos con **Docker** (opción recomendada, pasos 1-4) o con un **PostgreSQL instalado localmente** (ver sección "Opción alternativa" más abajo).

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

## Opción alternativa: correr sin Docker (PostgreSQL local)

Si no quieres usar Docker, instala PostgreSQL directamente en tu máquina:

1. **Instala PostgreSQL** desde https://www.postgresql.org/download/ (acepta los valores por defecto: puerto `5432`, usuario `postgres`, deja que se registre como servicio de Windows y define la contraseña que usará el servicio).

2. **Crea la base de datos** `database` con `psql`:

   ```bash
   psql -U postgres -c "CREATE DATABASE database;"
   ```

3. **Asegúrate de que la contraseña coincida** con la que la API espera (`Database:Password`, por defecto `postgres`). Si elegiste otra en la instalación, actualiza el secreto:

   ```bash
   dotnet user-secrets set "Database:Password" "tu-contraseña" --project src/Backend.API/Backend.API.csproj
   ```

4. **Configura los secretos** (si aún no los habías definido en esta máquina):

   ```bash
   dotnet user-secrets init --project src/Backend.API/Backend.API.csproj
   dotnet user-secrets set "Jwt:Key" "usa-una-clave-larga-y-aleatoria" --project src/Backend.API/Backend.API.csproj
   dotnet user-secrets set "Database:Password" "tu-contraseña" --project src/Backend.API/Backend.API.csproj
   ```

5. **Aplica las migraciones** (crea las tablas):

   ```bash
   dotnet tool restore
   dotnet tool run dotnet-ef database update --project src/Backend.API --startup-project src/Backend.API
   ```

6. **Ejecuta la API**:

   ```bash
   dotnet run --project src/Backend.API
   ```

> No hay que cambiar nada en el código: el proyecto solo necesita un PostgreSQL accesible en `localhost:5432` con la base `database`, igual que hace Docker. Si tu PostgreSQL local usa otro puerto u host, ajústalos en `Database:Port` / `Database:Host` (appsettings o User Secrets).

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

## CORS (conexión desde el front)

En desarrollo se aceptan peticiones de cualquier origen en `localhost` (React, Angular, etc.), no hay que configurar nada.

Para producción, define los orígenes permitidos en `appsettings.json` (o con la variable de entorno `Cors__AllowedOrigins__0`):

```json
"Cors": {
  "AllowedOrigins": ["https://tu-front.example.com"]
}
```

## Base de datos

- Base: `database`
- Usuario: `postgres`
- Contraseña: `postgres` (por defecto)
- Puerto: `5432`

Para conectarse desde un cliente (DBeaver, pgAdmin): host `localhost`, puerto `5432`, base `database`, usuario `postgres`, contraseña `postgres`.