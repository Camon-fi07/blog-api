namespace weblog_API.AppSettingsModels;

public class TokenProperties
{
    public string Secrets { get; set; }
    public string Issuer { get; set; }
    public string Audience { get; set; }
    public string TokenLifeTime { get; set; }
}