# Employee Management System

A console-based Employee Management System built in C# to practice Collections (`List`, `Dictionary`, `Queue`, `Stack`, `HashSet`), OOP (inheritance), manual loop-based search/filtering, and building a menu-driven console app.

## Overview

The system manages departments and employees for a company. New employees go through an **onboarding queue** before becoming active. Every important action (adding a department, onboarding an employee, activating an employee, adding a skill) is logged to an **action history stack**. The company also tracks the **set of unique skills** across all employees.

## Project Structure

```
EmployeeManagementSystem/
├── Models/
│   ├── Employee.cs        # Id, Name, DepartmentId, HireDate, Salary, Skills
│   ├── Manager.cs         # Inherits Employee, adds TeamMembers
│   └── Department.cs      # Id, Name
├── Services/
│   └── Company.cs         # Core business logic (see below)
└── Program.cs              # Console entry point: do-while menu + seed data
```

## Domain Models

- **Employee** — `Id`, `Name`, `DepartmentId`, `HireDate` (`DateOnly`), `Salary`, `Skills` (`List<string>`)
- **Manager** — inherits `Employee`, adds `TeamMembers` (employees reporting to this manager)
- **Department** — `Id`, `Name`

## Company (Business Logic)

`Company` holds five collections and exposes operations on top of them:

| Collection | Type | Purpose |
|---|---|---|
| `ActiveEmployees` | `List<Employee>` | Employees who finished onboarding |
| `Departments` | `Dictionary<int, Department>` | Departments keyed by Id |
| `OnBoarding` | `Queue<Employee>` | Employees waiting to be activated, processed FIFO |
| `ActionHistory` | `Stack<string>` | Log of every action, most recent first |
| `Skills` | `HashSet<string>` | Unique skills across the whole company |

### Key operations

- `AddDepartment(Department)` — validates and registers a new department, logs the action.
- `AddNewEmployeeToOnBoarding(Employee)` — validates the employee (name, existing department, no duplicate Id) and enqueues them, logs the action.
- `ProcessNextEmployeeInOnBoarding()` — dequeues the next employee, moves them to `ActiveEmployees`, merges their skills into the company `Skills` set, logs the action.
- `FindEmployeeById(int)` / `FindEmployeeByName(string)` — manual `foreach` search (no LINQ).
- `GetAllEmployeesOfDepartmentById(int)` — manual `foreach` filter (no LINQ).
- `CalculateAverageSalary()` — manual accumulation loop (no LINQ).
- `DisplayDepartmentsReport()` — prints employee count per department, computed with manual loops.
- `DisplayActionHistory()` — prints the stack, most recent action first.
- `DisplayCompanySkills()` — prints all unique skills.
- `AddSkillToEmployee(int, string)` — adds a skill to a specific employee and to the company-wide skill set, logs the action.

## Console Menu (Program.cs)

The app runs in a `do-while` loop until the user chooses `0`:

```
1.  Add Department
2.  Add Employee (to Onboarding)
3.  Process Next Employee in Onboarding
4.  Find Employee by Id
5.  Find Employee by Name
6.  List Employees by Department
7.  Show Average Salary
8.  Show Department Report
9.  Show Action History
10. Show Company Skills
11. Add Skill to Employee
0.  Exit
```

### Input validation

- `ReadMenuChoice()` returns `-1` for any non-numeric input, which falls through to the `default` case and shows an "Invalid option" message instead of crashing.
- `ReadInt(prompt)` and `ReadDecimal(prompt)` loop until the user enters a valid number.
- All operations that can throw (duplicate Ids, missing departments, empty onboarding queue, etc.) are wrapped in `try/catch`, so business-rule violations are shown as friendly error messages instead of crashing the app.

## Seed Data

On startup, `SeedDepartmentsAndEmployees(company)` runs automatically and:

1. Creates 3 departments: **IT**, **HR**, **Finance**.
2. Creates 5 employees, immediately adds each one to onboarding, and processes them right away — so the app starts with active employees, a populated skill set, and a non-empty action history, ready to explore from the menu.

| Id | Name | Department | Salary | Initial Skills |
|---|---|---|---|---|
| 1 | Ahmad Khaled | IT | 1200 | C#, SQL |
| 2 | Lina Yousef | IT | 1500 | C#, Azure |
| 3 | Sara Ali | HR | 1000 | Recruiting |
| 4 | Omar Hasan | Finance | 1700 | Excel, Accounting |
| 5 | Rana Fadi | Finance | 1100 | Excel |

## How to Run

```bash
dotnet build
dotnet run
```

Requires .NET 6+ (uses `DateOnly`).

## Notes / Possible Extensions

- `Manager` extends `Employee` with `TeamMembers`. It isn't currently wired into the console menu (no "Promote to Manager" or "Assign Team Member" flow yet) — this can be added as a future menu option if needed.
- Employee/Department Ids are entered manually; auto-incrementing Ids could be added to `Company` later.
