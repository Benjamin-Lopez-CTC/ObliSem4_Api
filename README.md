\# ObliSem4Api



API REST en \*\*.NET 10\*\* con \*\*Entity Framework Core\*\* y \*\*MySQL\*\* para la gestión de usuarios, proyectos y tareas con autenticación JWT.



\---



\## Requisitos previos



\- \[.NET 10 SDK](https://dotnet.microsoft.com/)

\- Servidor \*\*MySQL\*\* en ejecución (ej. MySQL Server / MySQL Workbench)

\- Herramienta CLI de EF Core (si no está instalada):

&#x20; ```bash

&#x20; dotnet tool install --global dotnet-ef

&#x20; ```

\- \*\*Postman\*\* (o cualquier cliente HTTP) para probar los endpoints



\---



\## Configuración inicial



\### 1. Clonar el repositorio

```bash

git clone <URL\_DEL\_REPOSITORIO>

cd ObliSem4Api

```



\### 2. Configurar variables de entorno (`.env`)

Copiá el archivo de ejemplo `ObliSem4Api/.env.example` y renombralo a \*\*`ObliSem4Api/.env`\*\* (debe quedar dentro de la carpeta del proyecto, junto a `Program.cs`):



```env

ConnectionStrings\_\_DefaultConnection="Server=localhost;Port=3306;Database=ObliSem4Db;User=root;Password=TU\_PASSWORD;"



Jwt\_\_Key=CLAVE\_ALEATORIA\_DE\_AL\_MENOS\_32\_CARACTERES

Jwt\_\_Issuer=ObliSem4Api

Jwt\_\_Audience=ObliSem4Client

```



Podés generar una clave segura para `Jwt\_\_Key` con:

```bash

node -e "console.log(require('crypto').randomBytes(32).toString('hex'))"

```

\*(La API valida al iniciar que `Jwt\_\_Key` exista y tenga al menos 32 caracteres).\*



\### 3. Restaurar paquetes y aplicar migraciones

Desde la carpeta del proyecto (`ObliSem4Api/`):

```bash

cd ObliSem4Api

dotnet restore

dotnet ef database update

```

Esto creará la base de datos, las tablas y los datos de prueba iniciales.



\### 4. Ejecutar la API

```bash

dotnet run

```

La API quedará disponible en:

\- \*\*HTTP:\*\* `http://localhost:5026`

\- \*\*HTTPS:\*\* `https://localhost:7287` \*(si Postman rechaza el certificado de desarrollo, usá HTTP o desactivá SSL verification en Settings).\*



\---



\## Cómo probar la API en Postman



> \*\*Nota sobre los usuarios seed:\*\* Los usuarios precargados (`juan@gmail.com`, `maria@gmail.com`, `admin@gmail.com`) tienen hashes de prueba (`HASH\_PRUEBA\_\*`), por lo que no sirven para iniciar sesión. Para probar, \*\*registrá un usuario nuevo primero\*\*.



\### Paso 1: Registrarse y obtener un token

\- `POST http://localhost:5026/api/users/register`

\- Body (`raw` → `JSON`):

&#x20; ```json

&#x20; {

&#x20;   "email": "usuario@gmail.com",

&#x20;   "password": "Password123"

&#x20; }

&#x20; ```

\- Copiá el campo `token` de la respuesta (`201 Created`).



\### Paso 2: Usar el token en endpoints protegidos

En Postman, en los endpoints que requieren autenticación, ir a la pestaña \*\*Authorization → Bearer Token\*\* y pegar el `token`.



\### Paso 3: Probar endpoints de Administrador (opcional)

1\. En MySQL, cambiá el rol del usuario registrado a `Admin`:

&#x20;  ```sql

&#x20;  UPDATE Users SET Role = 'Admin' WHERE Email = 'usuario@gmail.com';

&#x20;  ```

2\. Volvé a hacer `POST /api/users/login` para obtener un \*\*nuevo token\*\* con el rol actualizado.



\---



\## Endpoints disponibles



\### Users (`/api/users`)

| Método | Ruta | Auth | Descripción |

|---|---|---|---|

| `POST` | `/api/users/register` | Pública | Registrar usuario y obtener JWT |

| `POST` | `/api/users/login` | Pública | Iniciar sesión y obtener JWT |

| `GET` | `/api/users/me` | Bearer | Obtener datos del usuario en sesión |

| `PATCH` | `/api/users/theme` | Bearer | Cambiar tema (`"light"`, `"dark"` o `null`) |

| `GET` | `/api/users` | Admin | Listar todos los usuarios |



\### Projects (`/api/projects`)

| Método | Ruta | Auth | Descripción |

|---|---|---|---|

| `GET` | `/api/projects` | Bearer | Listar proyectos del usuario (soporta filtros y `sortBy`/`sortDesc` por query) |

| `GET` | `/api/projects/{id}` | Bearer | Obtener un proyecto por ID |

| `POST` | `/api/projects` | Bearer | Crear proyecto |

| `PATCH` | `/api/projects/{id}/color?color=blue` | Bearer | Cambiar color del proyecto (omitir `color` para `null`) |

| `PATCH` | `/api/projects/{id}/status` | Bearer | Cambiar estado (`Pendiente`, `En progreso`, `Completado`, `Archivado`) |

| `DELETE` | `/api/projects/{id}` | Bearer | Soft delete (enviar a papelera) |

| `PATCH` | `/api/projects/{id}/restore` | Bearer | Restaurar proyecto desde la papelera |

| `GET` | `/api/projects/admin` | Admin | Listar todos los proyectos |

| `DELETE` | `/api/projects/{id}/hard` | Admin | Borrado definitivo en base de datos |



\### Tasks (`/api/tasks`)

| Método | Ruta | Auth | Descripción |

|---|---|---|---|

| `GET` | `/api/tasks` | Bearer | Listar tareas paginadas del usuario con filtros opcionales por query |

| `POST` | `/api/tasks` | Bearer | Crear tarea |

