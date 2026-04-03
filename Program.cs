using static Defaults;
using Spectre.Console;

await Encryption.Decrypt();

while (true)
{
    string[] menuOptions = ["Choose book", "Delete book/note", "Edit config", "Exit"];
    Config.LoadConfig();
    Console.Clear();
    string input = AnsiConsole.Prompt(
        new SelectionPrompt<string>()
        .Title($"Select an option:")
        .PageSize(4)
        .AddChoices(menuOptions));

   switch(input)
    {
        case "Choose book":
        ChooseBook();
        break;
        case "Delete book/note":
        Delete();
        break;
        case "Edit config":
        Bash.OpenNote(configFile);
        break;
        case "Exit":
        await Encryption.Encrypt();
        return;
    }
}

static void ChooseBook()
{
    List<string>? books = Directory.EnumerateDirectories(booksPath).ToList();
    int counter = 1;
    string? input = null;

    if (books.Count == 0)
    {
        Easy.Print("There are no books, please enter the name of your first book:");
        input = Console.ReadLine();

        try
        {
            Directory.CreateDirectory(booksPath + input);
        }
        catch (Exception e)
        {
            Easy.Error(e.Message);
        }
    }

     if (books.Count == 0)
        {return;}

    books = Directory.EnumerateDirectories(booksPath).ToList();

    for (int i = 0; i < books.Count; i++)
    {
        books[i] = Path.GetFileName(books[i]);
    }

    Console.Clear();

   chosenBook = AnsiConsole.Prompt(
            new SelectionPrompt<string>()
            .Title($"Choose a book to write in")
            .PageSize(25)
            .WrapAround()
            .EnableSearch()
            .SearchPlaceholderText("type to search books")
            .AddChoices(books)
            .AddChoices("New book")
            .AddChoices("Exit"));

    if (chosenBook == "Exit") {return;}

    if (chosenBook == "New book")
    {
        Easy.Print("Please enter book name:");
        input = Console.ReadLine();

        try
        {
            Directory.CreateDirectory(booksPath + input);
        }
        catch (Exception e)
        {
            Easy.Error(e.Message);
        }

        chosenBook = input;
    }

    workingPath = booksPath + chosenBook;

    EditNote();
}

static void EditNote()
{
    while(true)
    {
        string? input = null;
        List<string>? notes = Directory.EnumerateFiles(workingPath).ToList();
        Console.Clear();

        if (notes.Count == 0)
        {
            var dirInfo = new DirectoryInfo(workingPath);
            string directory = dirInfo.Name;
            Easy.Print($"No notes in {directory}, please enter name of new note:");
            input = Console.ReadLine();

            try
            {
                Bash.OpenNote(workingPath + "/" + input);
            }
            catch (Exception e)
            {
                Easy.Error(e.Message);
            }
        }
        notes = Directory.EnumerateFiles(workingPath).ToList();

    for (int i = 0; i < notes.Count; i++)
    {
        notes[i] = Path.GetFileName(notes[i]);
    }
        if (notes.Count == 0)
        {return;}

        input = AnsiConsole.Prompt(
            new SelectionPrompt<string>()
            .Title($"Choose a note to write")
            .PageSize(25)
            .WrapAround()
            .EnableSearch()
            .SearchPlaceholderText("type to search notes")
            .AddChoices(notes)
            .AddChoices("New note")
            .AddChoices("Exit"));

        if (input == "Exit"){return;}

        if (input == "New note")
        {
            Easy.Print("Please enter note name:");
            input = Console.ReadLine();
        }

        try
        {
            Bash.OpenNote(workingPath + "/" + input);
        }
        catch (Exception e)
        {
            Easy.Error(e.Message);
        }
        
    }
}

static void Delete()
{
    while (true)
    {
        int counter = 1;
        Console.Clear();
        Easy.Print("Would you like to delete a book, or a note? (B/N) or X to exit");
        string? input = Console.ReadLine()?.Trim().ToLower();

        switch(input)
        {
            case "b":
                Console.Clear();
                List<string>? books = Directory.EnumerateDirectories(booksPath).ToList();

                if (books.Count > 0)
                {
                    foreach (string book in books)
                        {
                        Easy.Print($"{counter}: {Path.GetFileName(books[counter-1])}");
                        counter++;  
                        }
                    Easy.Print("Select which book to delete:");
                    input = Console.ReadLine();

                    if (int.TryParse(input, out int output))
                        {
                            Directory.Delete(books[output - 1], true);
                            Easy.Success($"{Path.GetFileName(books[output - 1])} has been deleted");
                        }
                    else
                        {
                            Console.Clear();
                            Easy.Warn($"Please enter a number 1 - {books.Count}");
                        }
                }
                else
                {
                    Easy.Warn("There are no books");
                }
                    break;
            case "n":
                string? selectedBook = null;
                List<string>? books2 = Directory.EnumerateDirectories(booksPath).ToList();
                if (books2.Count > 0)
                {
                    foreach (string book in books2)
                        {
                        Easy.Print($"{counter}: {Path.GetFileName(books2[counter-1])}");
                        counter++;  
                        }
                    Easy.Print("Select which book note is in:");
                    input = Console.ReadLine();
                    if (int.TryParse(input, out int output))
                        {
                            selectedBook = booksPath + Path.GetFileName(books2[output - 1]) + "/";
                        }
                    else
                        {
                            Console.Clear();
                            Easy.Warn($"Please enter a number 1 - {books2.Count}");
                        }
                    if (selectedBook != null)
                    {
                        List<string>? notes = Directory.EnumerateFiles(selectedBook).ToList();
                        counter = 1;
                        foreach (string note in notes)
                        {
                            Easy.Print($"{counter}: {Path.GetFileName(notes[counter - 1])}");
                            counter++;
                        }
                        Easy.Print("Select which note to delete");
                        input = Console.ReadLine();

                        if (int.TryParse(input, out output))
                        {
                            string noteToDelete = selectedBook + Path.GetFileName(notes[output - 1]);
                            File.Delete(noteToDelete);
                            Easy.Success($"{noteToDelete} has been deleted!");
                        }
                    }
                }
                else
                {
                    Easy.Warn("There are no books:");
                }
                break;
            case "x":
            return;
        }
    }
}