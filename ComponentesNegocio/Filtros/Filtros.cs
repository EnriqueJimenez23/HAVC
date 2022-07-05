using System;
using System.Linq;
using CapaDatos.Repositorio.EF6;
using CapaDominio.EntidadesNegocio;

namespace CapaDominio.ComponentesNegocio.Filtros
{
    internal class VariableConfiguracionFiltro : QueryObject<VariableConfiguracion>
    {
        public VariableConfiguracionFiltro FiltrarPorCategoria(CategoriaVariableConfiguracion? categoriaVariableConfiguracion)
        {
            if (categoriaVariableConfiguracion != null)
                And(x => x.Categoria == categoriaVariableConfiguracion);

            return this;
        }

        public VariableConfiguracionFiltro FiltrarPorNombre(string nombre)
        {
            if (!string.IsNullOrWhiteSpace(nombre))
                And(x => x.Nombre.Contains(nombre.Trim()));

            return this;
        }

        public VariableConfiguracionFiltro FiltrarPorDescripcion(string descripcion)
        {
            if (!string.IsNullOrWhiteSpace(descripcion))
                And(x => x.Descripcion.Contains(descripcion.Trim()));

            return this;
        }
    }

    internal class CatalogoFiltro : QueryObject<Catalogo>
    {
        public CatalogoFiltro FiltrarPorCategoria(CategoriaCatalogo? categoriaCatalogo)
        {
            if (categoriaCatalogo != null)
                And(x => x.Categoria == categoriaCatalogo);

            return this;
        }

        public CatalogoFiltro FiltrarPorEtiqueta(string etiqueta)
        {
            if (!string.IsNullOrWhiteSpace(etiqueta))
                And(x => x.Etiqueta.Contains(etiqueta.Trim()));

            return this;
        }

        public CatalogoFiltro FiltrarPorDescripcion(string descripcion)
        {
            if (!string.IsNullOrWhiteSpace(descripcion))
                And(x => x.Descripcion.Contains(descripcion.Trim()));

            return this;
        }
    }

    internal class RegistroOperacionFiltro : QueryObject<RegistroOperacion>
    {
        public RegistroOperacionFiltro FiltrarPorCategoria(CategoriaRegistroOperacion? categoriaRegistroOperacion)
        {
            if (categoriaRegistroOperacion != null)
                And(x => x.Categoria == categoriaRegistroOperacion);

            return this;
        }

        public RegistroOperacionFiltro FiltrarPorRegistro(int? registroId)
        {
            if (registroId != null)
                And(x => x.RegistroId == registroId);

            return this;
        }

        public RegistroOperacionFiltro FiltrarPorRegistroId(int registroId)
        {
            if (registroId > 0)
                And(x => x.RegistroId == registroId);

            return this;
        }

        public RegistroOperacionFiltro FiltrarPorUsuario(string usuario)
        {
            if (!string.IsNullOrWhiteSpace(usuario))
                And(x => x.Usuario == usuario);

            return this;
        }

        public RegistroOperacionFiltro FiltrarPorNombreUsuario(string nombreUsuario)
        {
            if (!string.IsNullOrWhiteSpace(nombreUsuario))
                And(x => x.NombreUsuario.Contains(nombreUsuario));

            return this;
        }

        public RegistroOperacionFiltro FiltrarPorDescripcion(string descripcion)
        {
            if (!string.IsNullOrWhiteSpace(descripcion))
                And(x => x.DescripcionOperacion.Contains(descripcion));

            return this;
        }

        public RegistroOperacionFiltro FiltrarPorFechaInicial(DateTime? fechaInicial)
        {
            if (fechaInicial != null)
                And(x => x.FechaOperacion >= fechaInicial);

            return this;
        }

        public RegistroOperacionFiltro FiltrarPorFechaFinal(DateTime? fechaFinal)
        {
            if (fechaFinal != null)
                And(x => x.FechaOperacion <= fechaFinal);

            return this;
        }
    }
    internal class PerfilFiltro : QueryObject<Perfil>
    {
        public PerfilFiltro FiltrarPorNombre(string nombrePerfil)
        {
            if (!string.IsNullOrWhiteSpace(nombrePerfil))
                And(x => x.NombrePerfil.Contains(nombrePerfil.Trim()));

            return this;
        }
    }
    internal class UsuarioFiltro : QueryObject<Usuario>
    {
        public UsuarioFiltro FiltrarPorSubRegion(string valor)
        {
            if (!string.IsNullOrWhiteSpace(valor))
                And(x => x.Subregion == valor);
            return this;
        }
        public UsuarioFiltro FiltrarPorDepartamento(string valor)
        {
            if (!string.IsNullOrWhiteSpace(valor))
                And(x => x.CodDepartamento == valor);
            return this;
        }
        public UsuarioFiltro FiltrarPorMunicipio(string valor)
        {
            if (!string.IsNullOrWhiteSpace(valor))
                And(x => x.CodMunicipio == valor);
            return this;
        }
        public UsuarioFiltro FiltrarPorNombreUsuario(string nombreUsuario)
        {
            if (!string.IsNullOrWhiteSpace(nombreUsuario))
                And(x => x.NombreUsuario.Contains(nombreUsuario.Trim()));

            return this;
        } 
        
        public UsuarioFiltro FiltrarPorNombre(string nombre)
        {
            if (!string.IsNullOrWhiteSpace(nombre))
                And(x => x.NombreCompleto.Contains(nombre.Trim()));

            return this;
        }

        public UsuarioFiltro FiltrarPorPerfil(string perfil)
        {
            if (!string.IsNullOrWhiteSpace(perfil))
                And(x => x.Perfil.NombrePerfil == perfil);

            return this;
        }
    }
    internal class ProyectoFiltro : QueryObject<Proyecto>
    {
        public ProyectoFiltro FiltrarPorSubRegion(string valor)
        {
            if (!string.IsNullOrWhiteSpace(valor))
                And(x => x.Subregion == valor);
            return this;
        }
        public ProyectoFiltro FiltrarPorDepartamento(string valor)
        {
            if (!string.IsNullOrWhiteSpace(valor))
                And(x => x.CodDepartamento == valor);
            return this;
        }
        public ProyectoFiltro FiltrarPorMunicipio(string valor)
        {
            if (!string.IsNullOrWhiteSpace(valor))
                And(x => x.CodMunicipio == valor);
            return this;
        }
        public ProyectoFiltro FiltrarPorNombre(string valor)
        {
            if (!string.IsNullOrWhiteSpace(valor))
                And(x => x.NombreProyecto.Contains(valor));
            return this;
        }
        public ProyectoFiltro FiltrarPorPunto(string valor)
        {
            if (!string.IsNullOrWhiteSpace(valor))
                And(x => x.Punto == valor);
            return this;
        }
        public ProyectoFiltro FiltrarPorPilar(string valor)
        {
            if (!string.IsNullOrWhiteSpace(valor))
                And(x => x.Pilar == valor);
            return this;
        }
        public ProyectoFiltro FiltrarPorSector(string valor)
        {
            if (!string.IsNullOrWhiteSpace(valor))
                And(x => x.Sector == valor);
            return this;
        }
        public ProyectoFiltro FiltrarPorEntidad(string valor)
        {
            if (!string.IsNullOrWhiteSpace(valor))
                And(x => x.EntidadResponsable == valor);
            return this;
        }
        public ProyectoFiltro FiltrarPorEstado(string valor)
        {
            if (!string.IsNullOrWhiteSpace(valor))
                And(x => x.EstadoProyecto == valor);
            return this;
        }
    }
    internal class VeedorFiltro : QueryObject<Veedor>
    {
        public VeedorFiltro FiltrarPorSubRegion(string valor)
        {
            if (!string.IsNullOrWhiteSpace(valor))
                And(x => x.Subregion == valor);
            return this;
        }
        public VeedorFiltro FiltrarPorDepartamento(string valor)
        {
            if (!string.IsNullOrWhiteSpace(valor))
                And(x => x.CodDepartamento == valor);
            return this;
        }
        public VeedorFiltro FiltrarPorMunicipio(string valor)
        {
            if (!string.IsNullOrWhiteSpace(valor))
                And(x => x.CodMunicipio == valor);
            return this;
        }
    }

    internal class PasosFiltro : QueryObject<Pasos>
    {
        public PasosFiltro FiltrarPorSubRegion(string valor)
        {
            if (!string.IsNullOrWhiteSpace(valor))
                And(x => x.Subregion == valor);
            return this;
        }
        public PasosFiltro FiltrarPorDepartamento(string valor)
        {
            if (!string.IsNullOrWhiteSpace(valor))
                And(x => x.CodDepartamento == valor);
            return this;
        }
        public PasosFiltro FiltrarPorMunicipio(string valor)
        {
            if (!string.IsNullOrWhiteSpace(valor))
                And(x => x.CodMunicipio == valor);
            return this;
        }
        public PasosFiltro FiltrarPorNombre(string valor)
        {
            if (!string.IsNullOrWhiteSpace(valor))
                And(x => x.NombreObjeto.Contains(valor));
            return this;
        }
            
        public PasosFiltro FiltrarPorEntidad(string valor)
        {
            if (!string.IsNullOrWhiteSpace(valor))
                And(x => x.EntidadResponsable == valor);
            return this;
        }
        public PasosFiltro FiltrarPorUsuario(string valor)
        {
            if (!string.IsNullOrWhiteSpace(valor))
                And(x => x.Usuario == valor);
            return this;
        }
    }

}
