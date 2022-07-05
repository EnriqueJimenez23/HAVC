using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CsWeb.Models
{
    /// <summary>
    /// Definición de objeto que representa el resultado de una operacion Ajax en el controlador
    /// </summary>
    public class Jresult
    {
        public bool Success { get; set; }
        public string Message { get; set; }
        public dynamic Result { get; set; }

        public Jresult()
        {
            Success = false;
            Message = string.Empty;
        }
    }
}