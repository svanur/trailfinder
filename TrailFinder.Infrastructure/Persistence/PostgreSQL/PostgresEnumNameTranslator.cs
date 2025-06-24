using Npgsql;

namespace TrailFinder.Infrastructure.Persistence.PostgreSQL;

public class PostgresEnumNameTranslator : INpgsqlNameTranslator
{
    public string TranslateTypeName(string name) => name;
    public string TranslateMemberName(string name) => name.ToLower();
}
