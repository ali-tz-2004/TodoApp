namespace TodoApp.Infrastructure.Models;

public class Configs
{
    public string TokenKey { get; set; } = string.Empty;
    public string TokenIssuer { get; set; } = string.Empty;
    public string TokenAudience { get; set; } = string.Empty;
    public int TokenTimeout { get; set; }
}