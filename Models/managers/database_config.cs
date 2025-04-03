// ============================================================================
class DatabaseTableForeignKey
{
    public string field { get; set; } = "";
    public string ref_table { get; set; } = "";
    public string ref_field { get; set; } = "";
}

// ============================================================================
class DatabaseTableField
{
    public string name { get; set; } = "";
    public string sql_type { get; set; } = "";
    public string dtype { get; set; } = "";
}

// ============================================================================
class DatabaseTableConfig
{
    public string name { get; set; } = "";
    public string json_file { get; set; } = "";
    public List<DatabaseTableField> fields { get; set; } = new();
    public List<DatabaseTableForeignKey> foreign_keys { get; set; } = new();
}

// ============================================================================
class DatabaseConfig
{
    public string database_name { get; set; } = "";
    public Dictionary<string, DatabaseTableConfig> tables { get; set; } = new();
}

// ============================================================================

/* EOF */
