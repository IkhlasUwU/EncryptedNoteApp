using static Defaults;

await Encryption.Decrypt();

while (true)
{
   Config.LoadConfig();
   Console.Clear();
   Easy.Print("Please select an option:\n1. Choose book\n2. Delete books/notes\n3. Edit config\n4. Exit");
   string? input = Console.ReadLine()?.Trim().ToLower();
   switch(input)
    {
        case "1":
        ChooseBook();
        break;
        case "2":
        Delete();
        break;
        case "3":
        Bash.OpenNote(configFile);
        break;
        case "4":
        await Encryption.Encrypt();
        return;
        case "x":
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

    books = Directory.EnumerateDirectories(booksPath).ToList();
    if (books.Count == 0)
    {return;}

    while(true)
    {
        books = Directory.EnumerateDirectories(booksPath).ToList();
        counter = 1;
        
        Console.Clear();
        foreach (string book in books)
        {
            Easy.Print($"{counter}: {Path.GetFileName(books[counter-1])}");
            counter++;
        }

        Easy.Print("Select which book you want to write in: (N for new book)");
        input = Console.ReadLine()?.Trim().ToLower();

        if (int.TryParse(input, out int output))
        {
            workingPath = books[output - 1] + "/";
            EditNote();
        }
        if (input == "n")
        {
            Easy.Print("What would you like to name your book?");
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
        else
        {
            return;
        }
    }
}

static void EditNote()
{
    while(true)
    {
        int counter = 1;
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
                Bash.OpenNote(workingPath + input);
            }
            catch (Exception e)
            {
                Easy.Error(e.Message);
            }
        }
        notes = Directory.EnumerateFiles(workingPath).ToList();
        if (notes.Count == 0)
        {return;}

        Console.Clear();
        foreach (string note in notes)
        {
            Easy.Print($"{counter}: {Path.GetFileName(notes[counter-1])}");
            counter++;
        }

        Easy.Print("Select which note you want to edit: (N for new note, X to exit)");
        input = Console.ReadLine()?.Trim().ToLower();

        if (int.TryParse(input, out int output))
        {
            Bash.OpenNote(notes[output - 1]);
        }
        else if (input == "n")
        {
            Console.Clear();
            Easy.Print($"Please enter name of new note:", false);
            input = Console.ReadLine();
            Bash.OpenNote(workingPath + input);
        }
        else if (input == "x")
        {
            return;
        }
        else
        {
            Console.Clear();
            Easy.Warn($"Please enter a number 1 - {notes.Count}");
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