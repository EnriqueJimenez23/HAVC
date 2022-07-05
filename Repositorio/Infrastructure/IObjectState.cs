
using System.ComponentModel.DataAnnotations.Schema;

namespace CapaDatos.Repositorio.Infrastructure
{
    public interface IObjectState
    {
        [NotMapped]
        ObjectState ObjectState { get; set; }
    }
}