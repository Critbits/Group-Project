using System.Net.Sockets;

namespace UVSIM;

/// <summary>
/// Op codes for the UVML instructions.
/// </summary>
public enum InstructionType
{
    VALUE = 0,
    READ = 10,
    WRITE = 11,
    LOAD = 20,
    STORE = 21,
    ADD = 30,
    SUBTRACT = 31,
    DIVIDE = 32,
    MULTIPLY = 33,
    BRANCH = 40,
    BRANCHNEG = 41,
    BRANCHZERO = 42,
    HALT = 43,
}

/// <summary>
/// Contains the original value of the instruction and the type of instruction.
/// </summary>
public class Instruction
{
    public int Value { get; }
    public InstructionType Type { get; }

    public Instruction(string instruction)
    {
        if (instruction[0] == '+')
        {
            Value = int.Parse(instruction[1..]);
        }
        else if (instruction[0] == '-')
        {
            Value = -int.Parse(instruction[1..]);
        }

        Type = (InstructionType)Math.Abs(Value / 1000);

        if (!Enum.IsDefined(Type) || instruction[0] == '-')
        {
            Type = InstructionType.VALUE;
        }
    }
}
