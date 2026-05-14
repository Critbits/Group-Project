#UVSim

# Project Description


# User Stories:
1. As a Computer Science student, I want to be able to load and run BasicML programs on the UVSim so I can understand and practice core concepts of machine language and computer architecture.

2. As an Instructor, I want the UVSim to correctly simulate memory operations and basic arithmetic functions so that my students can learn how computers manage memory and how different machine instructions work together in a program.

# Use Cases:
1. Description: Load a BasicML program into the UVSim memory, starting from location 00.
  - Actor: Student
  - Precondition: A valid BasicML program file.
  - Postcondition: Program loaded in memory.
  - Steps:
  - The student selects a BasicML program file.
  - UVSim loads each instruction into memory, starting at location 00.
  - Reading Data from Keyboard (READ operation)

2. Description: Allow the student to input a value through the keyboard and store it in a specified memory location.
  - Actor: Student
  - Precondition: A memory location is provided.
  - Postcondition: The input word is stored in the memory location.
  - Steps:
  - The student selects the memory address where the input will be stored.
  - The program prompts for keyboard input.
  - The input is stored in the selected memory location.
  - Writing Data to Screen (WRITE operation)

3. Description: Display a word from a specific memory location on the screen.
  - Actor: Student
  - Precondition: Data exists in the specified memory location.
  - Postcondition: The word is displayed on the screen.
  - Steps:
  - The student specifies the memory address to be displayed.
  - UVSim retrieves the word from the address and shows it on the screen.
  - Loading Data into the Accumulator (LOAD operation)

4. Description: Copy a word from a memory location into the accumulator for processing.
  - Actor: Student
  - Precondition: A valid memory location must be chosen.
  - Postcondition: The word from the memory is loaded into the accumulator.
  - Steps:
  - The student selects a memory location to load from.
  - UVSim copies the word from memory into the accumulator.
  - Storing Data from the Accumulator (STORE operation)

5. Description: Store the word currently in the accumulator into a specified memory location.
  - Actor: Student
  - Precondition: The accumulator must contain data.
  - Postcondition: The data in the accumulator is written to the chosen memory location.
  - Steps:
  - The student selects a memory location where data will be stored.
  - UVSim stores the word from the accumulator in the specified location.
  - Adding Data to the Accumulator (ADD operation)

6. Description: Add a word from memory to the word in the accumulator.
  - Actor: Student
  - Precondition: Both the accumulator and the memory location must contain valid data.
  - Postcondition: The sum of the values is stored in the accumulator.
  - Steps:
  - The student selects a memory location for the addition.
  - UVSim adds the word in memory to the accumulator’s value.
  - Subtracting Data from the Accumulator (SUBTRACT operation)

7. Description: Subtract a word from memory from the word in the accumulator.
  - Actor: Student
  - Precondition: The accumulator and memory location must have valid values.
  - Postcondition: The difference is stored in the accumulator.
  - Steps:
  - The student selects a memory location for subtraction.
  - UVSim subtracts the word in memory from the word in the accumulator.
  - Dividing Data in the Accumulator (DIVIDE operation)

8. Description: Divide the word in the accumulator by a word from a specified memory location.
  - Actor: Student
  - Precondition: The accumulator and memory location must contain valid numbers, and the divisor cannot be zero.
  - Postcondition: The quotient is stored in the accumulator.
  - Steps:
  - The student selects a memory location to divide by.
  - UVSim divides the accumulator by the memory value.
  - Multiplying Data with the Accumulator (MULTIPLY operation)

9. Description: Multiply the word in memory by the word in the accumulator.
  - Actor: Student
  - Precondition: The accumulator and the memory location must have valid data.
  - Postcondition: The product is stored in the accumulator.
  - Steps:
  - The student selects a memory location for multiplication.
  - UVSim multiplies the memory word by the word in the accumulator.
  - Branching to a Memory Location (BRANCH operation)

10. Description: Change the program’s execution to a specific memory location.
  - Actor: Student
  - Precondition: A valid memory address is given.
  - Postcondition: Program execution continues from the new memory location.
  - Steps:
  - The student enters a memory address to jump to.
  - UVSim sets the program counter to the given location.
  - Branching if Accumulator is Negative (BRANCHNEG operation)

11. Description: If the accumulator contains a negative value, jump to a specific memory location.
  - Actor: Student
  - Precondition: The accumulator contains a value.
  - Postcondition: Execution jumps to the given memory location if the accumulator is negative.
  - Steps:
  - The student enters the branch location.
  - UVSim checks the accumulator. If it’s negative, it jumps to the memory address.
  - Branching if Accumulator is Zero (BRANCHZERO operation)

12. Description: If the accumulator is zero, jump to a specified memory location.
  - Actor: Student
  - Precondition: The accumulator contains a value.
  - Postcondition: Program execution jumps to the specified memory location if the accumulator is zero.
  - Steps:
  - The student provides a branch location.
  - UVSim checks if the accumulator is zero, and if true, jumps to the memory address.
  - Halting Program Execution (HALT operation)

13. Description: Stop the program from running.
  - Actor: Student
  - Precondition: A valid program is running.
  - Postcondition: Program execution halts.
  - Steps:
  - The student enters the HALT command.
  - UVSim stops executing the program.
  - Program Execution Loop

14. Description: Create Instructions objects
  - Actor: Parser
  - Precondition: The program is loaded and ready to run.
  - Postcondition: Each instruction is executed until the program halts.
  - Steps:
  - UVSim begins execution at location 00.
  - It continues executing each instruction in memory until a HALT command is encountered.
  - Viewing the Accumulator and Memory State

15. Description: Validate instructions
  - Actor: Parser
 -  Precondition: A instruction is being read from a file.
  - Postcondition: Valid instructions are added to the instruction list.
  - Steps:
  - The parser calls the validate function
  - If the validate function returns positive store the instruction

