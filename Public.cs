using static Defaults;
using System.Text.Json;
using Microsoft.Extensions.Configuration;
using Spectre.Console;
public static class Defaults
{
    public static string user = Environment.UserName;
    public static  string notesPath = $"/tmp/notes/";
    public static string booksPath = $"{notesPath}books/";
    public static string encryptedPath = $"/home/{user}/notes/";
    public static string keyPath = encryptedPath;
    public static string configPath = $"/home/{user}/.config/notesConfig/";
    public static string configFile  = $"{configPath}notes.json";
    public static string workingPath = notesPath;
    public static string textColorString = "#45FF66";
    public static Style textColor = new (Color.FromHex(textColorString));
}

public static class Config
{
    public static void LoadConfig()
    {
        CheckConfig();
        LoadColor();
        LoadPath();
    }
    public static void CheckConfig()
    {
        if (!Directory.Exists(configPath))
        {
            Directory.CreateDirectory(configPath);
        }
        if (!File.Exists(configFile))
        {
            var defaultSettings = new
            {
                Settings = new
                {
                    TextColor = textColorString,
                    NotesPath = notesPath,
                    EncryptedPath = encryptedPath
                }
            };

            byte[] json = JsonSerializer.SerializeToUtf8Bytes(defaultSettings, new JsonSerializerOptions { WriteIndented = true });
            File.WriteAllBytes(configFile, json);
        }
    }

    public static void LoadColor()
    {
        IConfiguration config = new ConfigurationBuilder()
            .SetBasePath(configPath)
            .AddJsonFile("notes.json")
            .Build();

        string? hexColor = config["Settings:TextColor"];

        try
        {
            textColor = Color.FromHex(hexColor);
        }
        catch (Exception)
        {
            textColor = textColorString;
        }
    }

    public static void LoadPath()
    {
        IConfiguration config = new ConfigurationBuilder()
            .SetBasePath(configPath)
            .AddJsonFile("notes.json")
            .Build();

        string? path = config["Settings:NotesPath"];

        if (!Directory.Exists(path))
        {
            try
                {
                    Directory.CreateDirectory(path);
                }
            catch (Exception e)
                {
                    Easy.Error(e.Message);
                }
            
        }
        if (Directory.Exists(path))
        {
            notesPath = path;
            booksPath = $"{notesPath}books/";
        }

        LoadEncryptedPath();
        CheckBooks();
    }

    public static void LoadEncryptedPath()
    {
         IConfiguration config = new ConfigurationBuilder()
            .SetBasePath(configPath)
            .AddJsonFile("notes.json")
            .Build();

        string? path = config["Settings:EncryptedPath"];

        if (!Directory.Exists(path))
        {
            try
                {
                    Directory.CreateDirectory(path);
                }
            catch (Exception e)
                {
                    Easy.Error(e.Message);
                }
        }

        if (Directory.Exists(path))
        {
            encryptedPath = path;
        }
    }

    public static void CheckBooks()
    {
        if (!Directory.Exists(booksPath))
        {
            Directory.CreateDirectory(booksPath);
        }
    }
}