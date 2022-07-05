using System;
using CapaDatos.Repositorio.DataContext;
using CapaDatos.Repositorio.EF6;
using CapaDatos.Repositorio.Repositories;
using CapaDatos.Repositorio.UnitOfWork;
using CapaDominio.ComponentesNegocio;
using CapaDominio.EntidadesNegocio;
using Microsoft.Practices.Unity;

namespace CsWeb.App_Start
{
    /// <summary>
    /// Specifies the Unity configuration for the main container.
    /// </summary>
    public class UnityConfig 
    {
        #region Unity Container
        private static Lazy<IUnityContainer> container = new Lazy<IUnityContainer>(() =>
        {
            var container = new UnityContainer();
            RegisterTypes(container);
            return container;
        });

        /// <summary>
        /// Gets the configured Unity container.
        /// </summary>
        public static IUnityContainer GetConfiguredContainer()
        {
            return container.Value;
        }
        #endregion

        /// <summary>Registers the type mappings with the Unity container.</summary>
        /// <param name="container">The unity container to configure.</param>
        /// <remarks>There is no need to register concrete types such as controllers or API controllers (unless you want to 
        /// change the defaults), as Unity allows resolving a concrete type even if it was not previously registered.</remarks>
        public static void RegisterTypes(IUnityContainer container)
        {
            // NOTE: To load from web.config uncomment the line below. Make sure to add a Microsoft.Practices.Unity.Configuration to the using statements.
            // container.LoadConfiguration();

            // TODO: Register your types here
            // container.RegisterType<IProductRepository, ProductRepository>();

            container
                .RegisterType<IDataContextAsync, CsWebContext>(new PerRequestLifetimeManager())
                .RegisterType<IUnitOfWorkAsync, UnitOfWork>(new PerRequestLifetimeManager())
                .RegisterType<IRepositoryAsync<Catalogo>, Repository<Catalogo>>()
                .RegisterType<IRepositoryAsync<CatalogoT>, Repository<CatalogoT>>()
                .RegisterType<IRepositoryAsync<Municipios>, Repository<Municipios>>()
                .RegisterType<IRepositoryAsync<VariableConfiguracion>, Repository<VariableConfiguracion>>()
                .RegisterType<IRepositoryAsync<RegistroOperacion>, Repository<RegistroOperacion>>()
                .RegisterType<IRepositoryAsync<Perfil>, Repository<Perfil>>()
                .RegisterType<IRepositoryAsync<Usuario>, Repository<Usuario>>()
                .RegisterType<IRepositoryAsync<Permiso>, Repository<Permiso>>()
                .RegisterType<IRepositoryAsync<Pasos>, Repository<Pasos>>()
                .RegisterType<IRepositoryAsync<Pasos3>, Repository<Pasos3>>()
                .RegisterType<IRepositoryAsync<Pasos4>, Repository<Pasos4>>()
                .RegisterType<IRepositoryAsync<Pasos5>, Repository<Pasos5>>()
                .RegisterType<IRepositoryAsync<Pasos6>, Repository<Pasos6>>()
                .RegisterType<IRepositoryAsync<Pasos8>, Repository<Pasos8>>()
                .RegisterType<IRepositoryAsync<Pasos9>, Repository<Pasos9>>()
                .RegisterType<IRepositoryAsync<DatosGeo>, Repository<DatosGeo>>()
                .RegisterType<IRepositoryAsync<ContenidoSitio>, Repository<ContenidoSitio>>()
                .RegisterType<IRepositoryAsync<Proyecto>, Repository<Proyecto>>()
                .RegisterType<IRepositoryAsync<ProyectoAvance>, Repository<ProyectoAvance>>()
                .RegisterType<IRepositoryAsync<Veedor>, Repository<Veedor>>()
                .RegisterType<ICatalogoServicio, CatalogoServicio>()
                .RegisterType<IVariableConfiguracionServicio, VariableConfiguracionServicio>()
                .RegisterType<IUsuariosServicio, UsuariosServicio>()
                .RegisterType<IUsuariosServicio, UsuariosServicio>("UsuariosServicio")
                .RegisterType<IRegistroOperacionServicio, RegistroOperacionServicio>()
                //.RegisterType<IDocumentoServicio, DocumentoServicio>()
                .RegisterType<IAdminServicio, AdminServicio>();
        }
    }
}
