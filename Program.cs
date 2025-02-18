using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
var builder = WebApplication.CreateBuilder(args);

// Добавляем поддержку контроллеров (API)
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();

var app = builder.Build();


app.UseRouting();
app.UseAuthorization();
app.MapControllers(); // Подключаем контроллеры

app.Run(); // Запускаем сервер
