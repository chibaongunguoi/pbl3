namespace module_info;

using System.Text.RegularExpressions;
using Microsoft.Data.SqlClient;
using module_data;

abstract class InfoString : DataObj
{
    // ========================================================================
    private string content = "";

    // ========================================================================
    public InfoString() { }

    // ------------------------------------------------------------------------
    public InfoString(string content)
    {
        this.content = content;
    }

    // ========================================================================
    public override int fetch(SqlDataReader reader, int pos)
    {
        this.content = reader.GetString(pos++);
        return pos;
    }

    // ------------------------------------------------------------------------
    public override bool is_valid()
    {
        return Regex.IsMatch(this.content, get_pattern());
    }

    // ========================================================================
    public virtual string get_pattern()
    {
        return "";
    }

    // ------------------------------------------------------------------------
    public string get_content()
    {
        return this.content;
    }

    // ========================================================================
}

/* EOF */
