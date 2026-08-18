# Keplercraft V Orbital Station Console

## Overview
Keplercraft V Orbital Station Console is a simulation system designed to manage critical orbital station modules:
- **Life Support** (oxygen monitoring and drain)
- **Power Core** (reactor temperature control)
- **Research Lab** (data collection and experiments)

The system demonstrates **object‑oriented programming concepts** including abstraction, inheritance, interfaces, events, exception handling, and multithreading.

---

## Features
- Create and manage station modules (Life Support, Power Core, Research Lab).
- Track operational status, power consumption, and risk levels.
- Raise **critical condition events** when thresholds are exceeded.
- Background threading simulates:
  - Oxygen drain in Life Support
  - Reactor heating in Power Core
  - Data collection in Research Lab
- Custom exceptions (`ValueBelowZero`, `SystemNotOperational`) enforce validation rules.
- Event‑driven reporting via `ModuleManager`.

---

## Technologies Used
- **C# (.NET)**  
- **Object‑Oriented Programming** (abstraction, inheritance, polymorphism)  
- **Interfaces** (`Imaintainable`, `IStatusReport`)  
- **Events & Delegates**  
- **Multithreading** (`Task.Run`)  

---

## Threading and Multithreading

This project uses **threading** to simulate real‑time behavior of station modules. Each module runs its own background task:

- **Life Support Module** → a thread decreases oxygen levels over time.
- **Power Core Module** → a thread increases reactor temperature over time.
- **Research Lab Module** → a thread accumulates data over time.

These tasks are implemented using `Task.Run()` in **ModuleThreads.cs**, which allows them to run **concurrently** with the main program.  

This is an example of **multithreading**:  
- Multiple threads execute simultaneously (oxygen drain, reactor heating, data collection).  
- The main program remains responsive while background tasks continue.  
- Critical events are raised independently by each thread when thresholds are reached.  

In short: threading makes the simulation realistic by running module routines in parallel, rather than sequentially.

---

## Project Structure
project-root/
│── Program.cs
│── StationModule.cs
│── ModuleManager.cs
│── ModuleThreads.cs
│── LifeSupportModule.cs
│── PowerCoreModule.cs
│── ResearchLabModule.cs
│── Imaintainable.cs
│── IStatusReport.cs
│── ValueBelowZero.cs
│── SystemNotOperational.cs
│── README.md

---

## Getting Started
1. Clone or download the project.  
2. Open in **Visual Studio**.  
3. Build the solution to restore dependencies.  
4. Run `Program.cs` — modules will be created, added to the manager, and background threads will start simulating conditions.

---

## Usage Example
When you run the program:
- Life Support oxygen drains over time.  
- Power Core reactor temperature rises.  
- Research Lab collects data.  

Critical events are raised automatically:

[CRITICAL SYSTEM EVENT]
CRITICAL: Primary Life Support oxygen level at 19.5%.

---

## Group Members
- Amonique van Zyl 604294
- Aden Gouws 604061
- Letlhogonolo Kganyaetsile Phale 603862
- Gert-Pieter van Zyl 604008  

---

