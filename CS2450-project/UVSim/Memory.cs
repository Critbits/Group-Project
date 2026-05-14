using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UVSIM;

namespace UVSim;

public class Memory
{
    public static readonly int DefaultSize = 250;
    public class MemoryLocation
    {
        public int Data { get; set; }
        public int Index { get; set; }
        public MemoryLocation(int Data, int Index)
        {
            this.Data = Data;
            this.Index = Index;
        }
    }

    public MemoryLocation[] Locations { get; set; }
    public readonly int Length;

    public Memory(int size)
    {
        Locations = new MemoryLocation[size];
        for (int i = 0; i < size; i++)
        {
            Locations[i] = new MemoryLocation(0, i);
        }
        Length = Locations.Length;
    }

    public Memory(int[] memory)
    {
        Length = memory.Length;
        Locations = new MemoryLocation[Length > DefaultSize ? Length : DefaultSize];
        for (int i = 0; i < Length; i++)
        {
            Locations[i] = new MemoryLocation(memory[i], i);
        }
    }

    public int ReadMemory(int address)
    {
        if (address < 0 || address >= Locations.Length)
            throw new IndexOutOfRangeException($"Invalid memory address: {address}");
        return Locations[address].Data;
    }

    public void WriteMemory(int address, int value)
    {
        if (address < 0 || address >= Locations.Length)
            throw new IndexOutOfRangeException($"Invalid memory address: {address}");
        Locations[address].Data = value;
    }
}

