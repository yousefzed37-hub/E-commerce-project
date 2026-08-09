using Microsoft.EntityFrameworkCore;
using E_commerce_project.DataContext;

namespace E_commerce_project
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            builder.Services.AddControllersWithViews();

            builder.Services.AddDbContext<AppDbContext>(options =>
            options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();
            builder.Services.AddCors(options =>
            {
                options.AddPolicy("AllowAll", builder =>
                {
                    builder.AllowAnyOrigin()   // يسمح لأي موقع بالاتصال بالـ API
                           .AllowAnyMethod()   // يسمح بـ GET, POST, PUT, DELETE
                           .AllowAnyHeader();  // يسمح بأي Headers
                });
            });
            var app = builder.Build();

            // Configure the HTTP request pipeline.


            app.UseSwagger();
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "E-Commerce API V1");
                c.RoutePrefix = string.Empty; // بيخلي الصفحة الرئيسية تفتح Swagger فوراً!
            });
            app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            

            app.UseHttpsRedirection();
            app.UseRouting();
            app.UseCors("AllowAll"); // 👈 ضيف السطر ده هنا

            app.UseAuthorization();

            app.MapStaticAssets();
            app.MapControllerRoute(
                name: "default",
                pattern: "{controller=Home}/{action=Index}/{id?}")
                .WithStaticAssets();

            app.Run();
        }
    }
}
