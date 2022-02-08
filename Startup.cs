using Eauction_Seller_API.DataAccess;
using Eauction_Seller_API.Repositories;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.OpenApi.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Azure.Cosmos;
using Eauction_Seller_API.MessageSharer;
using RabbitMQ.Client;
using Eauction_Seller_API.Common;

namespace Eauction_Seller_API
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {

            services.AddControllers();
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "EauctionSellerAPI", Version = "v1" });
            });

            services.AddScoped<ICacheService, CacheService>();
            services.AddStackExchangeRedisCache(option =>
            {
                option.Configuration = Configuration.GetValue<string>("RedisCacheConString");
            });
            services.AddScoped<IRabbitMQListener, RabbitMQListener>();

            services.AddSingleton(service =>
            {
                var _config = Configuration.GetSection("RabbitMQ");
                return new ConnectionFactory()
                {
                    HostName = _config["HostName"],
                    UserName = _config["UserName"],
                    Password = _config["Password"],
                    Port = Convert.ToInt32(_config["Port"]),
                    VirtualHost = _config["VirtualHost"],
                };
            });
            services.AddDistributedMemoryCache();
            services.AddScoped<ICosmosSellerService, CosmosSellerService>();
            services.AddSingleton<ISellerRepository>(InitializeCosmosDbClientInstance(Configuration.GetSection("Cosmos")).GetAwaiter().GetResult());
            services.AddCors(c => { c.AddPolicy("AllowOrigin", option => option.AllowAnyMethod()); });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            app.UseSwagger();
            app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "SellerAPI v1"));

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthorization();

            app.UseCors(cors => cors.AllowAnyOrigin().AllowAnyHeader().AllowAnyMethod());

            app.UseMiddleware<AppExceptionHandler>();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
        private static async Task<ISellerRepository> InitializeCosmosDbClientInstance(IConfigurationSection configurationSection)
        {
            var account = configurationSection["AccountURL"];
            var key = configurationSection["AuthKey"];
            var databaseName = configurationSection["DatabaseId"];
            var collectionId = configurationSection["CollectionId"];

            var cosmosClient = new CosmosClient(account, key);
            var db = await cosmosClient.CreateDatabaseIfNotExistsAsync(databaseName);
            await db.Database.CreateContainerIfNotExistsAsync(collectionId, "/id");
            var sellerRepository = new SellerRepository(cosmosClient, databaseName, collectionId);
            return sellerRepository;
        }
    }
}
