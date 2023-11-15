using Employees.Data.Models;
using Employees.Web.Services.Interfaces;

namespace Employees.Web.Services
{
    public class EmployeeService : IEmployeeService
    {
        /// <summary>
        /// Dictionary key - empoyee EmpID and coworker EmpID
        /// Dictionary value - list of projects and days for current project
        /// </summary>
        /// <param name="employees"></param>
        /// <returns></returns>
        public Dictionary<Tuple<int, int>, List<Tuple<int, int>>> GetAllEmployeePairs(List<Employee> employees)
        {

            Dictionary<Tuple<int, int>, List<Tuple<int, int>>> pairsDic = new Dictionary<Tuple<int, int>, List<Tuple<int, int>>>();

            for (int i = 0; i < employees.Count(); i++)
            {
                Employee employee = employees[i];

                if (employee.DateTo == null)
                    employee.DateTo = DateTime.UtcNow;

                var coWorkers = employees
                    .Where(e => e.ProjectID == employee.ProjectID && e.EmpID != employee.EmpID)
                    .OrderBy(e => employee.EmpID).ToList();

                foreach (Employee coWorker in coWorkers)
                {
                    if (coWorker.DateTo == null)
                        coWorker.DateTo = DateTime.UtcNow;

                    bool didEmployeesWorkTogether =
                        employee.DateFrom < coWorker.DateTo && coWorker.DateFrom < employee.DateTo;

                    if (didEmployeesWorkTogether)
                    {
                        // do not add twice employee and coworker
                        if (employee.EmpID < coWorker.EmpID)
                        {
                            int daysInProject = GetDaysInCurrentProject
                           (employee.DateFrom, (DateTime)employee.DateTo, coWorker.DateFrom, (DateTime)coWorker.DateTo);

                            Tuple<int, int> emplPairKey = new Tuple<int, int>(employee.EmpID, coWorker.EmpID);
                            Tuple<int, int> emplProjectValue = new Tuple<int, int>(employee.ProjectID, daysInProject);

                            if (pairsDic.ContainsKey(emplPairKey))
                            {
                                pairsDic[emplPairKey].Add(emplProjectValue);
                            }
                            else
                            {
                                pairsDic[emplPairKey] = new List<Tuple<int, int>>() { emplProjectValue };
                            }
                        }
                    }
                }
            }

            return pairsDic;
        }

        /// <summary>
        /// May be more than 1 pair with equals days
        /// Return pairs with longest common projects period
        /// </summary>
        /// <param name="pairsDic"></param>
        /// <returns></returns>
        public Dictionary<Tuple<int, int>, List<Tuple<int, int>>> GetLongestPeriodPair(Dictionary<Tuple<int, int>, List<Tuple<int, int>>> pairsDic)
        {
            int longestPeriodDays = 0;
            KeyValuePair<Tuple<int, int>, List<Tuple<int, int>>> maxPair = default;
            Dictionary<Tuple<int, int>, List<Tuple<int, int>>> maxPairs = new Dictionary<Tuple<int, int>, List<Tuple<int, int>>>();

            foreach (var pair in pairsDic)
            {
                int tempMaxDays = 0;
                List<Tuple<int, int>> projects = pair.Value;

                foreach (Tuple<int, int> project in projects)
                {
                    tempMaxDays += project.Item2;

                    if (tempMaxDays > longestPeriodDays)
                    {
                        longestPeriodDays = tempMaxDays;
                        maxPair = pair;
                    }

                    if (tempMaxDays == longestPeriodDays)
                        maxPairs[pair.Key] = projects;
                }
            }

            return maxPairs;
        }

        /// <summary>
        /// Get days in common project for two employees
        /// </summary>
        /// <param name="firstStart"></param>
        /// <param name="firstEnd"></param>
        /// <param name="secondStart"></param>
        /// <param name="secondEnd"></param>
        /// <returns></returns>
        private int GetDaysInCurrentProject(DateTime firstStart, DateTime firstEnd, DateTime secondStart, DateTime secondEnd)
        {
            DateTime maxStart = firstStart > secondStart ? firstStart : secondStart;
            DateTime minEnd = firstEnd < secondEnd ? firstEnd : secondEnd;
            TimeSpan interval = minEnd - maxStart;

            return interval.Days;
        }
    }
}
