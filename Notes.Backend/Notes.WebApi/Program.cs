using MediatR;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Mvc.ApiExplorer;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Notes.Application;
using Notes.Application.Commands.CreateNote;
using Notes.Application.Commands.DeleteNote;
using Notes.Application.Commands.UpdateNote;
using Notes.Application.Common.Mapping;
using Notes.Application.Queries.GetNoteDetails;
using Notes.Application.Queries.GetNoteList;
using Notes.Domain;
using Notes.WebApi;
using Notes.WebApi.Middleware;
using Notes.WebApi.PersistanceLevel;
using Swashbuckle.AspNetCore.SwaggerGen;
using System.Reflection;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddDbContext<NotesDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("ExpressConnection")));

builder.Services.AddTransient<IRequestHandler<CreateNoteCommand, Guid>, CreateNoteCommandHandler>();
builder.Services.AddTransient<IRequestHandler<DeleteNoteCommand>, DeleteNoteCommandHandler>();
builder.Services.AddTransient<IRequestHandler<UpdateNoteCommand>, UpdateNoteCommandHandler>();

builder.Services.AddTransient<IRequestHandler<GetNoteDetailsQuery, NoteDetailsVm>, GetNoteDetailsQueryHandler>();

builder.Services.AddAutoMapper(config =>
{
    config.AddProfile(new AssemblyMappingProfile(Assembly.GetExecutingAssembly()));
    config.AddProfile(new AssemblyMappingProfile(typeof(NotesDbContext).Assembly));
    config.CreateMap<Note, NoteLookUpDto>();
    config.CreateMap<Note, NoteDetailsVm>();

});

builder.Services.AddApplication(); 
var configuration = builder.Configuration;
builder.Services.AddPersistance(configuration);
builder.Services.AddControllers();
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll", policy =>
    {
        policy.AllowAnyHeader();
        policy.AllowAnyMethod();
        policy.AllowAnyOrigin();
    });
});
builder.Services.AddAuthentication(config =>
{
    config.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    config.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;

})
    .AddJwtBearer("Bearer", options =>
    {
        options.Authority = "https://localhost:44300/";
        options.Audience = "NotesWebAPI";
        options.RequireHttpsMetadata = false;
    });
builder.Services.AddVersionedApiExplorer(options =>
    options.GroupNameFormat = "'v'VVV");
builder.Services.AddTransient<IConfigureOptions<SwaggerGenOptions>, ConfigureSwaggerOptions>();
builder.Services.AddSwaggerGen();
// builder.Services.AddScoped<IMediator, Mediator>();
builder.Services.AddScoped<IRequestHandler<GetNoteListQuery, NoteListVm>, GetNoteListQueryHandler>();
builder.Services.AddApiVersioning();
var app = builder.Build();
if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
}
using (var scope = app.Services.CreateScope())
{
    var serviceProvider = scope.ServiceProvider;
    try
    {
        var dbContext = serviceProvider.GetRequiredService<NotesDbContext>();
        dbContext.Database.EnsureCreated();
    }
    catch (Exception ex)
    {
        Console.WriteLine(ex.Message.ToString());
    }
}
app.UseSwagger();
app.UseSwaggerUI(cfg =>
{
    var serviceProvider = builder.Services.BuildServiceProvider();
    var apiVersionDescriptionProvider = serviceProvider.GetRequiredService<IApiVersionDescriptionProvider>();
    foreach (var description in
        apiVersionDescriptionProvider.ApiVersionDescriptions)
    {
        cfg.SwaggerEndpoint($"/swagger/{description.GroupName}/swagger.json",
            description.GroupName.ToUpperInvariant());
        cfg.RoutePrefix = string.Empty;
    }
});
app.UseCustomExceptionHandler();
app.UseRouting();
app.UseHttpsRedirection(); 
app.UseCors("AllowAll");
app.UseAuthentication();
app.UseAuthorization(); 
app.UseApiVersioning();
app.UseEndpoints( endpoints =>
{
    endpoints.MapControllers();
});
app.Run();
