using AutoMapper;
using ErrorDespacho.DependencyContainer;
using ErrorDespacho.Mapper;
using Microsoft.Extensions.DependencyInjection;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllersWithViews();
builder.Services.AddHttpClient();
builder.Services.DependencyInjection();

builder.Services.AddAutoMapper(typeof(PayloadProfile).Assembly);


builder.Services.AddScoped(sp =>
    sp.GetRequiredService<IConfiguration>()
      .GetConnectionString("OfimaConnection"));

var app = builder.Build();
using (var scope = app.Services.CreateScope())
{
    var mapper = scope.ServiceProvider.GetRequiredService<IMapper>();
}

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseRouting();

app.UseAuthorization();

app.MapStaticAssets();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}")
    .WithStaticAssets();


app.Run();
