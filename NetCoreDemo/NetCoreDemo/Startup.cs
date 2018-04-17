using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using Swashbuckle.AspNetCore.Swagger;
using Microsoft.Extensions.PlatformAbstractions;
using System.IO;
using NetCoreDemo.Middleware;

namespace NetCoreDemo
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
            services.AddMvc()
             //全局配置Json序列化处理
            .AddJsonOptions(options =>
             {
                 //忽略循环引用
                 options.SerializerSettings.ReferenceLoopHandling = ReferenceLoopHandling.Ignore;
                 //不使用驼峰样式的key
                 //options.SerializerSettings.ContractResolver = new DefaultContractResolver();
                 //设置时间格式
                 options.SerializerSettings.DateFormatString = "yyyy-MM-dd HH:mm:ss";
             }
            );
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new Info { Title = "My API", Version = "v1" });

                var basePath = PlatformServices.Default.Application.ApplicationBasePath;

                var xmlPath = Path.Combine(basePath, "NetCoreDemo.xml");
                var xmlPath1 = Path.Combine(basePath, "Demo.Model.xml");
                c.IncludeXmlComments(xmlPath);
                c.IncludeXmlComments(xmlPath1);
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            app.UseSwagger();
            app.UseSwaggerUI(c => {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "我的API V1");
            });

            app.UseSecurity();

            app.UseMvc();
        }
    }
}
