namespace AllaURL.API;

public static class Helper
{
    public static string GetClientIp(HttpContext context) => context?.Connection.RemoteIpAddress?.ToString() ?? throw new InvalidOperationException();
}