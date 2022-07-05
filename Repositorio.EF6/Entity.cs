using System.ComponentModel.DataAnnotations.Schema;
using CapaDatos.Repositorio.Infrastructure;

namespace CapaDatos.Repositorio.EF6
{
    public abstract class Entity : IObjectState
    {
        [NotMapped]
        public ObjectState ObjectState { get; set; }
    }
}