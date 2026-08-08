using EmployeeManagementSystem.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EmployeeManagementSystem.Services
{
    public class Company
    {
        List<Employee>ActiveEmployees=new List<Employee>();
        Dictionary<int,Department>Departments=new Dictionary<int,Department>();
        Queue<Employee>OnBoarding=new Queue<Employee>();
        Stack<string> ActionHistory = new Stack<string>();
        HashSet<string>Skills=new HashSet<string>();

        public void AddDepartment(Department department)
        {
            if(department is null)
                throw new ArgumentNullException(nameof(department));
            if (string.IsNullOrWhiteSpace(department.Name))
                throw new ArgumentException("Department name is required.");

            if (Departments.ContainsKey(department.Id))
                throw new InvalidOperationException($"Department Id {department.Id} is already in the company.");
            Departments.Add(department.Id, department);
            ActionHistory.Push($"Add new department: {department.Name} department");
        }

        public void AddNewEmployeeToOnBoarding(Employee employee)
        {
            if(employee is null)
                throw new ArgumentNullException( nameof(employee));
            if(string.IsNullOrWhiteSpace(employee.Name))
                throw new ArgumentException("Department name is required.");
            if (!Departments.ContainsKey(employee.DepartmentId))
                throw new InvalidOperationException($"Department Id {employee.DepartmentId} does not exist.");
            if(FindEmployeeById(employee.Id) is not null)
                throw new InvalidOperationException($"Employee Id {employee.Id} already exists.");
            OnBoarding.Enqueue(employee);
            ActionHistory.Push($"a new employee was added to onboarding: Employee[{employee.Id}], EmployeeName: {employee.Name}");
        }

        public void ProcessNextEmployeeInOnBoarding()
        {
            if (OnBoarding.Count == 0)
                throw new InvalidOperationException("there are no employees in onboarding queue");
            Employee employee = OnBoarding.Dequeue();
            ActiveEmployees.Add(employee);
            foreach (string skill in employee.Skills)
            {
                Skills.Add(skill);
            }
            ActionHistory.Push($"a new employee is not Active: Employee[{employee.Id}], EmployeeName: {employee.Name}");
        }

        public Employee? FindEmployeeByName(string name)
        {
            if (string.IsNullOrWhiteSpace(name))
                return null;
            foreach(var emp in ActiveEmployees)
            {
                if(emp.Name == name) return emp;
            }
            return null;
        }

        public Employee? FindEmployeeById(int Id)
        {
            foreach(var emp in ActiveEmployees)
            {
                if(emp.Id == Id) return emp;
            }
            return null;
        }

        public List<Employee> GetAllEmployeesOfDepartmentById(int DepartmentId)
        {
            List<Employee> employees = new();
            if (!Departments.TryGetValue(DepartmentId, out Department? department))
                throw new InvalidOperationException($"Department Id {DepartmentId} does not exist.");
            foreach(var emp in ActiveEmployees)
            {
                if (emp.DepartmentId == DepartmentId)
                    employees.Add(emp);
            }
            return employees;
        }

        public decimal CalculateAverageSalary()
        {
            if(ActiveEmployees.Count==0) return 0;
            decimal totalSalary = 0;
            foreach(var emp in ActiveEmployees)
            {
                totalSalary+= emp.Salary;
            }
            return totalSalary/ActiveEmployees.Count;
        }

        public void DisplayDepartmentsReport()
        {
            Console.WriteLine("===== Department Report =====");
            Console.WriteLine();

            foreach (KeyValuePair<int, Department> pair in Departments)
            {
                int employeeCount = 0;

                foreach (Employee employee in ActiveEmployees)
                {
                    if (employee.DepartmentId == pair.Key)
                        employeeCount++;
                }

                Console.WriteLine($"{pair.Value.Name,-10}: {employeeCount} employees");
            }

            Console.WriteLine();
            Console.WriteLine("=============================");
        }

        public void DisplayActionHistory()
        {
            Console.WriteLine("Action History:");

            foreach (string action in ActionHistory)
                Console.WriteLine(action);
        }

        public void DisplayCompanySkills()
        {
            Console.WriteLine("Company Skills:");

            foreach (string skill in Skills)
                Console.WriteLine(skill);
        }

        public void AddSkillToEmployee(int employeeId, string skill)
        {
            if (string.IsNullOrWhiteSpace(skill))
                throw new ArgumentException("Skill name is required.");

            Employee? employee = FindEmployeeById(employeeId);

            if (employee is null)
                throw new InvalidOperationException($"Employee with Id {employeeId} was not found.");

            string normalizedSkill = skill.Trim();

            if (!employee.Skills.Contains(normalizedSkill))
                employee.Skills.Add(normalizedSkill);

            Skills.Add(normalizedSkill);
            ActionHistory.Push($"Added skill {normalizedSkill} to {employee.Name}");
        }

        

    }
}
