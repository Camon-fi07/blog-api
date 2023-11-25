namespace weblog_API.Models;

public class SearchAddress
{
    public long Objectid { get; set; }

    public Guid Objectguid { get; set; }

    public string Text { get; set; }

    public string ObjectLevel { get; set; }

    public string ObjectLevelText { get; set; }
}