using myOwnDI.Model;

//IContainerBuilder builder = new ContainerBuilder();
//builder.RegisterSingleton<IService, Service>()
//    .RegisterTrancient<IHelper>(s => new Helper())
//    .RegisterSingleton<IAnotherService>(AnotherServiceInstance.Instance);

IContainerBuilder builder = new ContainerBuilder();
var container = builder
    .RegisterTrancient<IService, Service>()
    .RegisterScoped<Controller, Controller>()
    .Build();

var scope = container.CreateScope();
var controller = scope.Resolve(typeof(Controller));
Console.WriteLine("");

interface IAnotherService
{

}

interface IHelper
{

}

class AnotherServiceInstance : IAnotherService
{
    private AnotherServiceInstance() { }
    public static AnotherServiceInstance Instance = new();
}

class Helper : IHelper
{

}

class Registration
{
    public IContainer ConfigureServices()
    {
        var builder = new ContainerBuilder();
        builder.RegisterTrancient<IService, Service>();
        builder.RegisterScoped<Controller, Controller>();
        return builder.Build();
    }
}

class Controller
{
    private readonly IService _service;
    public Controller(IService service)
    {
        _service = service;
    }

    public void Do()
    {

    }

}

interface IService
{

}

class Service : IService
{

}