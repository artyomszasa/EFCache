using System;
using System.Linq;
using EFCache.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace EFCache
{
    class Program
    {
        class RootWithValue
        {
            public Root Root { get; set; }

            public int? Value { get; set; }
        }
        static IServiceProvider Startup()
        {
            return new ServiceCollection()
                .AddLogging(b => b.ClearProviders().SetMinimumLevel(LogLevel.Debug).AddConsole())
                .AddDbContext<MyDbContext>(b => b.UseNpgsql("Host=dummy.db; Username=udummy; Password=secret; Database=ddummy"))
                .BuildServiceProvider(true);
        }

        static void Main(string[] args)
        {
            var sp = Startup();
            try
            {
                using (var scope = sp.CreateScope())
                {
                    var dbContext = scope.ServiceProvider.GetRequiredService<MyDbContext>();
                    var roots = dbContext.Set<Root>()
                        .Select(e => new RootWithValue
                        {
                            Root = e,
                            Value = e.Words.Min(w => (int?)MyDbContext.CostyOperation(w.Data))
                        })
                        .Where(e => e.Value < 2)
                        .OrderBy(e => e.Value)
                        .Select(e => e.Root);
                    foreach (var root in roots)
                    {
                        Console.WriteLine(root.Text);
                    }
                }
            }
            finally
            {
                (sp as IDisposable)?.Dispose();
            }
        }
    }
}
