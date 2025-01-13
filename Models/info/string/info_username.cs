using module_info;

sealed class InfoUsername : InfoString
{
    // ========================================================================
    public InfoUsername() { }

    // ------------------------------------------------------------------------
    public InfoUsername(string content)
        : base(content) { }

    // ========================================================================
    public override string get_pattern()
    {
        return "";
    }

    // ========================================================================
}

/* EOF */
