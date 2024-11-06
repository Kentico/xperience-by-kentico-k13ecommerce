# Contributing Setup

## Required Software

The requirements to setup, develop, and build this project are listed below.

### .NET Runtime

.NET SDK 7.0 or newer

- <https://dotnet.microsoft.com/en-us/download/dotnet/8.0>
- See `global.json` file for specific SDK requirements

### Node.js Runtime

- [Node.js](https://nodejs.org/en/download) 20.10.0 or newer
- [NVM for Windows](https://github.com/coreybutler/nvm-windows) to manage multiple installed versions of Node.js
- See `engines` in the solution `package.json` for specific version requirements

### C# Editor

- VS Code
- Visual Studio
- Rider

### Database

SQL Express 2022 (v16.0):
- [SQL server downloads](https://www.microsoft.com/en-us/sql-server/sql-server-downloads)

### SQL Editor

- MS SQL Server Management Studio
- Azure Data Studio

## Sample Project

### Startup
1. You need access to valid localhost key for XbyK and for K13 (as they are not part of shipped databases).
2. Build whole solution.

### Database Setup

Project uses [Continuous integration](https://docs.kentico.com/developers-and-admins/ci-cd/continuous-integration) (CI) with initial data.

Initialize database with script `Init-Project.ps1` in scripts folder. It performs these steps:
1. Unzip database backups in /database folder (if not already unzipped)
2. Restore DB from backups
3. Wait for inserting license keys to both databases.
4. Turn off CI
5. Applying last supported hotfix to XbK database.
6. Turn on CI
7. CI Restore for XbK databse.

You can do these steps manually.
```powershell
PS ...\xperience-by-kentico-ecommerce> cd .\scripts\
PS ...\xperience-by-kentico-ecommerce\scripts> .\Init-Project.ps1
...
...
"Database Kentico13_DancingGoat restored from backup yyyy_mm_dd_Kentico13_DancingGoat.bak"
...
...
"Database XByK_DancingGoat_K13Ecommerce restored from backup yyyy_mm_dd_XByK_DancingGoat_K13Ecommerce.bak"


"Import license key to imported DBs, then press any key to continue..."
```

Import license key for K13 (via SQL editor), you may need to edit some of values based on your license key. Replace `K13_LICENSE_KEY` with valid license key for K13 and `K13_EXPIRATION` with date of expiration of your license key in form of `mm/dd/yyyy`:
```sql
INSERT INTO [dbo].[CMS_LicenseKey] VALUES ('localhost', 'K13_LICENSE_KEY', 'X', 'K13_EXPIRATION', 1);
```

Import license key for XbK. Replace `XBK_LICENSE_KEY` with valid license key for XbK:
```sql
UPDATE CMS_SettingsKey SET KeyValue='XBK_LICENSE_KEY' WHERE KeyName='CMSLicenseKey'
```

In `Init-Project` script press any key to continue:
```powershell
...
...
"CI restore for Enabled=False"

Updating database...
Executing SQL migration scripts...
Executing code migration scripts...

Successfully updated database to x.x.x version.

"Updated DB to latest hotfix"
...
...
"CI restore for Enabled=True"
...
....
Restoring objects...
...
...
Optimizing file repository...
Restore operation successfully finished.

"CI files processed"

PS ...\xperience-by-kentico-ecommerce\scripts>
```

### Synchronize to last stable version
After `git pull` run script `Reset-DatabaseConsistency.ps1`. You can also run it for reverting unwanted changes in database (but you have to revert beforehand all .xml file in `examples\DancingGoat-K13Ecommerce\App_Data\CIRepository` folder)

It perform these steps:
1. Turn off CI
2. Applying last supported hotfix to XbK database.
3. Turn on CI
4. CI Restore for XbK databse.

```powershell
PS ...\xperience-by-kentico-ecommerce> cd .\scripts\
PS ...\xperience-by-kentico-ecommerce\scripts> .\Reset-DatabaseConsistency.ps1
...
...
"CI restore for Enabled=False"
...
...
Updating database...
Executing SQL migration scripts...
Executing code migration scripts...

Successfully updated database to x.x.x version.

"Updated DB to latest hotfix"
...
...
"CI restore for Enabled=True"
...
...
Restoring objects...
...
...
Optimizing file repository...
Restore operation successfully finished.

"CI files processed"

PS ...\xperience-by-kentico-ecommerce\scripts>
```

### Admin Customization

To run the Sample app Admin customization in development mode, add the following to your [User Secrets](https://learn.microsoft.com/en-us/aspnet/core/security/app-secrets?view=aspnetcore-7.0&tabs=windows#secret-manager) for the application.

```json
"CMSAdminClientModuleSettings": {
  "kentico-xperience-integrations-repotemplate": {
    "Mode": "Proxy",
    "Port": 3009
  }
}
```

## Development Workflow

1. Create a new branch with one of the following prefixes

   - `feat/` - for new functionality
   - `refactor/` - for restructuring of existing features
   - `fix/` - for bugfixes

2. Run `dotnet format` against the `src/Kentico.Xperience.RepoTemplate` project

   > use `dotnet: format` VS Code task.

3. Commit changes, with a commit message preferably following the [Conventional Commits](https://www.conventionalcommits.org/en/v1.0.0/#summary) convention.

4. Once ready, create a PR on GitHub. The PR will need to have all comments resolved and all tests passing before it will be merged.

   - The PR should have a helpful description of the scope of changes being contributed.
   - Include screenshots or video to reflect UX or UI updates
   - Indicate if new settings need to be applied when the changes are merged - locally or in other environments
