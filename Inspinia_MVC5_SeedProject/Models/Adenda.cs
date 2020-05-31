using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.Entity;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Inspinia_MVC5_SeedProject.Models
{
    [Table("Adendas", Schema = "mvc")]
    public partial class Adenda
    {
        public Adenda()//Constructor para enlazar hijo
        {
            this.DetalleAdendas = new HashSet<DetalleAdenda>();
            this.DetalleCuotasAdendas = new HashSet<DetalleCuotasAdenda>();
            this.ArchivosAdendas = new HashSet<ArchivosAdendas>();
        }

        [Key]
        public int Id { get; set; }

        [Required(ErrorMessage = "El campo {0} es obligatorio")]
        [StringLength(10, ErrorMessage = "La longitud no debe ser mayor a 10 caracteres")]
        [Display(Name = "Número de Adenda")]
        public string NumAdenda { get; set; }

        [Display(Name = "Tipo de Adenda")]
        public string TipoAdenda { get; set; }

        [Required(ErrorMessage = "El campo {0} es obligatorio")]
        [Display(Name = "Fecha de Emisión")]
        [DataType(DataType.Date)]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:yyyy-MM-dd}")]
        public DateTime FechaEmision { get; set; }

        [Required(ErrorMessage = "El campo {0} es obligatorio")]
        [Display(Name = "Desde")]
        [DataType(DataType.Date)]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:yyyy-MM-dd}")]
        public DateTime FechaDesde { get; set; }

        [Required(ErrorMessage = "El campo {0} es obligatorio")]
        [Display(Name = "Hasta")]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:yyyy-MM-dd}")]
        [DataType(DataType.Date)]
        public DateTime FechaHasta { get; set; }

        [Required(ErrorMessage = "El campo {0} es obligatorio")]
        [Display(Name = "Suma Asegurada")]
        public double SumaAsegurada { get; set; }

        [Required(ErrorMessage = "El campo {0} es obligatorio")]
        [Display(Name = "Prima Neta")]
        public double PrimaNeta { get; set; }

        [Required(ErrorMessage = "El campo {0} es obligatorio")]
        [Display(Name = "Iva")]
        public double Iva { get; set; }

        [Required(ErrorMessage = "El campo {0} es obligatorio")]
        [Display(Name = "Otros")]
        public double Otros { get; set; }

        [Required(ErrorMessage = "El campo {0} es obligatorio")]
        [Display(Name = "Comisión")]
        public double ComisionEspecial { get; set; }

        [Display(Name = "Tipo de Cambio")]
        public double TipoDeCambio { get; set; }

        public string Comentario { get; set; }

        //Llaves foráneas
        public int PolizaId { get; set; }

        //Hijo
        public virtual ICollection<DetalleAdenda> DetalleAdendas { get; set; }
        public virtual ICollection<DetalleCuotasAdenda> DetalleCuotasAdendas { get; set; }
        public virtual ICollection<ArchivosAdendas> ArchivosAdendas { get; set; }

        //padre
        public virtual Poliza Poliza { get; set; }

    }
}