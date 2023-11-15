using System.ComponentModel.DataAnnotations;
using System.ComponentModel;

namespace Employees.Web.ViewModels
{
    public class UploadFileVM
    {
        [Required(ErrorMessage = "Please upload a file")]
        [DisplayName("Upload csv file")]
        public IFormFile File { get; set; }

        /// <summary>
        /// Pairs of employees who have worked together on common projects
        /// May be more than 1 pair
        /// </summary>  
        public Dictionary<Tuple<int, int>, List<Tuple<int, int>>> MaxEmployeePairs = new Dictionary<Tuple<int, int>, List<Tuple<int, int>>>();

        public List<string> Errors { get; set; } = new List<string>();
    }
}
