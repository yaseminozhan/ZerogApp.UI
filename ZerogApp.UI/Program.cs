using System.Globalization;
using Microsoft.AspNetCore.Localization;
using Microsoft.AspNetCore.Mvc.Razor;

var builder = WebApplication.CreateBuilder(args);

// 1) Resource klas�r�n� tan�mla
builder.Services.AddLocalization(options => options.ResourcesPath = "Resources");

// 2) View + DataAnnotations lokalizasyonunu a�
builder.Services.AddControllersWithViews()
    .AddViewLocalization(LanguageViewLocationExpanderFormat.SubFolder)
    .AddDataAnnotationsLocalization();

// 3) Desteklenen k�lt�rler
var supportedCultures = new[]
{
    new CultureInfo("tr"),
    new CultureInfo("en"),
    new CultureInfo("de"),
    new CultureInfo("sk"),
    new CultureInfo("fr")
};

var locOptions = new RequestLocalizationOptions
{
    DefaultRequestCulture = new RequestCulture("tr"),
    SupportedCultures = supportedCultures,
    SupportedUICultures = supportedCultures
};

// Kullan�c� se�imini hat�rlamak i�in: cookie provider en ba�ta
locOptions.RequestCultureProviders.Insert(0, new CookieRequestCultureProvider());

var app = builder.Build();

// PIPELINE
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

// 4) Lokalizasyon middleware (erken ekle)
app.UseRequestLocalization(locOptions);

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
