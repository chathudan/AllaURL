
# Database Migration Steps

This document outlines the steps for running database migrations for the **AllaURL** project, which includes **PostgreSQL** as the database provider and utilizes **Entity Framework Core** for database management.

### Prerequisites
1. **Docker**: Ensure that Docker and Docker Compose are installed and running on your machine.
2. **.NET SDK**: Ensure that you have the .NET SDK installed to work with Entity Framework Core migrations.
3. **PostgreSQL Docker Container**: The PostgreSQL database should be running in a Docker container, as defined in the `docker-compose.yml`.

### Steps for Database Migration

#### 1. **Ensure Docker and PostgreSQL Are Running**
   
   Before running migrations, ensure that the Docker containers are running and the PostgreSQL database is up:

   ```bash
   docker-compose up -d
   ```

   This will start the PostgreSQL and PGAdmin services as defined in the `docker-compose.yml` file.

#### 2. **Run the Migration Command**

   To apply the migrations, we use the **.NET CLI** to generate and apply migrations. Ensure that you're in the **`AllaURL.Database`** directory (the project responsible for managing the migration).

   Run the following commands:

   - **Add a Migration**:
     
     To create a new migration based on the changes to your models, run:

     ```bash
     dotnet ef migrations add InitialMigration --project ../AllaURL.Data/AllaURL.Data.csproj --startup-project . --output-dir Migrations
     ```

     - **`--project`** specifies the project that contains the `DbContext` (i.e., `AllaURL.Data`).
     - **`--startup-project`** specifies the **startup project**, which should be the **`AllaURL.Database`** project.
     - **`--output-dir`** specifies where the migration files will be placed. This is set to the **`Migrations`** folder in **`AllaURL.Database`**.

   - **Apply the Migration**:

     Once the migration is created, apply it to the database using:

     ```bash
     dotnet ef database update --project ../AllaURL.Data/AllaURL.Data.csproj --startup-project .
     ```

     This command will apply the migrations to the PostgreSQL database defined in the connection string. Make sure that the connection string is correctly configured in the `appsettings.Development.json` file or in the environment variables.

#### 3. **Verify the Migration**
   
   To ensure the migration was applied successfully, you can check the database schema using a database tool like **PGAdmin** or any PostgreSQL client to verify the tables were created/updated as expected.

   You can also check the logs of the Docker container:

   ```bash
   docker logs postgres-db
   ```

   This will show any potential issues or errors related to the database connection or migration application.

#### 4. **Revert a Migration** (if necessary)

   If you need to **rollback** a migration, you can use the `dotnet ef` command to revert to a previous migration:

   ```bash
   dotnet ef database update <PreviousMigrationName> --project ../AllaURL.Data/AllaURL.Data.csproj --startup-project .
   ```

   Replace `<PreviousMigrationName>` with the name of the migration you want to revert to. This command will revert the database schema back to the state defined by the specified migration.

#### 5. **Handling Schema Changes**

   Whenever you make changes to your model (e.g., adding/removing tables, changing columns), you will need to:

   - Add a new migration using `dotnet ef migrations add <MigrationName>`.
   - Apply the migration using `dotnet ef database update`.

   This ensures that your PostgreSQL schema remains in sync with the models in your code.

### Troubleshooting
- **"Password Authentication Failed" Error**:
   If you encounter a `password authentication failed` error while connecting to PostgreSQL, ensure the correct credentials are used, and verify that the `pg_hba.conf` file is set to require password authentication (`md5`).
   
- **Migration Errors**:
   If you run into issues with migrations failing, check the **PostgreSQL container logs** to see if there are any permission issues or conflicts in the schema.

### Summary
These are the steps to run and manage migrations in your **AllaURL** project:

1. Ensure Docker and PostgreSQL are running.
2. Add and apply migrations using the **`dotnet ef`** commands.
3. Verify the schema changes using a PostgreSQL client.
4. Rollback migrations if necessary.

Let me know if you encounter any issues or need further guidance!
