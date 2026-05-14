using Microsoft.VisualStudio.TestTools.UnitTesting;
using UVSIM;
using System;
using System.Collections.Generic;
using System.IO;

namespace UVSimTest
{
    [TestClass]
    public class UnitTest1
    {
        // -------------------- FileManager Tests --------------------

        [TestMethod]
        public void TestLoadProgram()
        {
            // Arrange
            string filepath = "test_program.txt";
            string expectedContent = "+010007\n+020008\n+030009\n";
            File.WriteAllText(filepath, expectedContent);

            // Act
            string program = FileManager.LoadProgram(filepath);

            // Assert
            Assert.AreEqual(expectedContent, program);

            // Cleanup
            File.Delete(filepath);
        }

        [TestMethod]
        public void TestLoadProgramException()
        {
            // Arrange
            string filepath = "nonexistent_program.txt";

            // Act and Assert
            Assert.ThrowsException<Exception>(() => FileManager.LoadProgram(filepath));
        }

        [TestMethod]
        public void TestSaveProgram()
        {
            // Arrange
            string filepath = "test_save_program.txt";
            string userInput = "+1007\n+2008\n+3009";
            VirtualMachine vm = new(100);
            List<Instruction> instructions = Parser.ParseString(userInput);

            // Act
            FileManager.SaveProgram(filepath, instructions);

            // Assert
            string savedContent = File.ReadAllText(filepath);
            Assert.AreEqual("+1007\r\n+2008\r\n+3009", savedContent);

            // Cleanup
            File.Delete(filepath);
        }

        [TestMethod]
        //works if readonly_directory already created and permission denied
        public void TestSaveProgramException()
        {
            // Arrange
            string filepath = "readonly_directory/test_save_program.txt";
            List<Instruction> program = new()
            {
                new("+1007"),
                new("+2008"),
                new("+3009")
            };

            // Act and Assert
            Assert.ThrowsException<Exception>(() => FileManager.SaveProgram(filepath, program));
        }

        // -------------------- Instruction Tests --------------------

        [TestMethod]
        public void TestInstructionType()
        {
            // Arrange
            Instruction instruction = new("+010000");

            // Assert
            Assert.AreEqual(InstructionType.READ, instruction.Type);
        }

        [TestMethod]
        public void TestInstructionValue()
        {
            // Arrange
            Instruction instruction = new("-1000");

            // Assert
            Assert.AreEqual(InstructionType.VALUE, instruction.Type);
        }

        [TestMethod]
        public void TestInstructionValueCalculation()
        {
            // Arrange
            Instruction instruction = new("-1000");

            // Assert
            Assert.AreEqual(-1000, instruction.Value);
        }

        [TestMethod]
        public void TestInvalidInstructionType()
        {
            // Arrange
            Instruction instruction = new("+9999");

            // Assert
            Assert.AreEqual(InstructionType.VALUE, instruction.Type);
        }

        // -------------------- Memory Tests --------------------

        [TestMethod]
        public void TestMemoryInitialization()
        {
            // Arrange
            Memory<int> memory = new(new int[100]);

            // Assert
            Assert.AreEqual(100, memory.Length);
            Assert.AreEqual(0, memory.Span[0]);
        }

        [TestMethod]
        public void TestMemoryReadWrite()
        {
            // Arrange
            Memory<int> memory = new(new int[100]);

            // Act
            memory.Span[10] = 1234;

            // Assert
            Assert.AreEqual(1234, memory.Span[10]);
        }

        [TestMethod]
        public void TestMemoryOutOfBoundsRead()
        {
            // Arrange
            Memory<int> memory = new(new int[100]);

            // Act and Assert
            Assert.ThrowsException<IndexOutOfRangeException>(() => { var value = memory.Span[200]; });
        }

        [TestMethod]
        public void TestMemoryOutOfBoundsWrite()
        {
            // Arrange
            Memory<int> memory = new(new int[100]);

            // Act and Assert
            Assert.ThrowsException<IndexOutOfRangeException>(() => memory.Span[200] = 1234);
        }

        [TestMethod]
        public void TestAddPositiveValues()
        {
            VirtualMachine vm = new(100);
            vm.Accumulator = 0; //Set Accumulator to 0
            vm.ProgramCounter = 0;

            string userInput = "+030001\n+000005\n+043000";  //Add 5 to Accumulator
            List<Instruction> instructions = Parser.ParseString(userInput);
            vm.LoadInstructionsIntoMemory(instructions);
            vm.RunFromMemory();

            Assert.AreEqual(5, vm.Accumulator);
        }

        [TestMethod]
        public void TestAddNegativeValue()
        {
            VirtualMachine vm = new(100);
            vm.Accumulator = 100;
            vm.Memory.WriteMemory(0, 30010); // ADD from address 10
            vm.Memory.WriteMemory(10, -50);

            vm.RunSingleInstruction();

            Assert.AreEqual(50, vm.Accumulator);
        }

        [TestMethod]
        public void TestBranchSetsProgramCounter()
        {
            VirtualMachine vm = new(100);
            vm.Memory.WriteMemory(0, 40010); // BRANCH to address 10

            vm.RunSingleInstruction();

            Assert.AreEqual(10, vm.ProgramCounter);
        }

        [TestMethod]
        public void TestBranchSkipsInstructions()
        {
            VirtualMachine vm = new(100);
            vm.Memory.WriteMemory(0, 40020); // BRANCH to address 20
            vm.Memory.WriteMemory(1, 11111); // should be skipped

            vm.RunSingleInstruction();

            Assert.AreNotEqual(1, vm.ProgramCounter);
        }

        [TestMethod]
        public void TestBranchNegJumpsIfNegative()
        {
            VirtualMachine vm = new(100);
            vm.Memory.WriteMemory(0, 41030); // BRANCHNEG to 30
            vm.Accumulator = -1;

            vm.RunSingleInstruction();

            Assert.AreEqual(30, vm.ProgramCounter);
        }

        [TestMethod]
        public void TestBranchZeroJumpsIfZero()
        {
            VirtualMachine vm = new(100);
            vm.Memory.WriteMemory(0, 42025); // BRANCHZERO to 25
            vm.Accumulator = 0;

            vm.RunSingleInstruction();

            Assert.AreEqual(25, vm.ProgramCounter);
        }

        [TestMethod]
        public void TestStoreInstruction()
        {
            // Arrange
            VirtualMachine vm = new(100);
            vm.Memory.WriteMemory(0, 21040); // STORE to address 40
            vm.Accumulator = 1234;

            // Act
            vm.RunSingleInstruction();

            // Assert
            Assert.AreEqual(1234, vm.Memory.ReadMemory(40));
        }

        [TestMethod]
        public void TestStoreOverwritesPreviousValue()
        {
            // Arrange
            VirtualMachine vm = new(100);
            vm.Memory.WriteMemory(0, 21010); // STORE to address 10
            vm.Memory.WriteMemory(10, 1111); // existing value
            vm.Accumulator = 5678;

            // Act
            vm.RunSingleInstruction();

            // Assert
            Assert.AreEqual(5678, vm.Memory.ReadMemory(10));
        }

        // -------------------- Parser Tests --------------------

        [TestMethod]
        public void TestParseFile()
        {
            // Arrange
            string filepath = "test_parser_program.txt";
            File.WriteAllText(filepath, "+010007\n+020008\n+030009\n");

            // Act
            List<Instruction> instructions = Parser.ParseFile(filepath);

            // Assert
            Assert.AreEqual(3, instructions.Count);
            Assert.AreEqual(InstructionType.READ, instructions[0].Type);

            // Cleanup
            File.Delete(filepath);
        }

        [TestMethod]
        public void TestParseString()
        {
            // Arrange
            string program = "+010007\n+020008\n+030009\n";

            // Act
            List<Instruction> instructions = Parser.ParseString(program);

            // Assert
            Assert.AreEqual(3, instructions.Count);
            Assert.AreEqual(InstructionType.READ, instructions[0].Type);
        }

        [TestMethod]
        public void TestInvalidInstruction()
        {
            // Arrange
            string program = "+abcd\n";

            // Act
            List<Instruction> instructions = Parser.ParseString(program);

            // Assert
            Assert.AreEqual(0, instructions.Count);
        }

        // -------------------- VirtualMachine Tests --------------------

        [TestMethod]
        public void TestLoadInstructionsIntoMemory()
        {
            // Arrange
            VirtualMachine vm = new(100);
            List<Instruction> instructions = new()
            {
                new("+1007"),
                new("+2008"),
                new("+3009")
            };

            // Act
            vm.LoadInstructionsIntoMemory(instructions);

            // Assert
            Assert.AreEqual(1007, vm.Memory.ReadMemory(0));
            Assert.AreEqual(2008, vm.Memory.ReadMemory(1));
            Assert.AreEqual(3009, vm.Memory.ReadMemory(2));
        }

        [TestMethod]
        public void TestRunSingleInstruction_Read()
        {
            // Arrange
            VirtualMachine vm = new(100);
            vm.Memory.WriteMemory(0, 10000); // READ to address 0
            var input = new StringReader("5000\n");
            Console.SetIn(input);

            // Act
            vm.RunSingleInstruction();

            // Assert
            Assert.AreEqual(5000, vm.Memory.ReadMemory(0));
        }

        [TestMethod]
        public void TestRunSingleInstruction_Write()
        {
            // Arrange
            VirtualMachine vm = new(100);
            vm.Memory.WriteMemory(0, 11010); // WRITE from address 10
            vm.Memory.WriteMemory(10, 1234); // Memory[10] = 1234

            var writer = new StringWriter();
            Console.SetOut(writer);

            // Act
            vm.RunSingleInstruction();

            // Assert
            Assert.AreEqual("Output: 1234\r\n", writer.ToString());
        }

        [TestMethod]
        public void TestRunSingleInstruction_Add()
        {
            // Arrange
            VirtualMachine vm = new(100);
            vm.Memory.WriteMemory(0, 30010); // ADD from address 10
            vm.Memory.WriteMemory(10, 5); // Memory[10] = 5
            vm.Accumulator = 10;

            // Act
            vm.RunSingleInstruction();

            // Assert
            Assert.AreEqual(15, vm.Accumulator);
        }

        [TestMethod]
        public void TestRunSingleInstruction_Branch()
        {
            // Arrange
            VirtualMachine vm = new(100);
            vm.Memory.WriteMemory(0, 40005); // BRANCH to address 5

            // Act
            vm.RunSingleInstruction();

            // Assert
            Assert.AreEqual(5, vm.ProgramCounter);
        }

        [TestMethod]
        public void TestRunFromMemory()
        {
            // Arrange
            VirtualMachine vm = new(100);
            List<Instruction> instructions = new()
            {
                new("+020005"), // LOAD from address 0
                new("+030006"), // ADD from address 1
                new("+043000")  // HALT
            };

            vm.Memory.WriteMemory(5, 5); // Memory[0] = 5
            vm.Memory.WriteMemory(6, 10); // Memory[1] = 10
            vm.LoadInstructionsIntoMemory(instructions);

            // Act
            vm.RunFromMemory();

            // Assert
            Assert.AreEqual(15, vm.Accumulator);
        }

        [TestMethod]
        public void TestExecutesAll()
        {
            VirtualMachine vm = new(100);
            vm.Memory.WriteMemory(0, 20010); // LOAD 10
            vm.Memory.WriteMemory(1, 30011); // ADD 11
            vm.Memory.WriteMemory(2, 21012); // STORE 12
            vm.Memory.WriteMemory(3, 43000); // HALT

            vm.Memory.WriteMemory(10, 5);
            vm.Memory.WriteMemory(11, 10);

            vm.RunFromMemory();

            Assert.AreEqual(15, vm.Memory.ReadMemory(12));
        }
    }
}
