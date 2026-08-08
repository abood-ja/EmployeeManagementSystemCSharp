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

        
    }
}
