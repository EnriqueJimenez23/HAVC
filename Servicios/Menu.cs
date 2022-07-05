using System.Collections.Generic;

namespace CapaServicios.Servicios
{
    public class Menu
    {
        #region Properties

        public string Url { get; set; }
        public string Etiqueta { get; set; }
        public string Icono { get; set; }
        public bool EstaSeleccionado { get; set; }
        public bool AbrirEnNuevaVentana { get; set; }
        public List<Menu> SubMenus { get; set; }

        #endregion

        #region Constructor

        public Menu()
        {
            Url = string.Empty;
            Etiqueta = string.Empty;
            Icono = string.Empty;
            EstaSeleccionado = false;
            AbrirEnNuevaVentana = false;
            SubMenus = new List<Menu>();
        }

        #endregion
    }
}
