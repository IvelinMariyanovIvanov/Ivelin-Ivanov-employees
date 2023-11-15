using CsvHelper.TypeConversion;
using Employees.Data.Models;
using Employees.Web.Services.Interfaces;
using Employees.Web.ViewModels;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;

namespace Employees.Web.Controllers
{
    public class EmployeesController : Controller
    {
        private readonly ICSVService _csvService;

        public EmployeesController(ICSVService csvService)
        {
            _csvService = csvService;
        }

        public IActionResult Upload()
        {
            return View(new UploadFileVM());
        }

        [HttpPost]
        public IActionResult Upload(UploadFileVM form)
        {
            if (!ModelState.IsValid)
                return View(form);

            if(form.File == null || form.File.Length == 0)
            {
                form.Errors.Add($"The file is nul or empty");
                return View(form);
            }

            string fileName = Path.GetFileNameWithoutExtension(form.File.FileName);
            string extension = Path.GetExtension(form.File.FileName);

            if (extension != ".csv")
            {
                form.Errors.Add($"The {fileName} is not a csv file");
                return View(form);  
            }

            try
            {
                List<Employee> employees =
                    _csvService.ReadCSV<Employee>(form.File.OpenReadStream()).OrderBy(e => e.EmpID).ToList();

                if(employees.Count == 0)
                {
                    form.Errors.Add($"There is no records inthe file");
                    return View(form);
                }

                Dictionary<Tuple<int, int>, List<Tuple<int, int>>> pairsDic = _csvService.GetAllEmployeePairs(employees);

                form.MaxEmployeePairs = _csvService.GetLongestPeriodPair(pairsDic);

                if (form.MaxEmployeePairs.Count == 0)
                    form.Errors.Add("There are no employees who have worked together on common projects");

                return View(form);
            }
            // invalid row data
            catch (TypeConverterException ex)
            {
                form.Errors.Add($"{ex.Message}");
                return View(form);
            }
            catch (Exception ex)
            {  
                form.Errors.Add($"{ex.Message}");
                return View(form);
            }
        }
    }
}
