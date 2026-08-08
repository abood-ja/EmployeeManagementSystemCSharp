using EmployeeManagementSystem.Models;
using EmployeeManagementSystem.Services;
using System;

namespace EmployeeManagementSystem
{
    public class Program
    {
        static void ShowMenu()
        {
            Console.Clear();
            Console.WriteLine("========= Employee Management System =========");
            Console.WriteLine("1.  Add Department");
            Console.WriteLine("2.  Add Employee (to Onboarding)");
            Console.WriteLine("3.  Process Next Employee in Onboarding");
            Console.WriteLine("4.  Find Employee by Id");
            Console.WriteLine("5.  Find Employee by Name");
            Console.WriteLine("6.  List Employees by Department");
            Console.WriteLine("7.  Show Average Salary");
            Console.WriteLine("8.  Show Department Report");
            Console.WriteLine("9.  Show Action History");
            Console.WriteLine("10. Show Company Skills");
            Console.WriteLine("11. Add Skill to Employee");
            Console.WriteLine("0.  Exit");
            Console.WriteLine("================================================");
            Console.Write("Choose an option: ");
        }

        static int ReadMenuChoice()
        {
            string? input = Console.ReadLine();
            if (int.TryParse(input, out int choice))
                return choice;


            return -1;
        }

        static void AddDepartment(Company company)
        {
            try
            {
                int id = ReadInt("Enter Department Id: ");
                Console.Write("Enter Department Name: ");
                string name = Console.ReadLine() ?? "";

                var department = new Department { Id = id, Name = name };
                company.AddDepartment(department);
                Console.WriteLine("Department added successfully.");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
            }
        }

        static void AddEmployee(Company company)
        {
            try
            {
                int id = ReadInt("Enter Employee Id: ");
                Console.Write("Enter Employee Name: ");
                string name = Console.ReadLine() ?? "";
                int departmentId = ReadInt("Enter Department Id: ");
                decimal salary = ReadDecimal("Enter Salary: ");

                var employee = new Employee
                {
                    Id = id,
                    Name = name,
                    DepartmentId = departmentId,
                    HireDate = DateOnly.FromDateTime(DateTime.Now),
                    Salary = salary
                };

                company.AddNewEmployeeToOnBoarding(employee);
                Console.WriteLine("Employee added to onboarding queue.");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
            }
        }

        static void ProcessNextOnboarding(Company company)
        {
            try
            {
                company.ProcessNextEmployeeInOnBoarding();
                Console.WriteLine("Next employee in onboarding is now active.");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
            }
        }

        static void FindEmployeeById(Company company)
        {
            int id = ReadInt("Enter Employee Id: ");
            Employee? employee = company.FindEmployeeById(id);

            if (employee is null)
                Console.WriteLine("Employee not found.");
            else
                PrintEmployee(employee);
        }

        static void FindEmployeeByName(Company company)
        {
            Console.Write("Enter Employee Name: ");
            string name = Console.ReadLine() ?? "";
            Employee? employee = company.FindEmployeeByName(name);

            if (employee is null)
                Console.WriteLine("Employee not found.");
            else
                PrintEmployee(employee);
        }

        static void ListEmployeesByDepartment(Company company)
        {
            try
            {
                int departmentId = ReadInt("Enter Department Id: ");
                var employees = company.GetAllEmployeesOfDepartmentById(departmentId);

                if (employees.Count == 0)
                {
                    Console.WriteLine("No employees found in this department.");
                    return;
                }

                foreach (var employee in employees)
                    PrintEmployee(employee);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
            }
        }

        static void ShowAverageSalary(Company company)
        {
            decimal average = company.CalculateAverageSalary();
            Console.WriteLine($"Average Salary: {average:0.00}");
        }

        static void AddSkillToEmployee(Company company)
        {
            try
            {
                int id = ReadInt("Enter Employee Id: ");
                Console.Write("Enter Skill: ");
                string skill = Console.ReadLine() ?? "";

                company.AddSkillToEmployee(id, skill);
                Console.WriteLine("Skill added successfully.");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
            }
        }

        static void PrintEmployee(Employee employee)
        {
            Console.WriteLine($"[{employee.Id}] {employee.Name} - DeptId: {employee.DepartmentId}, " +
                               $"Salary: {employee.Salary}, HireDate: {employee.HireDate}, " +
                               $"Skills: {string.Join(", ", employee.Skills)}");
        }

        static int ReadInt(string prompt)
        {
            int value;
            Console.Write(prompt);
            while (!int.TryParse(Console.ReadLine(), out value))
            {
                Console.Write("Invalid number, try again: ");
            }
            return value;
        }

        static decimal ReadDecimal(string prompt)
        {
            decimal value;
            Console.Write(prompt);
            while (!decimal.TryParse(Console.ReadLine(), out value))
            {
                Console.Write("Invalid number, try again: ");
            }
            return value;
        }

        static void SeedDepartmentsAndEmployees(Company company)
        {
            var it = new Department { Id = 1, Name = "IT" };
            var hr = new Department { Id = 2, Name = "HR" };
            var finance = new Department { Id = 3, Name = "Finance" };

            company.AddDepartment(it);
            company.AddDepartment(hr);
            company.AddDepartment(finance);

            var employees = new[]
            {
                new Employee
                {
                    Id = 1,
                    Name = "Ahmad Khaled",
                    DepartmentId = it.Id,
                    HireDate = new DateOnly(2022, 3, 15),
                    Salary = 1200m,
                    Skills = new() { "C#", "SQL" }
                },
                new Employee
                {
                    Id = 2,
                    Name = "Lina Yousef",
                    DepartmentId = it.Id,
                    HireDate = new DateOnly(2023, 6, 1),
                    Salary = 1500m,
                    Skills = new() { "C#", "Azure" }
                },
                new Employee
                {
                    Id = 3,
                    Name = "Sara Ali",
                    DepartmentId = hr.Id,
                    HireDate = new DateOnly(2021, 11, 20),
                    Salary = 1000m,
                    Skills = new() { "Recruiting" }
                },
                new Employee
                {
                    Id = 4,
                    Name = "Omar Hasan",
                    DepartmentId = finance.Id,
                    HireDate = new DateOnly(2020, 1, 10),
                    Salary = 1700m,
                    Skills = new() { "Excel", "Accounting" }
                },
                new Employee
                {
                    Id = 5,
                    Name = "Rana Fadi",
                    DepartmentId = finance.Id,
                    HireDate = new DateOnly(2024, 2, 5),
                    Salary = 1100m,
                    Skills = new() { "Excel" }
                },
            };

            foreach (var employee in employees)
            {
                company.AddNewEmployeeToOnBoarding(employee);
                company.ProcessNextEmployeeInOnBoarding();
            }
        }

        static void Main(string[] args)
        {
            var company = new Company();
            SeedDepartmentsAndEmployees(company);
            

            bool exit = false;

            do
            {
                ShowMenu();
                int choice = ReadMenuChoice();

                switch (choice)
                {
                    case 1:
                        AddDepartment(company);
                        break;
                    case 2:
                        AddEmployee(company);
                        break;
                    case 3:
                        ProcessNextOnboarding(company);
                        break;
                    case 4:
                        FindEmployeeById(company);
                        break;
                    case 5:
                        FindEmployeeByName(company);
                        break;
                    case 6:
                        ListEmployeesByDepartment(company);
                        break;
                    case 7:
                        ShowAverageSalary(company);
                        break;
                    case 8:
                        company.DisplayDepartmentsReport();
                        break;
                    case 9:
                        company.DisplayActionHistory();
                        break;
                    case 10:
                        company.DisplayCompanySkills();
                        break;
                    case 11:
                        AddSkillToEmployee(company);
                        break;
                    case 0:
                        exit = true;
                        Console.WriteLine("Goodbye!");
                        break;
                    default:
                        Console.WriteLine("Invalid option. Please choose a number from the menu.");
                        break;
                }

                if (!exit)
                {
                    Console.WriteLine();
                    Console.WriteLine("Press Enter to continue...");
                    Console.ReadLine();
                }

            } while (!exit);
        }


      
        
    }
}