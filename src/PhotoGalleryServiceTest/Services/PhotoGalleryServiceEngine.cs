﻿using Boilerplate.Features.Core.Commands;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using System;
using System.IO;

namespace PhotoGalleryServiceTest.Services;

public abstract class PhotoGalleryServiceEngine
    : WebApplicationFactory<Program>
{
    private readonly DistributedServiceEngine _distributedServiceEngine;

    public PhotoGalleryServiceEngine() 
    {
        _distributedServiceEngine = CreateDistributedServiceEngine();
    }

    public bool ForExploratory { get; set; }

    protected override void Dispose(bool disposing)
    {
        base.Dispose(disposing);

        if(disposing && !ForExploratory)
        {
            Stop();
        }
    }

    protected bool Started { get; set; }

    public virtual void Start()
    {
        if (Started)
        {
            if(ForExploratory)
            {
                throw new InvalidOperationException("This engine should only be used for Exploratory testning and only run once in a test session");
            }

            return;
        }

        //StartDistributedService();
        Services.GetService(typeof(ICommandDispatcher));

        Started = true;
    }

    public virtual void Stop() 
    {
        if(!Started)
        {
            return;
        }

        //StopDistributedService();
        Started = false;
    }

    protected override IHost CreateHost(IHostBuilder builder)
    {
        return base.CreateHost(builder);
    }

    public virtual void StartDistributedService()
    {
        _distributedServiceEngine.StartAsync()
            .GetAwaiter()
            .GetResult();
    }

    public virtual void StopDistributedService()
    {
        _distributedServiceEngine.StopAsync()
            .GetAwaiter()
            .GetResult();
    }

    protected abstract DistributedServiceEngine CreateDistributedServiceEngine();

    protected IConfiguration GetConfiguration()
    {
        return new ConfigurationBuilder()
            .SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
            .AddJsonFile($"appsettings.Development.json", optional: true)
            .AddEnvironmentVariables()
            .Build();
    }
}

