using EmployeeManagement.Models;
using EmployeeManagement.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Xml.Serialization;
namespace EmployeeManagement.Controllers
{
    public class HomeController : Controller
    {
        private readonly EmployeeManagementDbContext _dbContext;

        public HomeController(EmployeeManagementDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        // Index: Display list of employees
        public async Task<IActionResult> Index()
        {
            var employees = await _dbContext.Employees.ToListAsync();
            return View(employees);
        }
        // Add by Anjali Agrawal
        // Details: Display employee details
        public async Task<IActionResult> Details(int id)
        {
            var employee = await _dbContext.Employees.FirstOrDefaultAsync(e => e.Id == id);
            if (employee == null)
            {
                return View("NotFound");
            }

            var employeeViewModel = new EmployeeViewModel
            {
                Id = employee.Id,
                Name = employee.Name,
                Email = employee.Email,
                Department = employee.Department
            };

            return View(employeeViewModel);
        }
        // Add by Anjali Agrawal
        // JSON Response: Return employee data as JSON
        [HttpGet]
        [Route("api/employee/json")]
        public async Task<IActionResult> GetEmployeeJson()
        {
            var employees = await _dbContext.Employees.ToListAsync();
            return Json(employees);
        }
        // SOAP Response: Return employee data as XML SOAP
        [HttpGet]
        [Route("api/employee/soap")]
        public async Task<IActionResult> GetEmployeeSoap()
        {
            var employees = await _dbContext.Employees.ToListAsync();

            // Serialize the employee list into XML
            var xmlSerializer = new XmlSerializer(typeof(List<Employee>));

            using var stringWriter = new StringWriter();
            {
                // Serialize the list of employees to XML format
                xmlSerializer.Serialize(stringWriter, employees);

                // Return the XML as a response with content type 'text/xml'
                return Content(stringWriter.ToString(), "text/xml");
            }
        }
        // Add by Anjali Agrawal

        // Edit (GET): Display the form to edit an employee
        [HttpGet]
        public async Task<IActionResult> Edit(int? id)
        {
            if (!id.HasValue)
            {
                return View(new Employee()); // Create new employee
            }

            var employee = await _dbContext.Employees.FirstOrDefaultAsync(e => e.Id == id.Value);
            if (employee == null)
            {
                return View("NotFound"); // Employee not found
            }

            return View(employee);
        }
        // Add by Anjali Agrawal
        // Edit (POST): Save employee (Add/Update)
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Employee employeeModel)
        {
            if (!ModelState.IsValid) // Validate form submission
            {
                return View(employeeModel);
            }

            if (employeeModel.Id > 0)
            {
                // Update existing employee
                var existingEmployee = await _dbContext.Employees.FirstOrDefaultAsync(e => e.Id == employeeModel.Id);
                if (existingEmployee != null)
                {
                    existingEmployee.Name = employeeModel.Name;
                    existingEmployee.Email = employeeModel.Email;
                    existingEmployee.Department = employeeModel.Department;

                    _dbContext.Employees.Update(existingEmployee);
                }
            }
            else
            {
                // Add new employee
                await _dbContext.Employees.AddAsync(employeeModel);
            }

            await _dbContext.SaveChangesAsync();
            return RedirectToAction("Index");
        }

        // Remove: Delete employee
        public async Task<IActionResult> Remove(int id)
        {
            var employee = await _dbContext.Employees.FirstOrDefaultAsync(e => e.Id == id);
            if (employee == null)
            {
                return View("NotFound"); // Employee not found
            }

            _dbContext.Employees.Remove(employee);
            await _dbContext.SaveChangesAsync();
            return RedirectToAction("Index");
        }
    }
}
