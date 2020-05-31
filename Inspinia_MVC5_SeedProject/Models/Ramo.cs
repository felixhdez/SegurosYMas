using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.Entity;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Inspinia_MVC5_SeedProject.Models
{
    [Table("Ramos", Schema = "mvc")]
    public partial class Ramo
    {
        public Ramo()
        {
            this.Productos = new HashSet<Producto>();
        }


        [Key]
        public int Id { get; set; }

        [Required(ErrorMessage = "El campo {0} es obligatorio")]
        [StringLength(100, MinimumLength = 3, ErrorMessage = "La longitud debe estar entre 3 y 10 caracteres")]
        [Display(Name = "Descripción")]
        public string Descripcion { get; set; }

        //Hijo
        public virtual ICollection<Producto> Productos { get; set; }
    }
}