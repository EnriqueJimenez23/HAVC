using System.ComponentModel.DataAnnotations;
using System.IO;

namespace CsWeb.Models
{
    public class FileNombreViewModel
    {
        public MemoryStream ms { get; set; }
        public string nombreFile { get; set; }
    }
}