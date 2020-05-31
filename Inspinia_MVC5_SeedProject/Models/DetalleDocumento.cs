using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.Entity;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Inspinia_MVC5_SeedProject.Models
{
    [Table("DetalleDocumentos", Schema = "mvc")]
    public partial class DetalleDocumento
    {
        [Key]
        public int Id { get; set; }

        //[Required(ErrorMessage = "El campo {0} es obligatorio")]
        [Display(Name = "Fecha")]
        [DataType(DataType.Date)]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:yyyy-MM-dd}")]
        public DateTime Fecha { get; set; }

        //[Required(ErrorMessage = "El campo {0} es obligatorio")]
        //[StringLength(100, MinimumLength = 3, ErrorMessage = "La longitud debe estar entre 3 y 100 caracteres")]
        [Display(Name = "Emisor")]
        public string Emisor { get; set; }

        //[Required(ErrorMessage = "El campo {0} es obligatorio")]
        //[Range(1, int.MaxValue, ErrorMessage = "El valor debe ser mayor que cero")]
        [Display(Name = "Número")]
        public int Numero { get; set; }

        //[Required(ErrorMessage = "El campo {0} es obligatorio")]
        //[Range(1, double.MaxValue, ErrorMessage = "El valor debe ser mayor que cero")]
        [Display(Name = "Valor")]
        public double Valor { get; set; }

        //[Required(ErrorMessage = "El campo {0} es obligatorio")]
        //[StringLength(250, MinimumLength = 3, ErrorMessage = "La longitud debe estar entre 3 y 250 caracteres")]
        [Display(Name = "Comentarios")]
        public string Comentarios { get; set; }

        //Llave Foraneas
        public int DocumentoId { get; set; }
        public int ReclamoId { get; set; }

        //Padre
        public virtual Documento Documento { get; set; }
        public virtual Reclamo Reclamo { get; set; }
    }
}