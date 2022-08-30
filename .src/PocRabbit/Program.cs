using PocRabbit;

IHost host = Host.CreateDefaultBuilder(args)
    .ConfigureServices(services =>
    {
        services.AddHostedService<ProducerWorker>();
        services.AddHostedService<ConsumerWorker>();
    })
    .Build();

await host.RunAsync();
