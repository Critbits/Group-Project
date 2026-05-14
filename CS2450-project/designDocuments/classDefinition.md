Class Definition Document: UVSIM Simulator 

Project Overview 

This project is a UVSIM (Universal Virtual Simple Machine) simulator. The simulator executes instructions written in a custom assembly language. The code includes components for file I/O, parsing instructions, managing memory, running the virtual machine, and a terminal interface for user interaction. The class design is modular and loosely coupled, ensuring the separation of user interface, data model, and business logic. 

 

Class Definitions 

Classes in UVSim Folder:

1. FileManager (Static Class) 

Purpose: Handles file input and output operations for loading and saving programs. 

Functions: 

LoadProgram(string filepath) 

Purpose: Loads a program from the specified file path. 

Parameters: 

filepath (string): Path to the file containing the program. 

Return Value: (string) Program content as a string. 

Pre-condition: File at filepath exists and is readable. File contains text-based UVML instructions, each on a new line. 

Post-condition: Program content is loaded into a string. Throws an Exception if the file cannot be read. 

SaveProgram(string filepath, List<Instruction> program) 

Purpose: Saves a program (list of instructions) to the specified file path. 

Parameters: 

filepath (string): Path to the file where the program will be saved. 

program (List): List of instructions to save. 

Return Value: None (void). 

Pre-condition: program list is valid (not null). Application has write permissions to filepath. 

Post-condition: Program is saved to the file. Throws an Exception if the file cannot be written to. 

SerializeProgram(List<Instruction> program) 

Purpose: Converts a list of Instruction objects into a string representation suitable for saving to a file. 

Parameters: 

program (List): List of instructions to serialize. 

Return Value: (string) Serialized program as a string. 

Pre-condition: program list is valid (not null). 

Post-condition: Program is converted into a string format, with each instruction's Value on a new line. 

 

2. InstructionType (Enum) 

Purpose: Defines the possible opcodes for the UVML instructions. 

Values: 

VALUE, READ, WRITE, LOAD, STORE, ADD, SUBTRACT, DIVIDE, MULTIPLY, BRANCH, BRANCHNEG, BRANCHZERO, HALT 

 

3. Instruction (Class) 

Purpose: Represents a single UVML instruction. 

Fields: 

Value (int): Numerical value of the instruction. 

Type (InstructionType): Type of instruction (opcode). 

Constructor: 

Instruction(string instruction) 

Purpose: Creates an Instruction object from a string representation. 

Parameters: 

instruction (string): String representation of the instruction (e.g., "+100001"). 

Return Value: None (constructor). 

Pre-condition: instruction string is in the correct format ([+-] followed by 6 digits). 

Post-condition: Instruction object is created with Value and Type set based on the input string. If the string is invalid, Type defaults to InstructionType.VALUE. 

 

4. Memory (Class) 

Purpose: Represents the memory of the virtual machine. 

Fields: 

Locations (ObservableCollection): Collection of memory locations. 

Length (int): Size of the memory. 

Constructors: 

Memory(int size) 

Purpose: Creates a Memory object with the specified size, initializing all locations to 0. 

Parameters: 

size (int): Number of memory locations. 

Return Value: None (constructor). 

Pre-condition: size is a non-negative integer. 

Post-condition: Memory object is created with size locations, each initialized to 0. 

Memory(int[] memory) 

Purpose: Creates a Memory object initialized with values from an integer array. 

Parameters: 

memory (int[]): Array of integers to initialize the memory with. 

Return Value: None (constructor). 

Pre-condition: memory array is valid (not null). 

Post-condition: Memory object is created, initialized with values from the array. 

Functions: 

ReadMemory(int address) 

Purpose: Reads the value stored at the specified memory address. 

Parameters: 

address (int): Memory address to read from. 

Return Value: (int) Value stored at the address. 

Pre-condition: address is within bounds (0 <= address < Length). 

Post-condition: Returns the value at the address. Throws IndexOutOfRangeException if address is invalid. 

WriteMemory(int address, int value) 

Purpose: Writes a value to the specified memory address. 

Parameters: 

address (int): Memory address to write to. 

value (int): Value to write. 

Return Value: None (void). 

Pre-condition: address is within bounds (0 <= address < Length). 

Post-condition: Updates the value at the address. Throws IndexOutOfRangeException if address is invalid. 

 

5. Memory.MemoryLocation (Nested Class) 

Purpose: Represents a single memory location. 

Fields: 

Data (int): Data stored at the memory location. 

Index (int): Index of the memory location. 

Constructor: 

MemoryLocation(int data, int index) 

Purpose: Creates a MemoryLocation object. 

Parameters: 

data (int): Initial data to store. 

index (int): Index of the location. 

Return Value: None (constructor). 

Pre-condition: index is valid within the bounds of the Memory class. 

Post-condition: MemoryLocation object is created with specified data and index. 

 

6. Parser (Static Class) 

Purpose: Parses UVML instructions from a file or string. 

Functions: 

ParseFile(string filepath) 

Purpose: Parses instructions from a file. 

Parameters: 

filepath (string): Path to the file containing the program. 

Return Value: (List) List of Instruction objects parsed from the file. 

Pre-condition: File at filepath exists and contains valid instructions. 

Post-condition: Returns a list of Instruction objects. Returns an empty list if the file is empty or invalid. 

ParseString(string program) 

Purpose: Parses instructions from a string. 

Parameters: 

program (string): String containing the program. 

Return Value: (List) List of Instruction objects parsed from the string. 

Pre-condition: program string contains valid instructions. 

Post-condition: Returns a list of Instruction objects. Returns an empty list if the string is empty or invalid. 

ValidateInstruction(string instruction) 

Purpose: Validates an instruction string. 

Parameters: 

instruction (string): Instruction string to validate. 

Return Value: (bool) True if the instruction is valid, false otherwise. 

Pre-condition: instruction is not null or empty. 

Post-condition: Returns true if the instruction matches the format "[+-]dddddd". 

 

7. VirtualMachine (Class) 

Purpose: Simulates the virtual machine, including memory, accumulator, and program counter. 

Fields: 

Accumulator (int): Accumulator register. 

Memory (Memory): Memory of the virtual machine. 

ProgramCounter (int): Program counter. 

Functions: 

RunFromMemory() 

Purpose: Executes instructions in memory. 

Pre-condition: Instructions are loaded into memory. 

Post-condition: Executes instructions until HALT or end of memory. 

 

8. Terminal (Class) 

Purpose: Provides a terminal-based user interface for interacting with the UVSIM. Intended for command-line usage and debugging.

Functions: 

Run() 

Purpose: Starts the main loop for user interaction. 

 
Classes in UVSimUI Folder:

1. public partial class AppShell : Shell

Purpose: Represents the main application shell for the UVSIM user interface.

Functions:

AppShell()

Purpose: Initializes the application shell.

AddNewTab()

Purpose: Adds a new tab to the shell.

2. public partial class MainPage : ContentPage

Purpose: Represents the main page of the UVSIM user interface.

Functions:

MainPage()

Purpose: Initializes the main page.

InitializeComponent()

Purpose: Initializes the components of the main page.

3. public partial class App : Application

Purpose: Represents the main application class for the Main Maui Window.

Functions:

App()

Purpose: Initializes the application.

protected override Window CreateWindow(IActivationState? activationState)

Purpose: Creates the main window for the application.

4. public class AppTheme : BindableObject

Purpose: Represents the application's color theme for the UVSIM user interface.

Fields:

public static readonly BindableProperty PrimaryColorProperty = BindableProperty.Create(nameof(PrimaryColor), typeof(Color), typeof(AppTheme), new Color(76, 114, 29));

Purpose: Represents the primary color of the application theme.

public static readonly BindableProperty SecondaryColorProperty = BindableProperty.Create(nameof(SecondaryColor), typeof(Color), typeof(AppTheme), new Color(255, 255, 255));

Purpose: Represents the secondary color of the application theme.

Functions:

public void SetTheme(Color primary, Color secondary)

Purpose: Sets the application's color theme.

Pre-condition: primary and secondary colors are valid Color objects.

Post-condition: Updates the application's theme colors.

5. public class PendingInstruction

Purpose: Represents an instruction that is pending execution in the UVSIM user interface.

Fields:

public int Line

Purpose: Represents the line number of the instruction.

public string Instruction

Purpose: Represents the instruction as a string.

public bool Valid

Purpose: Indicates whether the instruction is valid.

public bool Loaded

Purpose: Indicates whether the instruction is loaded.

Functions:

public PendingInstruction()

Purpose: Initializes a new instance of the PendingInstruction class.

Parameters:

line (int): The line number of the instruction.
instruction (string): The instruction as a string.
valid (bool): Indicates whether the instruction is valid.
loaded (bool): Indicates whether the instruction is loaded.

Purpose: 

Sets the pending instruction with its line number, instruction string, validity, and loaded status.

Pre-condition: line is a valid integer, instruction is a valid string, valid and loaded are boolean values.

Post-condition: Initializes the PendingInstruction object with the provided values.

6.  public class ClipBoard

Purpose: Represents a clipboard for copying and pasting instructions in the UVSIM user interface.

Fields:

public string Text

Purpose: Represents the text stored in the clipboard.

public List<PendingInstruction> ClippedInstructions

Purpose: Represents the list of pending instructions stored in the clipboard.

Functions:

public ClipBoard()

Purpose: Initializes a new instance of the ClipBoard class.

7. public class InstructionEditorViewModel : INotifyPropertyChanged

Purpose: Represents the view model for the instruction editor in the UVSIM user interface.

Fields:

public event PropertyChangedEventHandler? PropertyChanged;

Purpose: Event triggered when a property changes.

public ObservableCollection<PendingInstruction> PendingInstructions

Purpose: Represents the collection of pending instructions in the instruction editor.

private static ClipBoard clipBoard = new ClipBoard()

Purpose: Represents the clipboard for copying and pasting instructions.

public InstructionEditorViewModel()

Purpose:

Initializes the instruction editor view model.

Functions:

public void LoadProgramFromFile

Purpose:

Loads a program from a file and updates the pending instructions.

Parameters:

string program

Purpose: The program to load.

bool old_format

Purpose: Indicates whether the program is in the old 4digit format.

public string CreateProgram()

Purpose: Creates a program from the pending instructions.

public void CopyCutInstructions

Purpose: Copies or cuts the selected instructions to the clipboard.

Parameters:

int start

Purpose: The starting index of the instructions to copy or cut.

int end

Purpose: The ending index of the instructions to copy or cut.

bool cutMode = false

Purpose: Indicates whether to cut (true) or copy (false) the instructions. false by default

public bool PasteInstructions(int start)

Purpose: Pastes the instructions from the clipboard to the specified index.

public void ClearInstructions()

Purpose: Clears all pending instructions in the instruction editor.

public string SerializeEditor()

Purpose: Serializes the pending instructions into a string format.

8. public partial class MainPage : ContentPage

Fields:

private bool ExecutingAll = false

Purpose: Indicates whether the program is currently executing all instructions.

private bool HaltEncountered = false

Purpose: Indicates whether a HALT instruction has been encountered during execution.

private bool ReadEncountered = false

Purpose:

Indicates whether a READ instruction has been encountered during execution.

Purpose: Represents content for the main page of the UVSIM user interface.

Functions:

UpdateUI()

Purpose: Updates the user interface based on the current state of the view model.

LoadFromFileClicked()

Purpose: Handles the event when the "Load from File" button is clicked.

Parameters:

sender (object): The source of the event.

e (EventArgs): The event data.

Pre-condition: The event is triggered by a user action.

Post-condition: Loads a program from a file and updates the UI accordingly.

SaveClicked()

Purpose: Handles the event when the "Save" button is clicked.

Parameters:

sender (object): The source of the event.

e (EventArgs): The event data.

Pre-condition: The event is triggered by a user action.

Post-condition: Saves the current program to a file and updates the UI accordingly.

private void LoadToMemoryClicked()

Purpose: Handles the event when the "Load to Memory" button is clicked.

Parameters:

sender (object): The source of the event.

e (EventArgs): The event data.

Pre-condition: The event is triggered by a user action.

Post-condition: Loads the current program into memory and updates the UI accordingly.

private void ResetMemoryClicked()

Purpose: Handles the event when the "Reset Memory" button is clicked.

Parameters:

sender (object): The source of the event.

e (EventArgs): The event data.

Pre-condition: The event is triggered by a user action.

Post-condition: Resets the memory and updates the UI accordingly.

private void InputCompleted()

Purpose: Handles the event when the user input is completed.

Parameters:

sender (object): The source of the event.

e (EventArgs): The event data.

Pre-condition: The event is triggered by a user action.

Post-condition: Processes the user input and updates the UI accordingly.

private void ExecuteNextClicked()

Purpose: Handles the event when the "Execute Next" button is clicked.

Parameters:

sender (object): The source of the event.

e (EventArgs): The event data.

Pre-condition: The event is triggered by a user action.

Post-condition: Executes the next instruction and updates the UI accordingly.

private void ExecuteAllClicked()

Purpose: Handles the event when the "Execute All" button is clicked.

Parameters:

sender (object): The source of the event.

e (EventArgs): The event data.

Pre-condition: The event is triggered by a user action.

Post-condition: Executes all instructions and updates the UI accordingly.

private void CopyClicked()

Purpose: Handles the event when the "Copy" button is clicked.

Parameters:

sender (object): The source of the event.

e (EventArgs): The event data.

Pre-condition: The event is triggered by a user action.

Post-condition: Copies the selected instructions to the clipboard and updates the UI accordingly.

private void CutClicked()

Purpose: Handles the event when the "Cut" button is clicked.

Parameters:

sender (object): The source of the event.

e (EventArgs): The event data.

Pre-condition: The event is triggered by a user action.

Post-condition: Cuts the selected instructions to the clipboard and updates the UI accordingly.

private void PasteClicked()

Purpose: Handles the event when the "Paste" button is clicked.

Parameters:

sender (object): The source of the event.

e (EventArgs): The event data.

Pre-condition: The event is triggered by a user action.

Post-condition: Pastes the instructions from the clipboard and updates the UI accordingly.

private void ClearClicked()

Purpose: Handles the event when the "Clear" button is clicked.

Parameters:

sender (object): The source of the event.

e (EventArgs): The event data.

Pre-condition: The event is triggered by a user action.

Post-condition: Clears all pending instructions and updates the UI accordingly.

NewTabClicked()

Purpose: Handles the event when the "New Tab" button is clicked.

Parameters:

sender (object): The source of the event.

e (EventArgs): The event data.

Pre-condition: The event is triggered by a user action.

Post-condition: Opens a new tab in the application and updates the UI accordingly.

9. public static class MauiProgram

Purpose: Represents the main entry point for the Maui application.

Functions:

public static MauiApp CreateMauiApp()

Purpose: Creates the main Maui application.

Pre-condition: None.

Post-condition: Returns a MauiApp instance with the main application shell and theme applied.

10. public partial class SettingsPage : ContentPage

Purpose: Represents the color settings page of the UVSIM user interface.

Fields:

private readonly AppTheme _appTheme

Purpose: Represents the application color theme

Functions:

public SettingsPage()

Purpose: Initializes the settings page with the specified application theme.

Parameters:

appTheme (AppTheme)
Purpose: The application theme to apply.

private void OnPrimaryColorTextChanged

Purpose: Handles the event when the primary color text changes.

Parameters:

sender (object): The source of the event.

e (TextChangedEventArgs): The event data.

Pre-condition: The event is triggered by a user action.

Post-condition: Updates the primary color of the application theme based on the input text.

private void OnSecondaryColorTextChanged

Purpose: Handles the event when the secondary color text changes.

Parameters:

sender (object): The source of the event.

e (TextChangedEventArgs): The event data.

Pre-condition: The event is triggered by a user action.

Post-condition: Updates the secondary color of the application theme based on the input text.

private void OnSubmitClicked

Purpose: Handles the event when the "Submit" button is clicked.

Parameters:

sender (object): The source of the event.

e (EventArgs): The event data.

Pre-condition: The event is triggered by a user action.

Post-condition: Updates the application theme colors based on the input values.

11. public class VirtualMachineViewModel : INotifyPropertyChanged

Purpose: Represents the view model for the virtual machine in the UVSIM user interface.

Fields:

public event PropertyChangedEventHandler? PropertyChanged;

Purpose: Event triggered when a property changes.

public VirtualMachine VM

Purpose: Represents the virtual machine instance.

Function:

public void OnPropertyChanged

Purpose: Raises the PropertyChanged event for the specified property.

Parameters:

propertyName (string): The name of the property that changed.

Pre-condition: propertyName is a valid property name.

Post-condition: Raises the PropertyChanged event for the specified property.
