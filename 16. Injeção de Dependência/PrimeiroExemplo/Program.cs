//dotnet add package Microsoft.Extensions.DependencyInjection
using Microsoft.Extensions.DependencyInjection;
using PrimeiroExemplo.src.Interfaces;
using PrimeiroExemplo.src.Model;


//! Todos podem ser adicionados aos servicos com interface e classe, ou somente com a classe.
//! Se implementado com a interface, ao chamar o serviço, deve-se usar a Interface
//! Ex. Se declarar: .AddScoped<ScopedService>(), usa-se .GetService<ScopedService>();
//! Ex. Se declarar: .AddScoped<IService, ScopedService>(), usa-se .GetService<IService>();

var serviceProvider = new ServiceCollection()
                .AddTransient<TransientService>()
                .AddScoped<ScopedService>()
                .AddSingleton<SingletonService>()
                .BuildServiceProvider();

//criando escopo1
Console.WriteLine("-------Iniciando primeiro escopo:");
using (var scope = serviceProvider.CreateScope()) {
    var transientService1 = serviceProvider.GetService<ScopedService>();
    transientService1.PrintId();

    var scopedService1 = scope.ServiceProvider.GetService<ScopedService>();
    scopedService1.PrintId();

    var singletonService1 = serviceProvider.GetService<SingletonService>();
    singletonService1.PrintId();


    //segunda requisição
    //transient vai mudar, pois é uma nova classe usada
    //scoped vai manter, pois ainda estamos no mesmo escopo
    //singleton vai manter pois é uma para o mesmo programa
    Console.WriteLine("-------Requisitando novos serviços dentro do primeiro escopo:");

    var transientService2 = serviceProvider.GetService<ScopedService>();
    transientService2.PrintId();

    var scopedService2 = scope.ServiceProvider.GetService<ScopedService>();
    scopedService2.PrintId();

    var singletonService2 = serviceProvider.GetService<SingletonService>();
    singletonService2.PrintId();
}

Console.WriteLine("-------Iniciando segundo escopo:");
using (var scope = serviceProvider.CreateScope()) {
    //transient vai mudar, pois é uma nova classe usada
    //scoped vai mudar, pois mudamos de escopo
    //singleton vai manter pois é uma para o mesmo programa

    var transientService2 = serviceProvider.GetService<ScopedService>();
    transientService2.PrintId();

    var scopedService2 = scope.ServiceProvider.GetService<ScopedService>();
    scopedService2.PrintId();

    var singletonService2 = serviceProvider.GetService<SingletonService>();
    singletonService2.PrintId();
}