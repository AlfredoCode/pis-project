# Project Registration System – Backend

## Setup

1. Postgres

    1. install PostgreSQL 17 (and optionally pgAdmin)
        - remember the password you set for the `postgres` user

    2. ensure `createdb` is in the PATH environment variable (e.g. add Postgres's `bin` directory to PATH)

    3. create database `pisdb`:

        ```sh
        createdb --username=postgres --encoding=UTF-8 pisdb
        ```
        (or you could create the database under a different user)

2. set up EF Core connection to the database:
    
    1. add the connection string to dotnet user-secrets:
    
        ```sh
        cd API
        dotnet user-secrets init
        dotnet user-secrets set ConnectionStrings:PisDB "Host=localhost:5432; Username=postgres; Password=...; Database=pisdb"
        ```
        (fill in the actual password for the user)

        NOTE: the database does NOT have a password, the user ("role") does

    2. install (or `update` instead of `install`) EF Core CLI:
        ```sh
        dotnet tool install --global dotnet-ef
        ```
    
    3. run migrations to initialize the database tables

        ```sh
        cd API
        dotnet ef database update --project ../DAL
        ```

3. trust the development certificate:
   ```sh
   dotnet dev-certs https --trust
   ```

4. run the application:
    ```sh
    cd API
    dotnet run
    ```

5. open the browser at <https://localhost:7117/scalar> to see the API documentation/playground
