# Perfume Shop

Full-stack perfume shop application.

- `frontend/`: Angular application
- `backend/`: ASP.NET Core API

## Requirements

- Node.js and npm
- .NET SDK 10
- SQL Server with a database named `PerfumeShope`

## Run the backend

From the repository root:

```powershell
dotnet run --project backend/PerfumeShopAPI.csproj --launch-profile https
```

The API runs at `https://localhost:7096` and `http://localhost:5174`.

The backend connection string is in `backend/appsettings.json`. Update it for the SQL Server instance on the machine where you clone this project. Apply the database schema/data before using registration, login, catalog, or orders.

## Run the frontend

In a second terminal:

```powershell
cd frontend
npm install
npm start
```

Open `http://localhost:4200`.

## Run with Docker

Install Docker Desktop, copy the environment template, and set strong local secrets:

```powershell
Copy-Item .env.example .env
docker compose up --build
```

Open `http://localhost:8080`. The frontend container serves the Angular app and proxies `/api` to the backend container. SQL Server data is stored in the `sqlserver-data` Docker volume.

The database schema and seed data are not included in the repository yet. Import the `PerfumeShope` database/schema into the SQL Server container before using registration, login, catalog, or orders.

The frontend expects the API at `https://localhost:7096/api`. Trust the local ASP.NET HTTPS development certificate if the browser displays a certificate warning:

```powershell
dotnet dev-certs https --trust
```

## GitHub

After creating an empty GitHub repository, connect and push it:

```powershell
git remote add origin https://github.com/YOUR_USERNAME/YOUR_REPOSITORY.git
git push -u origin main
```
