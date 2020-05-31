using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.Entity;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Inspinia_MVC5_SeedProject.Models
{
    [Table("ReciboCuotas", Schema = "mvc")]
    public partial class ReciboCuota
    {
        [Key]
        public int Id { get; set; }

        [Required(ErrorMessage = "El campo {0} es obligatorio")]
        [Display(Name = "Fecha")]
        [DataType(DataType.Date)]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:yyyy-MM-dd}")]
        public DateTime Fecha { get; set; }

        [Required(ErrorMessage = "El campo {0} es obligatorio")]
        [StringLength(20, MinimumLength = 3, ErrorMessage = "La longitud debe estar entre 3 y 20 caracteres")]
        [Display(Name = "Número de Recibo")]
        public string NumRecibo { get; set; }
        
        [Display(Name = "Pago")]
        public double Pago { get; set; } 
        
        [Display(Name = "Pago Neto")]
        public double PagoNeto { get; set; }

        //Llave Foranea
        public int CuotaId { get; set; }

        //Padre
        public virtual Cuota Cuota { get; set; }

    }
}