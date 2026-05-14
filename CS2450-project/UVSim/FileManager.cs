namespace UVSIM;

/// <summary>
/// In charge of performing File IO operations.
/// </summary>
public static class FileManager
{
    /// <summary>
    /// Loads a program from the file path.
    /// </summary>
    /// <param name="filepath">Path to program.</param>
    /// <returns>Program as a string.</returns>
    /// <exception cref="Exception">Throws on failure to load program.</exception>
    public static string LoadProgram(string filepath)
    {
        string program;
        try
        {
            // Reads all text from the file at the specified path
            Console.WriteLine(filepath);
            program = File.ReadAllText(filepath);
        }
        catch
        {
            // Throws an exception if the file cannot be read
            throw new Exception("Unable to load program. ");
        }

        return program;
    }

    /// <summary>
    /// Saves a program to filepath.
    /// </summary>
    /// <param name="filepath">Path to file to store the program.</param>
    /// <param name="program">The program to be saved.</param>
    /// <exception cref="Exception">Throws on failure to save program.</exception>
    public static void SaveProgram(string filepath, List<Instruction> program)
    {
        try
        {
            // Ensures the directory exists before attempting to save
            EnsureDirectoryExists(filepath);

            // Serializes the program (converts it to a string) and writes it to the file
            File.WriteAllText(filepath, SerializeProgram(program));
        }
        catch
        {
            // Throws an exception if the file cannot be saved
            throw new Exception("Unable to save program.");
        }
    }

    /// <summary>
    /// Ensures the directory for the given file path exists.
    /// </summary>
    /// <param name="filepath">Path to the file.</param>
    public static void EnsureDirectoryExists(string filepath)
    {
        // Extracts the directory path from the file path
        string? directory = Path.GetDirectoryName(filepath);

        // If the directory does not exist, it is created
        if (!string.IsNullOrEmpty(directory) && !Directory.Exists(directory))
        {
            Directory.CreateDirectory(directory);
        }
    }

    /// <summary>
    /// Validates the file path to ensure it is valid.
    /// </summary>
    /// <param name="filepath">Path to the file.</param>
    /// <returns>True if the file path is valid, otherwise false.</returns>
    public static bool IsValidFilePath(string filepath)
    {
        try
        {
            // Extracts the directory and file name from the path
            string? directory = Path.GetDirectoryName(filepath);
            string fileName = Path.GetFileName(filepath);

            // Checks if the directory or file name is empty
            if (string.IsNullOrWhiteSpace(directory) || string.IsNullOrWhiteSpace(fileName))
            {
                return false;
            }

            // Checks for invalid characters in the file path and file name
            if (filepath.IndexOfAny(Path.GetInvalidPathChars()) >= 0 || fileName.IndexOfAny(Path.GetInvalidFileNameChars()) >= 0)
            {
                return false;
            }

            return true; // File path is valid
        }
        catch
        {
            return false; // If an exception occurs, the path is invalid
        }
    }

    /// <summary>
    /// Converts List<Instruction> program to a string that can be stored.
    /// </summary>
    /// <param name="program">Program to be serialized.</param>
    /// <returns>Serialized program.</returns>
    public static string SerializeProgram(List<Instruction> program)
    {
        StringWriter sw = new();

        for (int i = 0; i < program.Count; i++)
        {
            if (program[i].Value >= 0)
                sw.Write($"+{program[i].Value}");
            else
                sw.Write($"-{Math.Abs(program[i].Value)}");

            if (i < program.Count - 1) // Only add newline if it's not the last instruction
                sw.WriteLine();
        }

        return sw.ToString();
    }
}
