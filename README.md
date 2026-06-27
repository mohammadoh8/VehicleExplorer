# VehicleExplorer

VehicleExplorer is a ASP.NET Core MVC web application for selecting a vehicle make, model year, and vehicle type, then viewing matching models using the NHTSA APIs.

The app uses a layered structure: MVC controllers call a vehicle catalog service, which depends on a provider abstraction for external vehicle data. The NHTSA integration maps provider-specific API responses into shared vehicle models used by the application.

## Run with IIS Express

1. Open `VehicleExplorer/VehicleExplorer.sln` in Visual Studio.
2. Select the `VehicleExplorer.Web` profile.
3. Run the application with IIS Express.
4. Open the URL shown by Visual Studio, usually similar to:

```text
https://localhost:44313
```

## Run with Docker

From the solution directory:

```bash
cd VehicleExplorer
docker build -f VehicleExplorer/Dockerfile -t vehicle-explorer .
docker run --rm -p 8080:8080 vehicle-explorer
```

Then open:

```text
http://localhost:8080
```

## Deployment

The application is deployed to an AWS EC2 instance using Docker and GitHub Actions.

Required GitHub Actions secrets:

```text
HOST
USER
SSH_KEY
URL
```
On every push to `main`, deploy.yml starts

The deployed app is available over HTTP:

```text
http://ec2-56-228-14-250.eu-north-1.compute.amazonaws.com/
```
