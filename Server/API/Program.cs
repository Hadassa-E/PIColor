using BLL;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
builder.Services.AddControllers().AddJsonOptions(o => o.JsonSerializerOptions.PropertyNamingPolicy = null);
builder.Services.AddCors(opotion => opotion.AddPolicy("AllowAll",
                p => p.AllowAnyOrigin()
                .AllowAnyMethod()
                .AllowAnyHeader()));
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

try
{
    //builder.Services.AddScoped(typeof(IopenPicture), typeof(OpenPicture));
    builder.Services.AddScoped(typeof(IprocessPictureBLL), typeof(ProcessPictureBLL));
}
catch (Exception e) { }
finally
{
    var app = builder.Build();

    // Configure the HTTP request pipeline.
    if (app.Environment.IsDevelopment())
    {
        app.UseSwagger();
        app.UseSwaggerUI();
    }
    app.UseCors("AllowAll");

    app.UseHttpsRedirection();

    app.UseAuthorization();

    app.MapControllers();

    app.Run();
}