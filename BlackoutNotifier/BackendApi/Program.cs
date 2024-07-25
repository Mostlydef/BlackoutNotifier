namespace BackendAPI
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);
            var app = builder.Build();
            
            app.MapPost("/data", async (HttpContext httpContext) =>
            {
                using StreamReader reader = new StreamReader(httpContext.Request.Body);
                string name = await reader.ReadToEndAsync();
                Console.WriteLine(name);
                return $"Полученные данные: {name}";
            });

            app.Run();
        }
    }
}
