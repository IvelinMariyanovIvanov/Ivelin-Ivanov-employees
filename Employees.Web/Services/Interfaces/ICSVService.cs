using Employees.Data.Models;
using Employees.Web.ViewModels;

namespace Employees.Web.Services.Interfaces
{
    public interface ICSVService
    {
        /// <summary>
        /// Read csv file
        /// </summary>
        /// <typeparam name="T"> Employee </typeparam>
        /// <param name="file"> uploaded file</param>
        /// <returns></returns>
        public IEnumerable<T> ReadCSV<T>(Stream file);
    }
}
