using module_info;

sealed class InfoPassword : InfoString
{
    // ========================================================================
    public InfoPassword() { }

    // ------------------------------------------------------------------------
    public InfoPassword(string content)
        : base(content) { }

    // ========================================================================
    public override string get_pattern()
    {
        return "";
    }

    // ========================================================================
}

/* EOF */
