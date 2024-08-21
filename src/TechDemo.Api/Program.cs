using TechDemo.Infrastructure;

var builder = WebApplication.CreateBuilder(args);
{
    builder.Services.AddEndpointsApiExplorer();
    builder.Services.AddInfrastructureServices(builder.Configuration);
}

var app = builder.Build();
{
    app.UseHttpsRedirection();
}

app.Run();

