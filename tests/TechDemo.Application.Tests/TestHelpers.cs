using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Moq;
using TechDemo.Domain.Shared.Repositories;

namespace TechDemo.Application.Tests;

public static class TestHelpers
{
    public static ISender BuildSender(Mock<IUnitOfWork> unitOfWorkMock)
    {
        var services = new ServiceCollection();

        services.AddMediatR(cfg =>
            cfg.RegisterServicesFromAssembly(typeof(ApplicationExtensions).Assembly))
            .AddSingleton(unitOfWorkMock.Object);

        var provider = services.BuildServiceProvider();
        var sender = provider.GetRequiredService<ISender>();

        return sender;
    }
}