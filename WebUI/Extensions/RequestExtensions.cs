
public static class RequestExtensions
{
    public static string GetIpAddress(this HttpRequest request)
    {
        if (request.Headers.ContainsKey("CF-CONNECTING-IP"))
            return request.Headers["CF-CONNECTING-IP"].FirstOrDefault();

        if (request.Headers.ContainsKey("X-Forwarded-For"))
        {
            var ipAddress = request.Headers["X-Forwarded-For"].FirstOrDefault();
            if (!string.IsNullOrEmpty(ipAddress))
            {
                var addresses = ipAddress.Split(',');
                if (addresses.Length != 0)
                    return addresses[0].Trim();  // Trim any white spaces
            }
        }

        return request.HttpContext.Connection.RemoteIpAddress?.ToString();
    }
}
