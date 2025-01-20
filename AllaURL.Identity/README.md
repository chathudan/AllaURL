

## Identity  Migration Command


### For ApplicationDbContext  (ASP.NET Core Identity):
```bash
dotnet ef migrations add InitialCreate --context ApplicationDbContext
dotnet ef database update --context ApplicationDbContext
```

### For ConfigurationDbContext  (IdentityServer configuration data):
```bash
dotnet ef migrations add AddIdentityServerSchema --context ConfigurationDbContext
dotnet ef database update --context ConfigurationDbContext
```

### For PersistedGrantDbContext  (IdentityServer operational data):

```bash
dotnet ef migrations add AddPersistedGrantSchema --context PersistedGrantDbContext
dotnet ef database update --context PersistedGrantDbContext
```