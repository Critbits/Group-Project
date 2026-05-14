using System;
using UVSim;

namespace UVSIM;

/// <summary>
/// Represents a virtual machine for executing UVML instructions.
/// </summary>
public class VirtualMachine
{
    public int Accumulator = 0;
    public Memory Memory { get; set; }
    public int ProgramCounter = 0;

    public VirtualMachine(int size = 250)
    {
        Memory = new Memory(size);
    }

    public VirtualMachine(int[] memory)
    {
        Memory = new Memory(memory);
    }

    public void LoadInstructionsIntoMemory(List<Instruction> instructions)
    {
        for (int i = 0; i < instructions.Count && i < Memory.Length; i++)
        {
            Memory.WriteMemory(i, instructions[i].Value);
        }
    }

    public void RunFromMemory()
    {
        while (ProgramCounter < Memory.Length)
        {
            int instructionValue = Memory.ReadMemory(ProgramCounter);
            if (instructionValue / 1000 == (int)InstructionType.HALT)
            {
                Console.WriteLine("Program halted.");
                break; // Stop execution when HALT is encountered
            }
            RunSingleInstruction();
        }
    }

    public void RunSingleInstruction(string UserInput = "") // UserInput is optional to allow the function to still work in the console application
    {
        if (ProgramCounter >= Memory.Length)
        {
            throw new Exception("Program counter out of bounds.");
        }

        int instructionValue = Memory.ReadMemory(ProgramCounter);
        Instruction instruction = new(instructionValue.ToString("+000000")); // Changed to "+000000" from "D5", keeps the correct sign on the instruction

        if (instruction.Type == InstructionType.READ && UserInput == "")
        {
            UserInput = Console.ReadLine() ?? string.Empty; // Handles UserInput in the console application
        }

        ExecuteInstruction(instruction, UserInput);

        if (instruction.Type != InstructionType.BRANCH &&
            instruction.Type != InstructionType.BRANCHNEG &&
            instruction.Type != InstructionType.BRANCHZERO)
        {
            ProgramCounter++;
        }
    }

    private void ExecuteInstruction(Instruction instruction, string UserInput)
    {
        switch (instruction.Type)
        {
            case InstructionType.READ:
            case InstructionType.WRITE:
            case InstructionType.LOAD:
            case InstructionType.STORE:
                ExecuteMemory(instruction, UserInput);
                break;

            case InstructionType.ADD:
            case InstructionType.SUBTRACT:
            case InstructionType.DIVIDE:
            case InstructionType.MULTIPLY:
                ExecuteArithmetic(instruction);
                break;

            case InstructionType.BRANCH:
            case InstructionType.BRANCHNEG:
            case InstructionType.BRANCHZERO:
                ExecuteBranching(instruction);
                break;

            case InstructionType.HALT:
                Console.WriteLine("Program halted.");
                break;
            case InstructionType.VALUE:
                break;
            default:
                throw new Exception("Unknown instruction.");
        }
    }

    private void ExecuteMemory(Instruction instruction, string UserInput)
    {
        int address = instruction.Value % 1000;
        switch (instruction.Type)
        {
            case InstructionType.READ:
                if (UserInput == "")
                {
                    Console.Write("Input: ");
                    if (int.TryParse(Console.ReadLine(), out int input) && Math.Abs(input) <= 9999) // Writes input to memory for the console application
                    {
                        Memory.WriteMemory(address, input);
                    }
                    else
                    {
                        Console.WriteLine("Invalid input. Must be an integer between -9999 and 9999.");
                    }
                }
                else
                {
                    if (int.TryParse(UserInput, out int input) && Math.Abs(input) <= 9999) // Writes input to memory for the UI application
                    {
                        Memory.WriteMemory(address, input);
                    }
                    else
                    {
                        Console.WriteLine("Invalid input. Must be an integer between -9999 and 9999.");
                    }
                }
                break;
            case InstructionType.WRITE:
                Console.WriteLine($"Output: {Memory.ReadMemory(address)}");
                break;

            case InstructionType.LOAD:
                Accumulator = Memory.ReadMemory(address);
                break;

            case InstructionType.STORE:
                Memory.WriteMemory(address, Accumulator);
                break;
        }
    }

    private void ExecuteArithmetic(Instruction instruction)
    {
        int address = instruction.Value % 1000;
        int value = Memory.ReadMemory(address);

        switch (instruction.Type)
        {
            case InstructionType.ADD:
                Accumulator += value;
                break;
            case InstructionType.SUBTRACT:
                Accumulator -= value;
                break;
            case InstructionType.DIVIDE:
                if (value == 0) throw new DivideByZeroException("Cannot divide by zero.");
                Accumulator /= value;
                break;
            case InstructionType.MULTIPLY:
                Accumulator *= value;
                break;
            case InstructionType.VALUE:
                break;
        }
    }

    private void ExecuteBranching(Instruction instruction)
    {
        int address = instruction.Value % 1000;

        switch (instruction.Type)
        {
            case InstructionType.BRANCH:
                ProgramCounter = address;
                break;

            case InstructionType.BRANCHNEG:
                if (Accumulator < 0)
                {
                    ProgramCounter = address;
                    return;
                }
                break;

            case InstructionType.BRANCHZERO:
                if (Accumulator == 0)
                {
                    ProgramCounter = address;
                    return;
                }
                break;
        }
    }
}
