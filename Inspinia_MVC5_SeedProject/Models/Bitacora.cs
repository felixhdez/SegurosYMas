using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.Entity;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Inspinia_MVC5_SeedProject.Models
{
    [Table("Bitacoras", Schema = "mvc")]
    public partial class Bitacora
    {
        [Key]
        public int Id { get; set; }

        [Display(Name = "Fecha")]
        [DataType(DataType.Date)]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:yyyy-MM-dd}")]
        public DateTime Fecha { get; set; }
        
        [StringLength(1000, ErrorMessage = "La longitud debe ser menor a 1000 caracteres")]
        [Display(Name = "Observación")]
        public string Observacion { get; set; }

        //Llave Foranea
        public int PolizaId { get; set; }

        //Padre
        public virtual Poliza Poliza { get; set; }

    }
}