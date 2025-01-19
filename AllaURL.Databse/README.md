
# Database Project - Dockerized PostgreSQL and PGAdmin

This project contains the configuration for a PostgreSQL database running in a Docker container, along with a PGAdmin container for database management. The setup uses **Docker Compose** to manage both services, ensuring that the database and PGAdmin are easy to spin up and access locally.

## Prerequisites

Before getting started, ensure that you have the following installed on your machine:
- **Docker**: [Install Docker](https://www.docker.com/get-started)
- **Docker Compose**: Docker Compose is included with Docker Desktop. If you are using Linux, you can follow [Docker Compose installation instructions](https://docs.docker.com/compose/install/).

## Project Structure

- `docker-compose.yml`: Docker Compose configuration to set up PostgreSQL and PGAdmin services.
- `README.md`: This documentation.
  
## Getting Started

### Step 1: Clone the Project

If you haven't cloned the project yet, do so using the following command:

```bash
git clone https://github.com/yourusername/database.git
cd database
```

### Step 2: Update Docker Compose Configuration

Ensure the **`docker-compose.yml`** is set up with the correct configuration. 

```yaml
version: '3.8'

services:
  postgres:
    image: postgres:13
    container_name: postgres-db
    environment:
      POSTGRES_USER: myuser          # Set the username for your PostgreSQL instance
      POSTGRES_PASSWORD: mypassword  # Set the password for your PostgreSQL instance
      POSTGRES_DB: mydb              # Set the default database name
    volumes:
      - postgres-data:/var/lib/postgresql/data  # Persistent storage for PostgreSQL data
    ports:
      - "5432:5432"  # Expose PostgreSQL port on localhost:5432
    networks:
      - mynetwork

  pgadmin:
    image: dpage/pgadmin4
    container_name: pgadmin
    environment:
      PGADMIN_DEFAULT_EMAIL: admin@admin.com   # Default login email for PGAdmin
      PGADMIN_DEFAULT_PASSWORD: admin           # Default login password for PGAdmin
    ports:
      - "5054:80"  # Expose PGAdmin UI on localhost:5054
    networks:
      - mynetwork
    depends_on:
      - postgres  # Ensure PGAdmin starts only after PostgreSQL is up

networks:
  mynetwork:
    driver: bridge

volumes:
  postgres-data:
    driver: local  # Persistent volume to store PostgreSQL data
```

### Step 3: Start the Services

From the root of the project directory, run the following command to start both **PostgreSQL** and **PGAdmin** services using Docker Compose:

```bash
docker-compose up -d
```

This will:
- Start the PostgreSQL database in a container.
- Start PGAdmin in a separate container.
- Expose PostgreSQL on port `5432` and PGAdmin on port `5054`.

### Step 4: Access PGAdmin

Once the containers are up and running, open your web browser and navigate to the PGAdmin UI at:

```
http://localhost:5054
```

Use the default credentials set in `docker-compose.yml` to log in:

- **Email**: `admin@admin.com`
- **Password**: `admin`

### Step 5: Connect PGAdmin to PostgreSQL

After logging in to PGAdmin, you need to connect PGAdmin to the PostgreSQL instance. Follow these steps:

1. In PGAdmin, click on **"Add New Server"** to create a new connection.
2. Under the **General** tab, provide a name for the server (e.g., "Local PostgreSQL").
3. Under the **Connection** tab, fill out the connection details:
   - **Host**: `postgres` (this is the service name in `docker-compose.yml`).
   - **Port**: `5432`
   - **Username**: `myuser` (as set in `docker-compose.yml`).
   - **Password**: `mypassword` (as set in `docker-compose.yml`).
   - **Database**: `mydb` (as set in `docker-compose.yml`).

Click **Save** to establish the connection.

### Step 6: Access and Manage the Database

Now you can use PGAdmin to manage your PostgreSQL database. You can:
- Create tables
- Run queries
- View and modify data

The database is persisted in a Docker volume (`postgres-data`), so your data will be available even after the containers are stopped or restarted.

### Step 7: Stopping and Restarting the Services

To stop the running containers, use:

```bash
docker-compose down
```

This stops the containers but keeps the data intact in the Docker volume.

To restart the services, run:

```bash
docker-compose up -d
```

### Troubleshooting

- **Cannot connect to the database in PGAdmin**: Make sure PostgreSQL is running by checking the logs:

  ```bash
  docker logs postgres-db
  ```

- **Cannot access PGAdmin UI**: Ensure PGAdmin is running and available at `http://localhost:5054`. You can check its logs:

  ```bash
  docker logs pgadmin
  ```

## Cleanup

To remove the containers and the volumes (if you no longer need the data), run:

```bash
docker-compose down -v
```

This will stop and remove the containers and the associated volumes.

## Conclusion

This setup provides a persistent PostgreSQL database running in Docker, which can be accessed and managed via PGAdmin UI. It's perfect for local development and testing.
