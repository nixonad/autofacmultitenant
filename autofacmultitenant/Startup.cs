﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Autofac;
using Autofac.Extensions.DependencyInjection;
using Autofac.Multitenant;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace autofacmultitenant
{
  public class Startup
  {
    public Startup(IConfiguration configuration) {
      this.Configuration = configuration;
    }

    public IConfiguration Configuration { get; }

    public static MultitenantContainer ApplicationContainer { get; private set; }

    // This method gets called by the runtime. Use this method to add services to the container.
    public IServiceProvider ConfigureServices(IServiceCollection services) {
      services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_2);
     
      var containerBuilder = new ContainerBuilder();
      containerBuilder.RegisterType<MyTenantIdentificationStrategy>().AsImplementedInterfaces();
      containerBuilder.Populate(services);
      var autofacContainer = containerBuilder.Build();

      var mtc = new MultitenantContainer(autofacContainer.Resolve<ITenantIdentificationStrategy>(), autofacContainer);
      Startup.ApplicationContainer = mtc;
      // Create the IServiceProvider based on the container.
      return new AutofacServiceProvider(Startup.ApplicationContainer);
    }

    // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
    public void Configure(IApplicationBuilder app, IHostingEnvironment env) {
      if (env.IsDevelopment()) {
        app.UseDeveloperExceptionPage();
      } else {
        // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
        app.UseHsts();
      }

      app.UseHttpsRedirection();
      app.UseMvc();
    }
  }
}
