using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.Entity;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Inspinia_MVC5_SeedProject.Models
{
    [Table("Agentes", Schema = "mvc")]
    public partial class Agente
    {
        public Agente()//Constructor para enlazar hijo
        {
            this.Polizas = new HashSet<Poliza>();
        }

        [Key]
        public int Id { get; set; }

        [Required(ErrorMessage = "El campo {0} es obligatorio")]
        [StringLength(150, MinimumLength = 3, ErrorMessage = "La longitud debe estar entre 3 y 150 caracteres")]
        [Display(Name = "Nombre Completo")]
        public string NombreCompleto { get; set; }

        //Hijo
        public virtual ICollection<Poliza> Polizas { get; set; }
    }
}