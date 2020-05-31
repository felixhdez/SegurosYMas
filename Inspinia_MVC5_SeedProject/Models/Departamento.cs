using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.Entity;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Inspinia_MVC5_SeedProject.Models
{
    [Table("Departamentos", Schema ="mvc")]
    public partial class Departamento
    {

        public Departamento()//Constructor para enlazar hijo
        {
            this.Personas = new HashSet<Persona>();
        }

        [Key]
        public int Id { get; set; }

        [Required(ErrorMessage = "El campo {0} es obligatorio")]
        [StringLength(80, MinimumLength = 3, ErrorMessage = "La longitud debe estar entre 3 y 80 caracteres")]
        [Display(Name = "Descripción")]
        public string Descripcion { get; set; }

        //Hijo
        public virtual ICollection<Persona> Personas { get; set; }

    }
}