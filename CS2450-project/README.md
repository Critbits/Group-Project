# UVSim - A Machine Language Simulator

UVSim is a software simulator designed for computer science students to learn machine language and computer architecture. It allows students to write and execute programs in a simple yet powerful machine language called **BasicML**. Through UVSim, students can gain hands-on experience with concepts such as CPU, registers, memory, and control flow.

---

## Table of Contents
- [Features](#features)
- [BasicML Instructions](#basicml-instructions)
- [Directory Structure](#directory-structure)
- [Installation](#installation)
- [Terminal Usage](#terminal-usage)
- [Unit Testing](#unit-testing)
- [Contributors](#contributors)
- [License](#license)

---

## Features
- **Virtual Machine (VM)**: Simulates a CPU with an accumulator, 250-word memory, and program counter.
- **BasicML Support**: Executes programs written in BasicML, a simple machine language.
- **Arithmetic Operations**: Perform addition, subtraction, multiplication, and division.
- **Control Flow**: Branching instructions to control program execution.
- **I/O Operations**: Read input from the user and write output to the console.
- **Memory Pagination**: View memory values in pages for better usuability.
- **Error Handling**: Handles invalid instructions, memory access violations, and division by zero.
- **Insturciton Validation**: ENsures instructions follow the BasicML format vefore execution.
- **Unit Testing**: Comprehensive unit tests for all use cases.
- **Instruction Editor**: Ability to create, edit, and save programs from within the application.
- **Graphical User Interface**: Graphical implementation for ease of use.
---

## BasicML Instructions
UVSim supports the following BasicML operations:

### I/O Operations:
| Instruction | Code | Description                                      |
|-------------|------|--------------------------------------------------|
| `READ`      | `10` | Read a word from the keyboard into memory.       |
| `WRITE`     | `11` | Write a word from memory to the screen.          |

### Load/Store Operations:
| Instruction | Code | Description                                      |
|-------------|------|--------------------------------------------------|
| `LOAD`      | `20` | Load a word from memory into the accumulator.    |
| `STORE`     | `21` | Store the accumulator value into memory.         |

### Arithmetic Operations:
| Instruction   | Code | Description                                    |
|---------------|------|------------------------------------------------|
| `ADD`         | `30` | Add a word from memory to the accumulator.     |
| `SUBTRACT`    | `31` | Subtract a word from memory from the accumulator. |
| `DIVIDE`      | `32` | Divide the accumulator by a word in memory.    |
| `MULTIPLY`    | `33` | Multiply the accumulator by a word in memory.  |

### Control Operations:
| Instruction   | Code | Description                                    |
|---------------|------|------------------------------------------------|
| `BRANCH`      | `40` | Jump to a specific memory location.            |
| `BRANCHNEG`   | `41` | Jump if the accumulator is negative.           |
| `BRANCHZERO`  | `42` | Jump if the accumulator is zero.               |
| `HALT`        | `43` | Stop program execution.                        |

---

## Directory Structure

The project directory is organized as follows:

UVSim/

├── FileManager.cs----------# Contains functionality for creating, loading, and saving files.

├── Instruction.cs----------# Contains the implementation of BasicML instructions.

├── Memory.cs----------# An implementation of virtual memory.

├── Parser.cs---------------# Parses and validates BasicML programs.

├── Program.cs------------# Entry point of the application.

├── Terminal.cs------------# Handles user interaction and terminal operations

├── UVSim.csproj----------# Project configuration file.

├── VirtualMachine.cs-----# Implementation of the UVSim virtual machine.

├── UVSim.sln-------------# Solution file for the project.

├── UVSimTest/-----------# Contains unit tests for the project.

│   ├── UnitTest1.cs--------# Unit tests for various components.

│   ├── UVSimTest.csproj--# Unit test project configuration file.

├── tests/-----------------# Test files for the simulator.

│   ├── Test1.txt------------# Sample BasicML program.

│   ├── Test2.txt------------# Another sample BasicML program.

│   ├── unitTests.csv-------# Spreadsheet for unit test documentation.

├── UVSimUI/-----------# Contains files for the GUI application.

├── designDocuments/-----------# Contains design documentation for the project.

├── projectDesign.md---# Project design document.

├── README.md---------# This file.

└── .gitignore------------# Git ignore file.

---

## Installation

### Prerequisites

- **.NET 8.0 SDK**: Ensure the .NET SDK is installed on your system. You can download it from [here](https://dotnet.microsoft.com/).

### Steps

1.  Clone the repository:

     ```bash
     git clone https://github.com/joshuaS248/CS2450-project.git
     ```
2.  Build the project:

    ```bash
    dotnet build
    ```
3.  Navigate to `UVSimUI` dirctory: 

    ```bash
    cd UVSimUI
    ```

### Running the simulator

1.  Prepare a BasicML program file (e.g., `Test3.txt`) in the `tests` direcotory.
2.  Run the simulator:
    ```bash
    dotnet run --framework:net9.0-windows10.0.19041.0
    ```
3. Follow the on-screen instruction to:
   - Load a BasicML program.
   - Execute the program step-by-step or as a whole.
   - View memory, accumulator, and program counter values.
   - Clear the terminal for a clean execution environment
   


### Example Input File(`Test3.txt`):

    "+100009"
    "+100010"
    "+200009"
    "+310010"
    "+410007"
    "+110009"
    "+430000"
    "+110010"
    "+430000"
    "+000000"
    "+000000"
    "-99999"

## Unit Testing

### Running Unit Tests

1.  Navigate to the `UVSimTest` directory:
    ```bash
    cd UVSimTest
    ```
2.  Run the tests:
    ```bash
    dotnet test
    ```
3.  Review the test results to ensure functionality and reliability

### Test Documentation

The `unitTests.csv` file in the `tests` directory contains detailed informaiton about each unit test, including:
  - Test Name
  - Description
  - Use Case Reference
  - Inputs
  - Expected Outputs
  - Success/Failure Criteria

---

## Contributors

1.  [Limhi Canton]
2.  [Christian Fellows]
3.  [Samuel Griffey]
4.  [Joshua Slaugh]

---

## License

This project is licensed under the MIT License. You are free to use, modify, and distribute this software as per the terms of the license.

---

## Additional Notes

1. Ensure that the input file follows the BasicML format.
  

















