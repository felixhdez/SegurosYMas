using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.Entity;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Inspinia_MVC5_SeedProject.Models
{
    [Table("Personas", Schema = "mvc")]
    public partial class Persona
    {
        public Persona()//Constructor para enlazar hijo
        {
            this.Polizas = new HashSet<Poliza>();
        }

        [Key]
        public int Id { get; set; }

        [Required(ErrorMessage = "El campo {0} es obligatorio")]
        [StringLength(100, MinimumLength = 3, ErrorMessage = "La longitud debe estar entre 3 y 100 caracteres")]
        [Display(Name = "Apellidos")]
        public string Apellidos { get; set; }

        [Required(ErrorMessage = "El campo {0} es obligatorio")]
        [StringLength(100, MinimumLength = 3, ErrorMessage = "La longitud debe estar entre 3 y 100 caracteres")]
        [Display(Name = "Nombres")]
        public string Nombres { get; set; }

        [Required(ErrorMessage = "El campo {0} es obligatorio")]
        [StringLength(50, MinimumLength = 3, ErrorMessage = "La longitud debe estar entre 3 y 50 caracteres")]
        [Display(Name = "Identificación")]
        public string Identificacion { get; set; }

        [Required(ErrorMessage = "El campo {0} es obligatorio")]
        [StringLength(250, MinimumLength = 3, ErrorMessage = "La longitud debe estar entre 3 y 250 caracteres")]
        [Display(Name = "Dirección")]
        public string Direccion { get; set; }

        [Required(ErrorMessage = "El campo {0} es obligatorio")]
        [StringLength(20, ErrorMessage = "La longitud debe ser menor 20 caracteres")]
        [Display(Name = "Teléfono 1")]
        public string NumTelf1 { get; set; }

        [StringLength(20, ErrorMessage = "La longitud debe ser menor a 20 caracteres")]
        [Display(Name = "Teléfono 2")]
        public string NumTelf2 { get; set; }

        [StringLength(20, ErrorMessage = "La longitud debe ser menor a 20 caracteres")]
        [Display(Name = "Teléfono 3")]
        public string NumTelf3 { get; set; }

        [StringLength(20, ErrorMessage = "La longitud debe ser menor a 20 caracteres")]
        [Display(Name = "Celular")]
        public string Celular { get; set; }

        [StringLength(50, ErrorMessage = "La longitud debe ser menor a 50 caracteres")]
        [Display(Name = "E-mail")]
        public string Email { get; set; }

        [StringLength(300, ErrorMessage = "La longitud debe ser menor a 300 caracteres")]
        [Display(Name = "Notas")]
        public string Notas { get; set; }

        //Llave Foranea
        public int DepartamentoId {get; set;}

        //Padre
        public virtual Departamento Departamento { get; set; }

        //Hijo
        public virtual ICollection<Poliza> Polizas { get; set; }


        //Otras
        [NotMapped]
        public string NombreCompleto { get { return this.Nombres + " " + this.Apellidos; } }

        [NotMapped]
        public string MostrarIdent { get  { return this.Identificacion + " | " + this.Nombres + " " + this.Apellidos; } }
    }
}