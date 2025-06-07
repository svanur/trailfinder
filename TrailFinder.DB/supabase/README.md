# Database Migrations

This directory contains all database migrations for the TrailFinder project.

## Structure

Migrations follow this structure:


migrations/ 
└── NNNN_description/ 
    ├── up.sql # Forward migration 
    └── down.sql # Rollback migration


Where:
- `NNNN` is a 4-digit sequential number (e.g., 0000, 0001, 0002)
- `description` is a brief description of what the migration does

## Examples:

1. To apply migrations (up):
   ```bash
   supabase db reset
   ```

2. To rollback a migration:
   ```bash
   supabase db reset --version NNNN
   ```

3. Document any specific steps in the migration file if needed

## Important Notes

- Never modify existing migrations
- Always create new migrations for changes
- Test migrations in a development environment first
- Keep up.sql and down.sql in sync - every change in up.sql should have a corresponding rollback in down.sql

