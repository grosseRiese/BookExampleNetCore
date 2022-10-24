using Bookstore.Models;
using Bookstore.Models.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using System.Configuration;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorPages();

//builder.Services.AddControllersWithViews();
builder.Services.AddMvc();
/*
 * builder.Services.AddSingleton<IBookstoreRepository<Author>, AuthorRepository>();
 * builder.Services.AddSingleton<IBookstoreRepository<Book>, BookRepository>();
**/
builder.Services.AddScoped<IBookstoreRepository<Author>, AuthorDbRepository>();
builder.Services.AddScoped<IBookstoreRepository<Book>, BookDbRepository>();

builder.Services.AddDbContext<BookstoreDbContext>();


var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}



app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();
app.UseAuthorization();

/* 
 * app.UseEndpoints(endpoints =>
{
    endpoints.MapControllerRoute(
        name: "default",
        pattern: "{controller=Book}/{action=Index}/{id?}");
});*/

app.MapControllerRoute(
        name: "default",
        pattern: "{controller=Book}/{action=Index}/{id?}"
        );

//app.MapDefaultControllerRoute();



app.MapRazorPages();

app.Run();
