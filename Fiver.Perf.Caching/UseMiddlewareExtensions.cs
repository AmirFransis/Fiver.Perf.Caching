using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Caching.Distributed;
using System.Threading.Tasks;

namespace Fiver.Perf.Caching
{
    public static class UseMiddlewareExtensions
    {
        public static IApplicationBuilder UseWriteCaching(this IApplicationBuilder app)
        {
            return app.UseMiddleware<WriteCachingMiddleware>();
        }

        public static IApplicationBuilder UseReadCaching(this IApplicationBuilder app)
        {
            return app.UseMiddleware<ReadCachingMiddleware>();
        }
    }

    public class WriteCachingMiddleware
    {
        private readonly RequestDelegate next;
        private readonly IDistributedCache cache;

        public WriteCachingMiddleware(
            RequestDelegate next,
            IDistributedCache cache)
        {
            this.next = next;
            this.cache = cache;
        }

        public async Task Invoke(HttpContext context)
        {
            await cache.SetObjectAsync("CurrentUser",
                new UserInfo { Username = "James", Email = "james@bond.com" });
            await this.next(context);
        }
    }

    public class ReadCachingMiddleware
    {
        private readonly RequestDelegate next;
        private readonly IDistributedCache cache;

        public ReadCachingMiddleware(
            RequestDelegate next,
            IDistributedCache cache)
        {
            this.next = next;
            this.cache = cache;
        }

        public async Task Invoke(HttpContext context)
        {
            var user = await cache.GetObjectAsync<UserInfo>("CurrentUser");
            await context.Response.WriteAsync($"{user.Username}, {user.Email}");
        }
    }
}
