using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.Entity;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Inspinia_MVC5_SeedProject.Models
{
    [Table("DetalleCuotasAdendas", Schema = "mvc")]
    public partial class DetalleCuotasAdenda
    {

        [Key]
        public int Id { get; set; }

        //[Required(ErrorMessage = "El campo {0} es obligatorio")]
        //[StringLength(5, MinimumLength = 3, ErrorMessage = "La longitud debe estar entre 3 y 5 caracteres")]
        [Display(Name = "Cuota")]
        public string Couta { get; set; }

        //[Required(ErrorMessage = "El campo {0} es obligatorio")]
        [Display(Name = "Vence")]
        [DataType(DataType.Date)]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:yyyy-MM-dd}")]
        public DateTime Vence { get; set; }

        //[Required(ErrorMessage = "El campo {0} es obligatorio")]
        //[Range(1, double.MaxValue, ErrorMessage = "El valor debe ser mayor que cero")]
        [Display(Name = "Monto")]
        public double Monto { get; set; }

        [Display(Name = "Saldo")]
        public double Saldo { get; set; }

        public string Estado { get; set; }

        public bool Deshabilitar { get; set; }

        //Llaves foraneas
        public int AdendaId { get; set; }
        public int CuotaId { get; set; }

        //Padres
        public virtual Adenda Adenda { get; set; }
        public virtual Cuota Cuota { get; set; }


    }
}