using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.Entity;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Inspinia_MVC5_SeedProject.Models
{
    [Table("Clientes", Schema ="mvc")]
    public partial class Cliente : Persona
    {
      
        public Cliente()//Constructor para enlazar hijo
        {
            this.Polizas = new HashSet<Poliza>();
        }

        [Required(ErrorMessage = "El campo {0} es obligatorio")]
        [Display(Name = "Tipo de Cliente")]
        public string TipoCliente { get; set; }

        [Required(ErrorMessage = "El campo {0} es obligatorio")]
        [Display(Name = "Fecha de Nacimiento")]
        [DataType(DataType.Date)]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:yyyy-MM-dd}")]
        public DateTime FechaNacimiento { get; set; }

        //Hijo
        public virtual new ICollection<Poliza> Polizas { get; set; }


    }
}