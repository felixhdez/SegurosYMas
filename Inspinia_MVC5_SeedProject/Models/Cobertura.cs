using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.Entity;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Inspinia_MVC5_SeedProject.Models
{
    [Table("Coberturas", Schema = "mvc")]
    public partial class Cobertura
    {
        public Cobertura()//Constructor para enlazar hijo
        {
            this.DetalleCoberturas = new HashSet<DetalleCobertura>();
        }

        [Key]
        public int Id { get; set; }

        [Required(ErrorMessage = "El campo {0} es obligatorio")]
        [StringLength(300, MinimumLength = 3, ErrorMessage = "La longitud debe estar entre 3 y 300 caracteres")]
        [Display(Name = "Descripción")]
        public string Descripcion { get; set; }

        //Hijo
        public virtual ICollection<DetalleCobertura> DetalleCoberturas { get; set; }

    }
}