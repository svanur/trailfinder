using System;
using Microsoft.EntityFrameworkCore.Migrations;
using NetTopologySuite.Geometries;
using TrailFinder.Core.Enums;

#nullable disable

namespace TrailFinder.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class InitialMigration : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            // The database schema is managed by Supabase, so no schema changes here.
            // This migration solely exists to mark the current model state in __EFMigrationsHistory.
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            // No schema changes to revert, as Up() did not create any.
        }
    }
}
