using Autofac.Extensions.DependencyInjection;
using Autofac;
using Market.Repos;
using Market.Abstract;
using Microsoft.Extensions.FileProviders;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddAutoMapper(typeof(MappingProfiles));
builder.Services.AddMemoryCache(x =>
{
    x.TrackStatistics = true;
});

builder.Host.UseServiceProviderFactory(new AutofacServiceProviderFactory());
builder.Host.ConfigureContainer<ContainerBuilder>(cb => cb.RegisterType<ProductRepo>()
            .As<IProductRepo>());

builder.Host.ConfigureContainer<ContainerBuilder>(cb => cb.RegisterType<CategoryRepo>()
.As<ICategoryRepo>());

//var confBuilder = new ConfigurationBuilder();
//confBuilder.SetBasePath(Directory.GetCurrentDirectory());
//confBuilder.AddJsonFile("appsettings.json");
//var autoFacconf = confBuilder.Build();


var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

//var staticFilesPath = Path.Combine(Directory.GetCurrentDirectory(), "StaticFiles");
//if (!Directory.Exists(staticFilesPath))
//{
//    Directory.CreateDirectory(staticFilesPath);
//}

//app.UseStaticFiles(new StaticFileOptions
//{
//    FileProvider = new PhysicalFileProvider(staticFilesPath),
//    RequestPath = "/static"
//});

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
