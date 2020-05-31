using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.Entity;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Inspinia_MVC5_SeedProject.Models
{
    [Table("Productos", Schema = "mvc")]
    public partial class Producto
    {
        public Producto()//Constructor para enlazar hijo
        {
            this.Polizas = new HashSet<Poliza>();
        }

        [Key]
        public int Id { get; set; }

        [Required(ErrorMessage = "El campo {0} es obligatorio")]
        [StringLength(10, MinimumLength = 3, ErrorMessage = "La longitud debe estar entre 3 y 10 caracteres")]
        [Display(Name = "Código")]
        public string Codigo { get; set; }

        [Required(ErrorMessage = "El campo {0} es obligatorio")]
        [StringLength(100, MinimumLength = 3, ErrorMessage = "La longitud debe estar entre 3 y 100 caracteres")]
        [Display(Name = "Descripción")]
        public string Descripcion { get; set; }

        [Required(ErrorMessage = "El campo {0} es obligatorio")]
        [Display(Name ="Comisión")]
        public double Comision { get; set; }

        //Llaves foráneas
        [Display(Name = "Aseguradora")]
        public int AseguradoraId { get; set; }

        [Display(Name = "Ramo")]
        public int RamoId { get; set; }

        //Padres
        public virtual Aseguradora Aseguradora { get; set; }
        public virtual Ramo Ramo { get; set; }

        //Hijo
        public virtual ICollection<Poliza> Polizas { get; set; }
    }
}