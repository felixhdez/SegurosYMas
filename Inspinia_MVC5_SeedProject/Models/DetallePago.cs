using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.Entity;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Inspinia_MVC5_SeedProject.Models
{
    [Table("DetallePagos", Schema = "mvc")]
    public partial class DetallePago
    {
        [Key]
        public int Id { get; set; }

        //[Required(ErrorMessage = "El campo {0} es obligatorio")]
        //[StringLength(100, MinimumLength = 3, ErrorMessage = "La longitud debe estar entre 3 y 100 caracteres")]
        [Display(Name ="Fecha")]
        public DateTime Fecha { get; set; }

        [Display(Name = "Moneda")]
        public string Moneda { get; set; }

        //[Required(ErrorMessage = "El campo {0} es obligatorio")]
        //[Range(1, double.MaxValue, ErrorMessage = "El valor debe ser mayor que cero")]
        [Display(Name = "Valor")]
        public double Valor { get; set; }

        //[Required(ErrorMessage = "El campo {0} es obligatorio")]
        //[Range(1, int.MaxValue, ErrorMessage = "El valor debe ser mayor que cero")]
        [Display(Name = "Número de Documento")]
        public int NumDoc { get; set; }

        //[Required(ErrorMessage = "El campo {0} es obligatorio")]
        //[StringLength(100, MinimumLength = 3, ErrorMessage = "La longitud debe estar entre 3 y 100 caracteres")]
        [Display(Name = "Banco/Taller")]
        public string BancoTaller { get; set; }

        
        //[StringLength(300, MinimumLength = 3, ErrorMessage = "La longitud debe estar entre 3 y 300 caracteres")]
        [Display(Name = "Notas")]
        public string Nota { get; set; }

        //Llave Foranea
        public int TipoDePagoId { get; set; }
        public int ReclamoId { get; set; }

        //Padre
        public virtual TipoDePago TipoDePago { get; set; }
        public virtual Reclamo Reclamo { get; set; }

    }
}