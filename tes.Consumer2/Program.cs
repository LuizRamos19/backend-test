// See https://aka.ms/new-console-template for more information
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using tes.Consumer2;
using teste.Infrastructure.Sevices.ExchangeRate;
using teste.Repositories.Redis;
using teste.Repositories.Sales;

IHost _host = Host.CreateDefaultBuilder().ConfigureServices(
            services => {
                services.AddSingleton<IApplication, Application>();
                services.AddSingleton<ISaleRepository, SaleRepository>();
                services.AddSingleton<IRedisRepository, RedisRepository>();
                services.AddSingleton<IExchangeRateService, ExchangeRateService>();
                services.AddStackExchangeRedisCache(o => {
                    o.InstanceName = "redis";
                    o.Configuration = "127.0.0.1:6379";
                });
            }
        ).Build();
var app = _host.Services.GetRequiredService<IApplication>();

await app.RunAsync();
