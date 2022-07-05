using CapaDominio.EntidadesNegocio;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace CsWeb.Models
{
    public class NivelExamenViewModel
    {
        public string ncaso { get; set; }
        public DateTime FechaIngreso { get; set; }
        public string Entidad { get; set; }
        public string Solicitante { get; set; }
        public string Propietario { get; set; }
        public string Paciente { get; set; }
        public string Especie { get; set; }
        public string Combo { get; set; }
        public string Examen { get; set; }
        public string Area { get; set; }
        public string Estado { get; set; }
    }
}