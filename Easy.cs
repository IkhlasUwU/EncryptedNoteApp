using Spectre.Console;
public static class Easy
{
     public static void Print(string input)
    {
        AnsiConsole.WriteLine(input);
    }

      public static void Print(string input, bool a)
    {
        AnsiConsole.WriteLine(input);
        if (a)
        {Console.ReadLine();}
    }

    public static void Warn(string input)
    {
        var warn = new Style(Color.FromHex("#F79E23"), decoration: Decoration.Bold);
        var message = new Text($"{input}\n", warn);
        AnsiConsole.Write(message);
        Console.ReadLine();
    }

    public static void Warn(string input, bool a)
    {
        var warn = new Style(Color.FromHex("#F79E23"), decoration: Decoration.Bold);
        var message = new Text($"{input}\n", warn);
        AnsiConsole.Write(message);
        if (a)
        {Console.ReadLine();}
    }

    public static void Success(string input)
    {
        var success = new Style(Color.FromHex("#3AF035"), decoration: Decoration.Bold);
        var message = new Text($"{input}\n", success);
        AnsiConsole.Write(message);
        Console.ReadLine();
    }

    public static void Success(string input, bool a)
    {
        var success = new Style(Color.FromHex("#3AF035"), decoration: Decoration.Bold);
        var message = new Text($"{input}\n", success);
        AnsiConsole.Write(message);
        if (a)
        {Console.ReadLine();}
    }

    public static void Error(string input)
    {
        var error = new Style(Color.FromHex("#ED1818"), decoration: Decoration.Bold);
        var message = new Text($"{input}\n", error);
        AnsiConsole.Write(message);
        Console.ReadLine();
    }

     public static void Error(string input, bool a)
    {
        var error = new Style(Color.FromHex("#ED1818"), decoration: Decoration.Bold);
        var message = new Text($"{input}\n", error);
        AnsiConsole.Write(message);

        if (a)
        {Console.ReadLine();}
    }
}
