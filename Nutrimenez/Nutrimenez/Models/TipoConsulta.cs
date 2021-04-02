using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Nutrimenez.Models
{
    public class TipoConsulta
    {
        public int Id { get; set; }
        [Display(Name = "Descripción")]
        [Required(ErrorMessage = "La descripción del tipo de consulta es un campo requerido.")]
        public string Descripcion { get; set; }
        public ICollection<Consulta> Avisos { get; set; }
    }
}
