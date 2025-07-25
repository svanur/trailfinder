# TrailFinder.DB
   
Database project for the TrailFinder application.

## Supabase

Studio URL: http://127.0.0.1:54323

## Project Structure

- `/migrations` - Database schema migrations
- `/seeds` - Development and test data
- `/scripts` - Deployment and maintenance scripts
- `/tests` - Database tests
- `/types` - TypeScript type definitions

## Getting Started

1. Install prerequisites:
   - .NET 9.0 SDK
   - PostgreSQL 15+
   - Supabase CLI

2. Install Supabase CLI

   Open PowerShell

   Install Scoop, if you don't have it already

   ```bash
   Set-ExecutionPolicy -ExecutionPolicy RemoteSigned -Scope CurrentUser
   Invoke-RestMethod -Uri https://get.scoop.sh | Invoke-Expression
   ```
   
   Add Supabase to Scoop bucket

   ```bash
   scoop bucket add supabase https://github.com/supabase/scoop-bucket.git
   ```
   
   Install Supabase via Scoop

   ```bash
   scoop install supabase
   ```

3Setup local development:
   ```bash
   supabase init
   supabase start
   ```

3. Apply migrations:
   ```bash
   ./scripts/deploy.ps1 -Environment development
   ```

## Development Workflow

1. Create new migration:
   ```bash
   ./scripts/new-migration.ps1 -Name "add_feature_x"
   ```

2. Test locally:
   ```bash
   ./scripts/test.ps1
   ```

3. Deploy to environment:
   ```bash
   ./scripts/deploy.ps1 -Environment [development|staging|production]
   ```

## Conventions

- Follow migration naming convention: `YYYYMMDDHHMMSS_description`
- Include seed data for new tables in `/supabase/seed.sql`
- Update TypeScript types in `/types/database.ts`

## Useful Commands

#### Add migrations

```bash
supabase db up
```

#### Reset database (careful - this deletes all data)

```
supabase db reset
```

#### Start Supabase services

```bash
supabase start
```

####  Stop Supabase services


```bash
supabase stop
```

#### View logs

```bash
supabase logs
```

#### Create new migration

```
supabase migration new feature_name
```
