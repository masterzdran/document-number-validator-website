var builder = WebApplication.CreateBuilder(args);

builder.Services.AddRazorPages();

var app = builder.Build();

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseRouting();

// Custom 404 page while preserving the 404 status code (SEO requirement).
app.UseStatusCodePagesWithReExecute("/NotFound");

app.MapRazorPages();

app.Run();
