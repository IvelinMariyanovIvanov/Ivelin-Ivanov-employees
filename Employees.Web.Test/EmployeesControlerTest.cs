using Employees.Data.Models;
using Employees.Web.Controllers;
using Employees.Web.Services;
using Employees.Web.Services.Interfaces;
using Employees.Web.ViewModels;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Moq;

namespace Employees.Web.Test
{
    public class EmployeesControlerTest
    {
        private readonly Mock<ICSVService> _mockCsvService;
        private readonly Mock<IEmployeeService> _mockEmployeeService;
        private readonly EmployeesController _controller;

        public EmployeesControlerTest()
        {
            _mockCsvService = new Mock<ICSVService>();
            _mockEmployeeService = new Mock<IEmployeeService>();

            _controller = new EmployeesController(_mockCsvService.Object, _mockEmployeeService.Object);
        }

        /// <summary>
        /// Sholud return true
        /// </summary>
        [Fact]
        public void Upload_ActionExecutes_ReturnsViewForUpload()
        {
            var result = _controller.Upload();

            Assert.IsType<ViewResult>(result);
        }

        /// <summary>
        /// Test with a null csv file should return false
        /// </summary>
        /// <param name="formFile"></param>
        /// <returns></returns>
        [Theory]
        [InlineData(null)]
        public void Upload_With_Null_File(IFormFile formFile)
        {
            // Arrange
            var form = new ViewModels.UploadFileVM { File = formFile };

            // Act - Invoke the method
            var result = _controller.Upload(form);
            ViewResult viewResult = Assert.IsType<ViewResult>(result);
            var model = Assert.IsType<UploadFileVM>(viewResult.Model);

            // Assert
            Assert.Null(model.File);
        }
    }
}