using System.Diagnostics;
using static Defaults;

public static class Bash
{
    public static void OpenNote(string path)
    {
        string editor = Environment.GetEnvironmentVariable("EDITOR") ?? "nano";
        ProcessStartInfo pInfo = new ProcessStartInfo
        {
            FileName = editor,
            Arguments = path,
            UseShellExecute = true,
        };

        using (Process? p = Process.Start(pInfo)){p?.WaitForExit();}
    }

public static void Unlock()
    {
        ProcessStartInfo pInfo = new ProcessStartInfo
        {
            FileName = "bash",
            RedirectStandardInput = true,
            RedirectStandardOutput = true,
            RedirectStandardError = true,
            UseShellExecute = false,
            CreateNoWindow = true
        };

        using (Process p = new Process {StartInfo = pInfo})
        {
            p.Start();

            using (var w = p.StandardInput)
            {
                w.WriteLine($"gpg -d {encryptedPath}locked.gpg > {notesPath}locked.tar.gz");
                w.WriteLine("exit");
            }

            Easy.Print(p.StandardOutput.ReadToEnd(), false);

            p.WaitForExit();
        }
    }
public static void UnpackFiles()
{
ProcessStartInfo pInfo = new ProcessStartInfo
        {
            FileName = "bash",
            RedirectStandardInput = true,
            RedirectStandardOutput = true,
            RedirectStandardError = true,
            UseShellExecute = false,
            CreateNoWindow = true
        };

        using (Process p = new Process {StartInfo = pInfo})
        {
            p.Start();

            using (var w = p.StandardInput)
            {
                w.WriteLine($"cd {notesPath}");
                w.WriteLine("tar -xf locked.tar.gz");
                w.WriteLine("rm locked.tar.gz");
                w.WriteLine("exit");
            }

            Easy.Print(p.StandardOutput.ReadToEnd(), false);

            p.WaitForExit();
        }
    }

public static void PackFiles()
{
    string archiveName = "locked.tar.gz";

    var books = Directory.GetDirectories(booksPath).ToList();
    
    if (books.Count == 0)
    {
        Easy.Warn("No files found to pack. Skipping tar.", false);
        return;
    }
    else
    {
        ProcessStartInfo pInfo = new ProcessStartInfo
            {
                FileName = "bash",
                RedirectStandardInput = true,
                RedirectStandardOutput = true,
                RedirectStandardError = true,
                UseShellExecute = false,
                CreateNoWindow = true
            };

            using (Process p = new Process {StartInfo = pInfo})
            {
                p.Start();

                using (var w = p.StandardInput)
                {
                    w.WriteLine($"cd {notesPath}");
                    w.WriteLine("tar --remove-files -czf locked.tar.gz *");
                    w.WriteLine("exit");
                }

                Easy.Print(p.StandardOutput.ReadToEnd(), false);

                p.WaitForExit();
            }
    }
}
}