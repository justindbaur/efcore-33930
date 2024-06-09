MRE for https://github.com/dotnet/efcore/issues/33930

# Working
```xml
<PackageReference Include="Microsoft.EntityFrameworkCore.Sqlite" Version="7.0.20" />
```

Outcome:
```bash
State Change: Original = Closed Current = Open
1 user(s)
State Change: Original = Open Current = Closed
```

The only state changes that are written to the console are the one that are written in `Program.cs`.

You can toggle `var migrateSecondDatabase = true;` so that it is now `false` and you will see the exact same logs as before. 

> Rollback `migrateSecondDatabase` change.

# Problem
```xml
<PackageReference Include="Microsoft.EntityFrameworkCore.Sqlite" Version="8.0.6" />
```

```bash
State Change: Original = Closed Current = Open
State Change: Original = Open Current = Closed
State Change: Original = Closed Current = Open
State Change: Original = Open Current = Closed
State Change: Original = Closed Current = Open
0 user(s)
State Change: Original = Open Current = Closed
```

EF will now close and open the connection by itself. The problem CAN be solved by changing `var migrateSecondDatabase = true;` so that it is now `false`. The new logs read as follows.

```bash
State Change: Original = Closed Current = Open
State Change: Original = Open Current = Closed
State Change: Original = Closed Current = Open
1 user(s)
State Change: Original = Open Current = Closed
```

I think this fix is very achievable for me to apply but this still seems like a behavior change in EF and it doesn't seem to abide by the docs on this method.

https://github.com/dotnet/efcore/blob/61f19020d9657721a26b3d0b313c3d03ef8db6f2/src/EFCore.Sqlite.Core/Extensions/SqliteDbContextOptionsBuilderExtensions.cs#L190-L193

> An existing DbConnection to be used to connect to the database. If the connection is in the open state then EF will not open or close the connection. If the connection is in the closed state then EF will open and close the connection as needed. The caller owns the connection and is responsible for its disposal.