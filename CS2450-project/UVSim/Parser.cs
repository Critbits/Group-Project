namespace UVSIM;

/// <summary>
/// Parse and validate ML instructions and store them in the instructions variable.
/// </summary>
public static class Parser
{
    /// <summary>
    /// Load the file located at filepath and return a program.
    /// </summary>
    public static List<Instruction> ParseFile(string filepath)
    {
        string program = FileManager.LoadProgram(filepath);
        return ParseString(program);
    }

    /// <summary>
    /// Parse a string into a program.
    /// </summary>
    public static List<Instruction> ParseString(string program)
    {
        
        List<Instruction> instructions = new();
        using (StringReader sr = new(program))
        {
            string? line = sr.ReadLine();
            ParseMode mode = GetParseMode(line);
            while (line != null)
            {
                if (mode == ParseMode.Short)
                {
                    if (ValidateInstructionShort(line)) {
                        instructions.Add(new Instruction(line));
                    }
                } else if (mode == ParseMode.Long)
                {
                    if (ValidateInstructionLong(line)) {
                        instructions.Add(new Instruction(line));
                    }
                }
                else
                {
                    throw new Exception("Invalid instruction: " + line);
                }
                line = sr.ReadLine();
            }
        }

        return instructions;
    }

    /// <summary>
    /// Validates that the instruction is of format [+-]1234.
    /// </summary>
    /// <param name="instruction">Line read from file to be validated.</param>
    /// <returns>True if the instruction is valid.</returns>
    public static bool ValidateInstructionShort(string instruction)
    {
        if (instruction.Length != 5) return false;
        if (instruction[0] != '+' && instruction[0] != '-') return false;
        if (!int.TryParse(instruction.AsSpan(1), out int j)) return false;
        return true;
    }

    public static bool ValidateInstructionLong(string instruction)
    {
        if (instruction.Length != 7) return false;
        if (instruction[0] != '+' && instruction[0] != '-') return false;
        if (!int.TryParse(instruction.AsSpan(1), out int j)) return false;
        return true;
    }

    private static ParseMode GetParseMode(string? instruction)
    {
        if (instruction == null) return ParseMode.Error;
        if (instruction.Length ==  5) return ParseMode.Short;
        if (instruction.Length == 7) return ParseMode.Long;
        return ParseMode.Error;
    }
}

enum ParseMode
{
    Short,
    Long,
    Error,
}