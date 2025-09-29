using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;


// Interfaces
public interface IEmailService
{
    void SendEmail(string to, string message);
}

public interface IUserService
{
    void CreateUser(string name, string email);
}

public interface ILoggerService
{
    void Log(string message);
}

// Implementations
public class EmailService : IEmailService
{
    private readonly ILoggerService _logger;

    public EmailService(ILoggerService logger)
    {
        _logger = logger;
    }

    public void SendEmail(string to, string message)
    {
        _logger.Log($"Sending email to {to}");
        //Console.WriteLine($"📧 Email sent to {to}: {message}");
    }
}

public class UserService : IUserService
{
    private readonly IEmailService _emailService;
    private readonly ILoggerService _logger;

    public UserService(IEmailService emailService, ILoggerService logger)
    {
        _emailService = emailService;
        _logger = logger;
    }

    public void CreateUser(string name, string email)
    {
        _logger.Log($"Creating user: {name}");
        //Console.WriteLine($"👤 User created: {name}");
        _emailService.SendEmail(email, $"Welcome {name}!");
    }
}

public class ConsoleLogger : ILoggerService
{
    public void Log(string message)
    {
        Console.WriteLine($"🔍 LOG: {message}");
    }
}

public class FileLogger : ILoggerService
{
    FileStream logFile = new FileStream("log.txt", FileMode.Append, FileAccess.Write);
    public void Log(string message)
    {
        using (var writer = new StreamWriter(logFile, leaveOpen: true))
        {
            writer.WriteLine($"🔍 LOG: {message}");
        }
    }
    ~FileLogger()
    {
        logFile.Close();
    }
}
public class Application
{
    private readonly IUserService _userService;

    public Application(IUserService userService)
    {
        _userService = userService;
    }

    public void Run()
    {
        Console.WriteLine("=== Dependency Injection Demo ===");
        _userService.CreateUser("Alice", "alice@example.com");
        _userService.CreateUser("Bob", "bob@example.com");
    }
}

class DIProgram
{
    public static async Task RunDIDemo()
    {
        // Setup DI Container
        var host = Host.CreateDefaultBuilder()
            .ConfigureServices((context, services) =>
            {
                services.AddSingleton<ILoggerService, FileLogger>();
                //if enabled then it will hide the file logger services.AddSingleton<ILoggerService, ConsoleLogger>();
                services.AddTransient<IEmailService, EmailService>();
                services.AddScoped<IUserService, UserService>();
                services.AddTransient<Application>();
            })
            .Build();

        var app = host.Services.GetRequiredService<Application>();
        app.Run();
    }
}