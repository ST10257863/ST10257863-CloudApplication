using Microsoft.Azure.Search;
using Microsoft.Azure.Search.Models;
using Microsoft.Extensions.DependencyInjection;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

//New Code******************************************************
// Add session services
//for JSON
builder.Services.AddHttpContextAccessor();

//for session (Getting user ID)
builder.Services.AddSession(options =>
{
	options.IdleTimeout = TimeSpan.FromMinutes(30); // Set timeout duration
	options.Cookie.HttpOnly = true; // Make the session cookie HTTP-only
	options.Cookie.IsEssential = true; // Make the session cookie essential
});

// Add distributed memory cache services (used by session)
builder.Services.AddDistributedMemoryCache();
//New Code******************************************************

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
	app.UseExceptionHandler("/Home/Error");
	// The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
	app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

//New Code******************************************************
// Add the session middleware
//for session (Getting user ID)
app.UseSession(); // Ensure session is used

//For JSON
app.UseRouting();
app.UseAuthorization();

app.UseEndpoints(endpoints =>
{
	endpoints.MapControllers();
});
//New Code******************************************************

app.MapControllerRoute(
	name: "default",
	pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();