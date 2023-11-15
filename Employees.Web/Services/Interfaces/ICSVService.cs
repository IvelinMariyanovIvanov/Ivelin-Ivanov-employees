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

        /// <summary>
        /// Dictionary key - empoyee EmpID and coworker EmpID
        /// Dictionary value - list of projects and days for current project
        /// </summary>
        /// <param name="employees"></param>
        /// <returns></returns>
        public Dictionary<Tuple<int, int>, List<Tuple<int, int>>> GetAllEmployeePairs(List<Employee> employees);

        /// <summary>
        /// May be more than 1 pair with equals days
        /// Return pairs with longest common projects period
        /// </summary>
        /// <param name="pairsDic"></param>
        /// <returns></returns>
        public Dictionary<Tuple<int, int>, List<Tuple<int, int>>> GetLongestPeriodPair(Dictionary<Tuple<int, int>, List<Tuple<int, int>>> pairsDic);
    }
}
