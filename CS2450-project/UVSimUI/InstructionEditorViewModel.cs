using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using UVSIM;

namespace UVSimUI
{
    public class PendingInstruction
    {
        public int Line { get; set; }
        public string Instruction { get; set; }
        public bool Valid { get; set; }
        public bool Loaded { get; set; }

        public PendingInstruction(int line, string instruction, bool valid = false, bool loaded = false)
        {
            Line = line;
            Instruction = instruction;
            Valid = valid;
            Loaded = loaded;
        }
    }

    public class ClipBoard
    {
        public List<PendingInstruction> ClippedInstructions { get; }
        public int length;

        public ClipBoard()
        {

            ClippedInstructions = new List<PendingInstruction>();
            length = 0;
            for (int i = 0; i < UVSim.Memory.DefaultSize; i++)
            {
                ClippedInstructions.Add(new PendingInstruction(i, "None"));
            }
        }
    }

    public class InstructionEditorViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler? PropertyChanged;
        public ObservableCollection<PendingInstruction> PendingInstructions { get; }
        private static ClipBoard clipBoard = new ClipBoard();
        public InstructionEditorViewModel()
        {
            PendingInstructions = new ObservableCollection<PendingInstruction>();

            for (int i = 0; i < UVSim.Memory.DefaultSize; i++)
            {
                PendingInstructions.Add(new PendingInstruction(i, "None"));
            }
        }
        /// <summary>
        /// Loads a program from a file. If old_format is true, also converts all instructions to uvsix format
        /// </summary>
        /// <param name="program"></param>
        /// <param name="old_format">true for .txt false for .uvsix</param>
        public void LoadProgramFromFile(string program, bool old_format)
        {
            int line = 1;
            if (old_format)
            {
                foreach (var instruction in program.Split('\n'))
                {
                    string TrimmedInstruction = instruction.Trim();
                    TrimmedInstruction = TrimmedInstruction.Insert(1, "0");
                    TrimmedInstruction = TrimmedInstruction.Insert(4, "0");
                    PendingInstructions[line - 1].Instruction = TrimmedInstruction;
                    PendingInstructions[line - 1].Valid = Parser.ValidateInstructionLong(TrimmedInstruction);
                    line++;
                }
            }
            else
            {
                foreach (var instruction in program.Split('\n'))
                {
                    string TrimmedInstruction = instruction.Trim();
                    PendingInstructions[line - 1].Instruction = TrimmedInstruction;
                    PendingInstructions[line - 1].Valid = Parser.ValidateInstructionLong(TrimmedInstruction);
                    line++;
                }
            }
            for (int i = (line - 1); i < PendingInstructions.Count; i++)
            {
                PendingInstructions[i].Instruction = "None";
                PendingInstructions[i].Valid = false;
            }
            OnPropertyChanged(nameof(PendingInstructions));
        }

        public string CreateProgram()
        {
            foreach (var instruction in PendingInstructions)
            {
                if (Parser.ValidateInstructionLong(instruction.Instruction))
                {
                    instruction.Valid = true;
                }
            }
            StringBuilder sb = new();
            foreach (var instruction in PendingInstructions)
            {
                if (instruction.Valid)
                {
                    sb.AppendLine(instruction.Instruction);
                    instruction.Loaded = true;
                }
            }
            return sb.ToString();
        }

        public void CopyCutInstructions(int start, int end, bool cutMode = false)
        {
            clipBoard.length = end - start + 1;
            for (int i = start, j = 0; i <= end; i++, j++)
            {
                clipBoard.ClippedInstructions[j].Instruction = PendingInstructions[i].Instruction;
                clipBoard.ClippedInstructions[j].Valid = PendingInstructions[i].Valid;
                if (cutMode)
                {
                    PendingInstructions[i].Instruction = "None";
                    PendingInstructions[i].Valid = false;
                    PendingInstructions[i].Loaded = false;
                }
            }
        }

        public bool PasteInstructions(int start)
        {
            if (start + clipBoard.length - 1 > PendingInstructions.Count)
            {
                return false;
            }

            for (int i = start, j = 0; i < start + clipBoard.length; i++, j++)
            {
                PendingInstructions[i].Instruction = clipBoard.ClippedInstructions[j].Instruction;
                PendingInstructions[i].Valid = clipBoard.ClippedInstructions[j].Valid;
                PendingInstructions[i].Loaded = false;
            }
            return true;
        }

        public void ClearInstructions()
        {
            PendingInstructions.Clear();
            for (int i = 0; i < UVSim.Memory.DefaultSize; i++)
            {
                PendingInstructions.Add(new PendingInstruction(i, "None"));
            }
        }

        /// <summary>
        /// Converts List<Instruction> program to a string that can be stored.
        /// </summary>
        /// <param name="program">Program to be serialized.</param>
        /// <returns>Serialized program.</returns>
        public string SerializeEditor()
        {
            StringWriter sw = new();

            for (int i = 0; i < PendingInstructions.Count; i++)
            {
                if (PendingInstructions[i].Instruction != "None")
                    sw.Write(PendingInstructions[i].Instruction);

                if (i < PendingInstructions.Count - 1) // Only add newline if it's not the last instruction
                    sw.WriteLine();
            }

            return sw.ToString();
        }

        public void OnPropertyChanged([CallerMemberName] string name = "") =>
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
    }
}
