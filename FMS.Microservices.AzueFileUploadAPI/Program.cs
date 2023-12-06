using FMS.Services.AzueFileUploadAPI.Helpers;
using FMS.Services.AzueFileUploadAPI.Repository;
using FMS.Services.AzueFileUploadAPI.Services;
using FMS.Services.AzueFileUploadAPI.Services.IService;
using FMS.Services.AzueFileUploadAPI.Services.Service;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddScoped<IAzureUploadFileService, AzureUploadFileService>();
builder.Services.AddScoped<FileUpload>();
builder.Services.AddScoped<IUploadFile, UploadFile>();
builder.Services.AddScoped<IDrowpdownOptionsService,DrowpdownOptionsService>();
builder.Services.AddScoped<IFilesDataService,FilesDataService>();
builder.Services.AddScoped<IBlobConfigService,BlobConfigService>();
builder.Services.AddScoped<IBlobDownloader,BlobDownloader>();


builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();
app.UseCors(policy => policy.AllowAnyHeader()
                            .AllowAnyMethod()
                            .SetIsOriginAllowed(origin => true)
                            //.WithHeaders(HeaderNames.ContentType)
                            );

// Configure the HTTP request pipeline.
//if (app.Environment.IsDevelopment())
//{
    app.UseSwagger();
    app.UseSwaggerUI();
//}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
