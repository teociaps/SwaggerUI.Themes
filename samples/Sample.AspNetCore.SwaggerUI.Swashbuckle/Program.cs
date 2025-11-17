using Sample.AspNetCore.SwaggerUI.Swashbuckle;
using System.Reflection;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddControllers();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(SwaggerGenConfigurer.Configure);

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();

    //string inlineStyle = "body { background-color: #000; }";
    //app.UseSwaggerUI(inlineStyle, c => c.DocumentTitle = "Sample Title");

    //app.UseSwaggerUI(CustomMinifiedStyle.CustomMin, c =>
    //app.UseSwaggerUI(CustomStyle.Custom, c =>
    //app.UseSwaggerUI(Assembly.GetExecutingAssembly(), "custom.css", c =>
    app.UseSwaggerUI(Assembly.GetExecutingAssembly(), "standalone.custom.css", c => // Fully independent - no common.css or ui.js
    //app.UseSwaggerUI(Style.Dark, c =>
    {
        c.DocumentTitle = "Sample Title";
        c.EnableAllAdvancedOptions();
        //c.EnablePinnableTopbar();
        //c.ShowBackToTopButton();
        //c.EnableStickyOperations();
        //c.EnableExpandOrCollapseAllOperations();
    });
}

app.UseHttpsRedirection();

app.AddEndpoints();
app.MapControllers();

await app.RunAsync();