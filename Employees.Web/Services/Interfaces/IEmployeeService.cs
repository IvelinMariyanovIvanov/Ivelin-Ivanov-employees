using Employees.Data.Models;

namespace Employees.Web.Services.Interfaces
{
    public interface IEmployeeService
    {
        /// <summary>
        /// Dictionary key - employee EmpID and coworker EmpID
        /// Dictionary value - list of projects and days for current project
        /// </summary>
        /// <param name="employees"></param>
        /// <returns></returns>
        public Dictionary<Tuple<int, int>, List<Tuple<int, int>>> GetAllEmployeePairs(List<Employee> employees);

        /// <summary>
        /// There may be more than 1 pair with equal days
        /// Returns pairs with the longest common project period
        /// </summary>
        /// <param name="pairsDic"></param>
        /// <returns></returns>
        public List<KeyValuePair<Tuple<int, int>, List<Tuple<int, int>>>> GetLongestPeriodPair(Dictionary<Tuple<int, int>, List<Tuple<int, int>>> pairsDic);
    }
}
