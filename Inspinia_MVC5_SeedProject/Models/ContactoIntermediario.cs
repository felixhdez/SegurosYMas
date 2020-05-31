using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.Entity;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Inspinia_MVC5_SeedProject.Models
{
    [Table("Intermediarios", Schema = "mvc")]
    public partial class ContactoIntermediario
    {
        public ContactoIntermediario()//Constructor para enlazar hijo
        {
            this.Polizas = new HashSet<Poliza>();
        }

        [Key]
        public int Id { get; set; }

        [Required(ErrorMessage = "El campo {0} es obligatorio")]
        [StringLength(100, MinimumLength = 3, ErrorMessage = "La longitud debe estar entre 3 y 100 caracteres")]
        [Display(Name = "Nombres")]
        public string Nombres { get; set; }

        [Required(ErrorMessage = "El campo {0} es obligatorio")]
        [StringLength(100, MinimumLength = 3, ErrorMessage = "La longitud debe estar entre 3 y 100 caracteres")]
        [Display(Name = "Apellidos")]
        public string Apellidos { get; set; }

        [StringLength(20, ErrorMessage = "La longitud debe ser menor a 20 caracteres")]
        [Display(Name = "Teléfono")]
        public string Telefono { get; set; }

        [StringLength(50, ErrorMessage = "La longitud debe ser mayor a 50 caracteres")]
        [Display(Name = "E-mail")]
        public string Email { get; set; }

        [StringLength(100, ErrorMessage = "La longitud debe ser menor a 100 caracteres")]
        [Display(Name = "Cargo")]
        public string Cargo { get; set; }

        //Hijo
        public virtual ICollection<Poliza> Polizas { get; set; }

        //Otras
        [NotMapped]
        public string NombreCompleto{ get { return this.Nombres + " " + this.Apellidos; } }
    }
}