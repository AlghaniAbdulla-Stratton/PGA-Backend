using Account.API.Models.Profile;
using Account.API.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.Configure<ProfileDatabaseSettings>(
    builder.Configuration.GetSection("DatabaseSettings"));

builder.Services.AddSingleton<AccountService>();
builder.Services.AddHttpClient<HttpService>();

builder.Services.AddControllers().AddJsonOptions(opt =>
{
    opt.JsonSerializerOptions.PropertyNamingPolicy = null;
});
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
