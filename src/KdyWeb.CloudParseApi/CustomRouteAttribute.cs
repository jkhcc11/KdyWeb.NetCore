using Microsoft.AspNetCore.Mvc;

namespace KdyWeb.CloudParseApi
{
    public class CustomRouteAttribute : RouteAttribute
    {
        public CustomRouteAttribute(string template) : base("api/cloud-parse/v1/" + template)
        {

        }
    }
}
