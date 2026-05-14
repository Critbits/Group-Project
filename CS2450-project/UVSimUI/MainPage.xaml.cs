namespace UVSimUI;
using UVSIM;
using Microsoft.Maui.Storage;
using System.Threading.Tasks;
using CommunityToolkit.Maui.Alerts;
using CommunityToolkit.Maui.Storage;
using System.Text;
using System.Threading;
using System.Net;

public partial class MainPage : ContentPage
{
    private VirtualMachineViewModel ViewModel = new();
    private InstructionEditorViewModel InstructionEditorViewModel = new();
    public string CurrentFilePath;
    public List<Instruction> LoadedInstructions;

    // private readonly AppTheme _appTheme;
    public MainPage(AppTheme appTheme)
    {
        InitializeComponent();
        // _appTheme = appTheme;
        UpdateUI();
        LoadedInstructions = new List<Instruction>();
        DirectionsLabel.Text = "Directions: Create or Load a program in the instructions editor, then load the program into memory to execute.\n\nUpdated: Old .txt format files will automatically be converted .uvsix files when loaded.\n\nTabs: Color Settings and tabs can be found in the top left tab selection menu.";
    }

    private void UpdateUI()
    {
        InstructionEditor.ItemsSource = null;
        InstructionEditor.ItemsSource = InstructionEditorViewModel.PendingInstructions;

        MemoryDisplay.ItemsSource = null;
        MemoryDisplay.ItemsSource = ViewModel.VM.Memory.Locations;
        AccumulatorDisplay.Text = $"Accumulator = {ViewModel.VM.Accumulator}";
        CounterDisplay.Text = $"Counter = {ViewModel.VM.ProgramCounter}";
    }

    private async void LoadFromFileClicked(object sender, EventArgs e)
    {
        try
        {
            var textFileType = new FilePickerFileType(new Dictionary<DevicePlatform, IEnumerable<string>>() {
                { DevicePlatform.WinUI, new[] { "txt", "uvsix" } }
            });

            var selected_file = await FilePicker.PickAsync(new PickOptions()
            {
                FileTypes = textFileType
            });

            if (selected_file != null)
            {
                CurrentFilePath = selected_file.FullPath;
                string program = FileManager.LoadProgram(CurrentFilePath);
                if (CurrentFilePath.EndsWith(".txt"))
                {
                    InstructionEditorViewModel.LoadProgramFromFile(program, true);
                }
                else if (CurrentFilePath.EndsWith(".uvsix"))
                {
                    InstructionEditorViewModel.LoadProgramFromFile(program, false);
                }
                else
                {
                    throw new Exception("Unsupported File Type");
                }
                UpdateUI();
            }
        }
        catch (Exception ex)
        {
            ConsoleLabel.Text = ($"File selection failed: {ex.Message}");
        }
    }

    private async void SaveClicked(object sender, EventArgs e)
    {
        using var stream = new MemoryStream(Encoding.Default.GetBytes(InstructionEditorViewModel.SerializeEditor()));
        var cancellationToken = new CancellationToken();
        var fileSaverResult = await FileSaver.Default.SaveAsync("new_program.uvsix", stream, cancellationToken);
        if (fileSaverResult.IsSuccessful)
        {
            ConsoleLabel.Text = $"Successful save to location: {fileSaverResult.FilePath}";
        }
        else
        {
            ConsoleLabel.Text = $"Failure, file save error: {fileSaverResult.Exception.Message}";
        }
    }

    private void LoadToMemoryClicked(object sender, EventArgs e)
    {

        try
        {
            string program = InstructionEditorViewModel.CreateProgram();
            LoadedInstructions = Parser.ParseString(program);
            if (ViewModel != null)
            {
                ViewModel.VM.LoadInstructionsIntoMemory(LoadedInstructions);
                UpdateUI();
            }
        }
        catch (Exception ex)
        {
            ConsoleLabel.Text = $"Load to Memory Failed: {ex.Message}";
        }
    }

    private void ResetMemoryClicked(object sender, EventArgs e)
    {
        if (ViewModel != null)
        {

            ViewModel.VM.Memory = new UVSim.Memory(UVSim.Memory.DefaultSize);
            ViewModel.VM.Accumulator = 0;
            ViewModel.VM.ProgramCounter = 0;
            ExecuteAllButton.IsEnabled = true;
            ExecuteNextButton.IsEnabled = true;
            ConsoleEntry.IsEnabled = false;
            ConsoleLabel.Text = "Virtual Machine: No Input needed.";

            foreach (var instruction in InstructionEditorViewModel.PendingInstructions)
            {
                instruction.Loaded = false; ;
            }

            UpdateUI();
        }
    }

    // booleans to keep track of execution state
    private bool ExecutingAll = false;
    private bool HaltEncountered = false;
    private bool ReadEncountered = false;

    private void InputCompleted(object sender, EventArgs e) // Event handler for when the user types enter in the ConsoleEntry
    {
        Entry entry = (Entry)sender;
        string userInput = entry.Text;
        ConsoleLabel.Text = $"User entered: {userInput}";

        // Clear the entry field after input
        entry.Text = "";
        entry.IsEnabled = false;
        ConsoleLabel.Text = "Value Entered. No Input needed.";
        ConsoleEntry.Placeholder = "Console Inputs Here";

        ViewModel.VM.RunSingleInstruction(userInput);
        UpdateUI();

        if (ExecutingAll) // Re-Enter the loop if user chose to execute all instructions
        {
            ExecuteAllClicked(sender, e);
        }
    }

    private void ExecuteNextClicked(object sender, EventArgs e)
    {
        try
        {
            if (ViewModel != null)
            {
                int nextInstruction = ViewModel.VM.Memory.ReadMemory(ViewModel.VM.ProgramCounter);
                string nextInstructionString = nextInstruction.ToString("+000000"); // Used for display purposes
                Instruction instruction = new(nextInstructionString);

                if (instruction.Type == InstructionType.READ)
                {
                    ConsoleLabel.Text = $"Virtual Machine: Enter value to read into location: {nextInstructionString[4..]}, then press enter.";
                    ConsoleEntry.IsEnabled = true;
                    ReadEncountered = true;
                }
                else if (instruction.Type == InstructionType.HALT)
                {
                    ConsoleLabel.Text = $"Virtual Machine: Program halted. Reset memory to start again.";
                    ExecuteNextButton.IsEnabled = false;
                    ExecuteAllButton.IsEnabled = false;
                    HaltEncountered = true;
                    ExecutingAll = false;
                    UpdateUI();
                }
                else
                {
                    ViewModel.VM.RunSingleInstruction();
                    if (instruction.Type == InstructionType.WRITE)
                    {
                        ConsoleEntry.Placeholder = $"Last Ouput: {ViewModel.VM.Memory.ReadMemory(instruction.Value % 1000)}";
                        ConsoleLabel.Text = $"Virtual Machine: Instruction {nextInstructionString[1..3]} executed. Output displayed.";
                    }
                    else
                    {
                        ConsoleLabel.Text = $"Virtual Machine: Instruction {nextInstructionString[1..3]} executed. No input needed.";
                    }
                    UpdateUI();
                }
            }
        }
        catch (Exception ex)
        {
            ConsoleLabel.Text = $"Execution Failed: {ex.Message}";
        }
    }

    private async void ExecuteAllClicked(object sender, EventArgs e)
    {

        try
        {
            if (ViewModel != null)
            {
                ExecutingAll = true;
                HaltEncountered = false;
                ReadEncountered = false;

                while (!HaltEncountered)
                {
                    ExecuteNextClicked(sender, e);

                    if (ReadEncountered)
                    {
                        // Wait until the user enters input
                        while (ReadEncountered)
                        {
                            await Task.Delay(100); // Allow UI updates
                        }
                    }

                    await Task.Delay(100); // Allow UI updates
                }

                ExecutingAll = false;
            }
        }
        catch (Exception ex)
        {
            ConsoleLabel.Text = ($"Execution Failed: {ex.Message}");
        }
    }

    private void CopyClicked(object sender, EventArgs e)
    {
        string copy_start = Copy_Start.Text;
        string copy_end = Copy_End.Text;
        int start_location;
        int end_location;
        if (!Int32.TryParse(copy_start, out start_location))
        {
            ConsoleLabel.Text = "Copy Failed: Invalid copy start position";
        }
        if (!Int32.TryParse(copy_end, out end_location))
        {
            ConsoleLabel.Text = "Copy Failed: Invalid copy end position";
        }
        if (start_location >= 0 && start_location < end_location && end_location < UVSim.Memory.DefaultSize)
        {
            InstructionEditorViewModel.CopyCutInstructions(start_location, end_location);
            ConsoleLabel.Text = $"Copy Successful: locations {start_location}-{end_location} (inclusive) copied to clipboard";
        }
        else
        {
            ConsoleLabel.Text = "Copy Failed: Start location and end location must be 0-249";
        }
    }

    private void CutClicked(object sender, EventArgs e)
    {
        string copy_start = Copy_Start.Text;
        string copy_end = Copy_End.Text;
        int start_location;
        int end_location;
        if (!Int32.TryParse(copy_start, out start_location))
        {
            ConsoleLabel.Text = "Cut Failed: Invalid copy start position";
        }
        if (!Int32.TryParse(copy_end, out end_location))
        {
            ConsoleLabel.Text = "Cut Failed: Invalid copy end position";
        }
        if (start_location >= 0 && start_location < end_location && end_location < UVSim.Memory.DefaultSize)
        {
            InstructionEditorViewModel.CopyCutInstructions(start_location, end_location, true);
            UpdateUI();
            ConsoleLabel.Text = $"Cut Successful: locations {start_location}-{end_location} (inclusive) copied to clipboard";
        }
        else
        {
            ConsoleLabel.Text = "Cut Failed: Start location and end location must be 0-249";
        }
    }

    private void PasteClicked(object sender, EventArgs e)
    {
        string paste_start = Paste_Start.Text;
        int start_location;
        if (Int32.TryParse(paste_start, out start_location) && start_location >= 0 && start_location < UVSim.Memory.DefaultSize)
        {
            if (!InstructionEditorViewModel.PasteInstructions(start_location))
            {
                ConsoleLabel.Text = "Paste Failed: Paste would exceed capacity";
            }
            else
            {
                UpdateUI();
                ConsoleLabel.Text = "Paste Successful";
            }
        }
        else
        {
            ConsoleLabel.Text = "Paste Failed: Invalid paste start position";
        }
    }

    private void ClearClicked(object sender, EventArgs e)
    {
        InstructionEditorViewModel.ClearInstructions();
        UpdateUI();
    }

    private void NewTabClicked(object sender, EventArgs e)
    {
        if (Shell.Current is AppShell appShell)
        {
            appShell.AddNewTab();
        }
    }
}