using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.Entity;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Inspinia_MVC5_SeedProject.Models
{
    [Table("Caracteristicas", Schema = "mvc")]
    public partial class Caracteristica
    {
        public Caracteristica()//Constructor para enlazar hijo
        {
            this.DetalleCaracteristicas = new HashSet<DetalleCaracteristica>();
        }

        [Key]
        public int Id { get; set; }

        [Required(ErrorMessage = "El campo {0} es obligatorio")]
        [StringLength(100, MinimumLength = 3, ErrorMessage = "La longitud debe estar entre 3 y 100 caracteres")]
        [Display(Name = "Descripción")]
        public string Descripcion { get; set; }

        //Hijo
        public virtual ICollection<DetalleCaracteristica> DetalleCaracteristicas { get; set; }

    }
}