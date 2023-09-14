public static class CatCoder
{
    /// <summary>
    /// Runs the algorithm on all input files in the 'level' directory.
    /// It tests the algorithm on the files with example in the name.
    /// It writes the output to the 'results' directory.
    ///
    /// Your Methode like this:
    /// 
    /// Func<string[], object> func = (input) => YourFunction(input);
    /// CatCoder.Run(func);
    /// 
    /// </summary>
    /// <param name="algorithm"> your methode </param>
    /// 
    /// <returns>Returns a bool it is true when you methode returns the right value</returns>
    public static bool Run(Func<string[], object> algorithm)
    {
        CreateDirectoryIfNotExist("level");
        CreateDirectoryIfNotExist("results");
        CreateDirectoryIfNotExist("test");
        MoveExampleFiles();

        string[] files = Directory.GetFiles(Path.Combine(Directory.GetCurrentDirectory(), "level"), "*.in");

        if (files.Length == 0)
        {
            Console.WriteLine("No input files found in the 'level' directory.");

            return false;
        }

        if (!TestCode(algorithm))
        {
            return false;
        }
            
        int level = int.Parse(files[1][^6].ToString());

        for (int i = 0; i < files.Length; i++)
        {
            string[] content = File.ReadAllLines(files[i]);
            object output = algorithm(content);
            WriteOutput(output.ToString(), level, i + 1);
        }

        return true;
    }

    private static void WriteOutput(string? output, int level, int solutionNumber)
    {
        string outputPath = Path.Combine("results", $"level{level}_{solutionNumber}.out");
        File.WriteAllText(outputPath, output ?? string.Empty);
    }

    private static void MoveExampleFiles()
    {
        string[] files = Directory.GetFiles(Path.Combine(Directory.GetCurrentDirectory(), "level"), "*example*.*");

        if (files.Length == 0)
        {
            return;
        }

        ClearFolder(Path.Combine(Directory.GetCurrentDirectory(), "test"));
        foreach (var file in files)
        {
            string fileName = Path.GetFileName(file);
            string destinationPath = Path.Combine(Directory.GetCurrentDirectory() + "\\test", fileName);

            File.Move(file, destinationPath);
        }
    }

    private static void CreateDirectoryIfNotExist(string name)
    {
        string directoryPath = Path.Combine(Directory.GetCurrentDirectory(), name);

        if (!Directory.Exists(directoryPath))
        {
            Directory.CreateDirectory(directoryPath);
        }
    }

    private static bool TestCode(Func<string[], object> algorithm)
    {
        string[] inputFiles =
            Directory.GetFiles(Path.Combine(Directory.GetCurrentDirectory(), "test"),
                               "*.in");
        string[] outputFiles =
            Directory.GetFiles(Path.Combine(Directory.GetCurrentDirectory(), "test"),
                               "*.out");

        if (inputFiles.Length != outputFiles.Length)
        {
            return false;
        }

        if (inputFiles.Length == 0 | outputFiles.Length == 0)
        {
            return false;
        }

        for (int i = 0; i < inputFiles.Length; i++)
        {
            string[] input = File.ReadAllLines(inputFiles[i]);
            string[] output = File.ReadAllLines(outputFiles[i]);
            object result = algorithm(input);

            if (result.ToString() != output[0])
            {
                return false;
            }
        }

        return true;
    }


    private static void ClearFolder(string path)
    {
        string[] files = Directory.GetFiles(path);
        foreach (var file in files)
        {
            File.Delete(file);
        }
    }
}
