using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Nutrimenez.Models
{
    public class Consulta
    {
        public int Id { get; set; }
        [Display(Name = "Descripción de la Consulta")]
        [Required(ErrorMessage = "La descripción  es un campo requerido.")]
        public string Descripcion { get; set; }
        [Display(Name = "Fecha de Consulta")]
        [Required(ErrorMessage = "La fecha en que se realiza la consulta es un campo requerido.")]
        [DataType(DataType.Date)]
        public DateTime FechaAviso { get; set; }

        [Display(Name = "Usuario")]
        public int UsuarioId { get; set; }
        [Display(Name = "Tipo de Consulta")]
        public int TipoConsultaId { get; set; }

        public Usuario Usuario { get; set; }
        public TipoConsulta TipoConsulta { get; set; }
    }
}
