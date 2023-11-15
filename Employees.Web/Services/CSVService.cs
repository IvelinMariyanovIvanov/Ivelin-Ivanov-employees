using CsvHelper;
using CsvHelper.TypeConversion;
using Employees.Data.Models;
using Employees.Web.Services.Interfaces;
using System.Globalization;

namespace Employees.Web.Services
{
    public class CSVService : ICSVService
    {
        /// <summary>
        /// Read csv file
        /// </summary>
        /// <typeparam name="T"> Employee </typeparam>
        /// <param name="file"> uploaded file</param>
        /// <returns></returns>
        public IEnumerable<T> ReadCSV<T>(Stream file)
        {
            var reader = new StreamReader(file);
            var csvReader = new CsvReader(reader, CultureInfo.InvariantCulture);

            // set null value for DateTime
            csvReader.Context.TypeConverterOptionsCache.GetOptions<DateTime?>()
                .NullValues.AddRange(new[] { "NULL", DateTime.UtcNow.ToString() });

            var options = new TypeConverterOptions { 
                Formats = new[] 
                { 
                    "MM/dd/yyyy" ,
                    "MM/dd/yy",
                    "dd/MM/yy",
                    "dd-MM-yy",
                    "ddd, dd MMM yyyy",
                    "dddd, dd MMMM yy",
                    "dddd, dd MMMM yyyy HH:mm",
                    "MM/dd/yy HH:mm",
                    "MM/dd/yyyy hh:mm tt",
                    "MM/dd/yyyy H:mm t",
                    "MM/dd/yyyy H:mm:ss",
                    "MMM dd",
                    "MM-dd-yyyTHH:mm:ss.fff",
                    "MM-dd-yyy g",
                    "MM-dd-yyyTHH:mm:ss",
                    "yyyy-MM-dd",
                    "yyyy/MM/dd",
                    "yyyy:MM:dd",
                } 
            };

            csvReader.Context.TypeConverterOptionsCache.AddOptions<DateTime>(options);
            var records =  csvReader.GetRecords<T>();

            return records;
        }
    }
}
