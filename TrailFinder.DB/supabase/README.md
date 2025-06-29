# Database Migrations

This directory contains all database migrations for the TrailFinder project.

## Structure

Migrations follow this structure:

supabase/ 
└── migrations/ 
    └── YYYYMMDDHHMMSS_description/

Where:
- `YYYYMMDDHHMMSS` is a timestamp of when the migration was created
- `description` is a brief description of what the migration does

## Examples:

### Create a new migration

```
supabase migration new feature_name
```

### Reset database (careful - this deletes all data)

```
supabase db reset
```

### Document any specific steps in the migration file if needed

#### Important Notes

- Never modify existing migrations
- Always create new migrations for changes
- Test migrations in a development environment first
- Keep up.sql and down.sql in sync - every change in up.sql should have a corresponding rollback in down.sql

