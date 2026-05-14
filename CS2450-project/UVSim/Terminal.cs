using System;
using System.Collections.Generic;
using System.IO;

namespace UVSIM;

public class Terminal
{
    private VirtualMachine vm; // The virtual machine instance
    private List<Instruction> loadedInstructions; // Stores the loaded instructions

    public Terminal()
    {
        vm = new VirtualMachine(); // Initializes the virtual machine
        loadedInstructions = new List<Instruction>(); // Initializes the instruction list
    }

    public void Run()
    {
        while (true)
        {
            // Displays the terminal menu
            Console.WriteLine("\n--- UVSim Terminal ---");
            Console.WriteLine("1. Load Program");
            Console.WriteLine("2. Run Program");
            Console.WriteLine("3. View Memory");
            Console.WriteLine("4. View Accumulator");
            Console.WriteLine("5. View Program Counter");
            Console.WriteLine("6. Step Through Program");
            Console.WriteLine("7. Clear Terminal");
            Console.WriteLine("8. Exit");
            Console.WriteLine("9. Save Program"); // Option to save the program
            Console.WriteLine("10. Edit Instructions"); // Option to edit instructions
            Console.Write("Enter your choice: ");
            string choice = Console.ReadLine();

            // Handles the user's menu selection
            switch (choice)
            {
                case "1":
                    LoadProgram(); // Loads a program from a file
                    break;
                case "2":
                    RunProgram(); // Runs the loaded program
                    break;
                case "3":
                    ViewMemory(); // Displays the memory contents
                    break;
                case "4":
                    ViewAccumulator(); // Displays the accumulator value
                    break;
                case "5":
                    ViewProgramCounter(); // Displays the program counter
                    break;
                case "6":
                    StepThroughProgram(); // Executes one instruction at a time
                    break;
                case "7":
                    ClearTerminal(); // Clears the terminal screen
                    break;
                case "8":
                    return; // Exits the program
                case "9":
                    SaveProgram(); // Saves the loaded program to a file
                    break;
                case "10":
                    EditInstructions(); // Allows the user to edit instructions
                    break;
                default:
                    Console.WriteLine("Invalid choice."); // Handles invalid input
                    break;
            }
        }
    }

    private void LoadProgram()
    {
        Console.Write("Enter file path: ");
        string filepath = Console.ReadLine();

        // Parses the file and loads the instructions into memory
        loadedInstructions = Parser.ParseFile(filepath);
        vm.LoadInstructionsIntoMemory(loadedInstructions);

        Console.WriteLine("Program loaded.");
    }

    private void RunProgram()
    {
        try
        {
            // Executes the program from memory
            vm.RunFromMemory();
        }
        catch (Exception ex)
        {
            // Displays an error message if the program fails
            Console.WriteLine($"Error: {ex.Message}");
        }
    }

    private void ViewMemory()
    {
        int pageSize = 10; // Number of memory locations to display per page
        int currentPage = 0; // Tracks the current page

        while (true)
        {
            Console.Clear();
            Console.WriteLine($"--- Memory Page {currentPage + 1} ---");

            // Calculates the start and end indices for the current page
            int start = currentPage * pageSize;
            int end = Math.Min(start + pageSize, vm.Memory.Length);

            // Displays the memory contents for the current page
            for (int i = start; i < end; i++)
            {
                Console.WriteLine($"Address {i:D2}: {vm.Memory.ReadMemory(i)}");
            }

            // Displays navigation options for paging
            Console.WriteLine($"\nPage {currentPage + 1}/{(vm.Memory.Length + pageSize - 1) / pageSize}");
            Console.WriteLine("N: Next Page | P: Previous Page | Q: Quit");

            // Handles user input for paging
            string choice = Console.ReadLine()?.ToUpper();
            if (choice == "N" && end < vm.Memory.Length) currentPage++;
            else if (choice == "P" && currentPage > 0) currentPage--;
            else if (choice == "Q") break;
        }
    }

    private void ViewAccumulator()
    {
        // Displays the current value of the accumulator
        Console.WriteLine($"Accumulator: {vm.Accumulator}");
    }

    private void ViewProgramCounter()
    {
        // Displays the current value of the program counter
        Console.WriteLine($"Program Counter: {vm.ProgramCounter}");
    }

    private void StepThroughProgram()
    {
        try
        {
            // Executes one instruction and displays the updated state
            vm.RunSingleInstruction();
            Console.WriteLine("Instruction executed.");
            ViewAccumulator();
            ViewProgramCounter();
        }
        catch (Exception ex)
        {
            // Displays an error message if execution fails
            Console.WriteLine($"Error: {ex.Message}");
        }
    }

    private void ClearTerminal()
    {
        // Clears the terminal screen
        Console.Clear();
        Console.WriteLine("Terminal cleared.");
    }

    private void SaveProgram()
    {
        if (loadedInstructions.Count == 0)
        {
            // Displays a message if no program is loaded
            Console.WriteLine("No program loaded to save.");
            return;
        }

        Console.Write("Enter file path to save the program (or press Enter to use default location): ");
        string filepath = Console.ReadLine();

        // Sets a default path if no path is provided
        if (string.IsNullOrWhiteSpace(filepath))
        {
            string defaultPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments), "UVSim", "program.txt");
            filepath = defaultPath;
            Console.WriteLine($"No path provided. Saving to default location: {filepath}");
        }

        // Validates the file path
        if (!FileManager.IsValidFilePath(filepath))
        {
            Console.WriteLine("Invalid file path. Please try again.");
            return;
        }

        try
        {
            // Saves the program to the specified path
            FileManager.EnsureDirectoryExists(filepath);
            FileManager.SaveProgram(filepath, loadedInstructions);
            Console.WriteLine("Program saved successfully.");
        }
        catch (Exception ex)
        {
            // Displays an error message if saving fails
            Console.WriteLine($"Error saving program: {ex.Message}");
        }
    }

    private void EditInstructions()
    {
        if (loadedInstructions.Count == 0)
        {
            // Displays a message if no program is loaded
            Console.WriteLine("No program loaded to edit.");
            return;
        }

        Console.WriteLine("Editing Instructions:");
        for (int i = 0; i < loadedInstructions.Count; i++)
        {
            // Displays the current instructions
            Console.WriteLine($"{i + 1}: {loadedInstructions[i].Value}");
        }

        Console.Write("Enter the instruction number to edit (or 0 to cancel): ");
        if (int.TryParse(Console.ReadLine(), out int index) && index > 0 && index <= loadedInstructions.Count)
        {
            Console.Write("Enter the new instruction value: ");
            string newValue = Console.ReadLine();

            try
            {
                // Updates the selected instruction
                loadedInstructions[index - 1] = new Instruction(newValue);
                Console.WriteLine("Instruction updated successfully.");
            }
            catch (Exception ex)
            {
                // Displays an error message if the update fails
                Console.WriteLine($"Error updating instruction: {ex.Message}");
            }
        }
        else
        {
            // Displays a message for invalid input
            Console.WriteLine("Invalid input. Returning to menu.");
        }
    }
}
