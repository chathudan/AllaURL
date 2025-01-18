using Microsoft.AspNetCore.Mvc.Filters;

namespace AllaURL.API.Filters;

public class ForwarderHeaderAttribute : ResultFilterAttribute
{
    public override void OnResultExecuting(ResultExecutingContext context)
    {
        context.HttpContext.Response.Headers.Append("X-AllaURL.Admin-Version", new[] { Constants.AppVersions });
        base.OnResultExecuting(context);
    }
}