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
    }
}
