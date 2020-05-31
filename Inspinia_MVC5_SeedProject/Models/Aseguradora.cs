using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.Entity;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Inspinia_MVC5_SeedProject.Models
{
    [Table("Aseguradoras", Schema = "mvc")]
    public partial class Aseguradora
    {
        public Aseguradora()//Constructor para enlazar hijo
        {
            this.Productos = new HashSet<Producto>();
        }

        [Key]
        public int Id { get; set; }

        [Required(ErrorMessage = "El campo {0} es obligatorio")]
        [StringLength(60, MinimumLength = 3, ErrorMessage = "La longitud debe estar entre 3 y 60 caracteres")]
        [Display(Name = "Descripción")]
        public string Descripcion { get; set; }

        [Display(Name = "Dirección")]
        public string Direccion { get; set; }

        [Display(Name = "Teléfono #1")]
        public string Telefono1 { get; set; }

        [Display(Name = "Teléfono #2")]
        public string Telefono2 { get; set; }

        [Display(Name = "Asistencia")]
        public string Asistencia { get; set; }

        [Required(ErrorMessage = "El campo {0} es obligatorio")]
        [Display(Name = "IR")]
        public double IR { get; set; }

        [Required(ErrorMessage = "El campo {0} es obligatorio")]
        [Display(Name = "IMI")]
        public double IMI { get; set; }

        //Hijo
        public virtual ICollection<Producto> Productos { get; set; }
    }
}