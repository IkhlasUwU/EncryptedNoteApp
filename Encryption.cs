using PgpCore;
using static Defaults;
public class Encryption
{
    public static async Task Encrypt()
    {
        Bash.PackFiles();

        if (File.Exists(encryptedPath + "locked.gpg") && !File.Exists(notesPath + "locked.tar.gz"))
            {
                Easy.Error("Are there no notes? if so, hit enter. If not, type x");
                string? input = Console.ReadLine()?.Trim().ToLower();
                switch (input)
                {
                    case "x":
                    break;
                    default:
                    File.Delete(encryptedPath + "locked.gpg");
                    break;
                }
            }

        List<string> files = Directory.EnumerateFiles(notesPath).ToList();


        FileInfo publicKey = new FileInfo($"{keyPath}publicKey");
        EncryptionKeys encryptionKeys = new EncryptionKeys(publicKey);

        using (PGP pgp = new PGP(encryptionKeys))
        {
            foreach (string file in files)
            {
                FileInfo input = new(file);
                FileInfo output = new(encryptedPath + "locked" + ".gpg");

                await pgp.EncryptAsync(input, output, armor: false);
            }
        }

        File.Delete(notesPath + "locked.tar.gz");
    }

    public static async Task Decrypt()
    {
        Config.LoadConfig();
        Bash.Unlock();
        Bash.UnpackFiles();
    }
}