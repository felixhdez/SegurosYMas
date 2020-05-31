using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.Entity;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Inspinia_MVC5_SeedProject.Models
{
    [Table("Reclamos", Schema = "mvc")]
    public partial class Reclamo
    {
        public Reclamo()//Constructor para enlazar hijo
        {
            this.DetalleDocumentos = new HashSet<DetalleDocumento>();
            this.DetallePagos = new HashSet<DetallePago>();
            this.ArchivosReclamos = new HashSet<ArchivosReclamos>();
        }

        [Key]
        public int Id { get; set; }

        [Required(ErrorMessage = "El campo {0} es obligatorio")]
        [StringLength(100, MinimumLength = 3, ErrorMessage = "La longitud debe estar entre 3 y 100 caracteres")]
        [Display(Name = "Número de Reclamo")]
        public string NumReclamo { get; set; }

        //[Required(ErrorMessage = "El campo {0} es obligatorio")]
        //[StringLength(20, MinimumLength = 3, ErrorMessage = "La longitud debe estar entre 3 y 20 caracteres")]
        [Display(Name = "Tipo de Reclamo")]
        public string TipoReclamo { get; set; }

        [Required(ErrorMessage = "El campo {0} es obligatorio")]
        [StringLength(100, MinimumLength = 3, ErrorMessage = "La longitud debe estar entre 3 y 100 caracteres")]
        [Display(Name = "Dependiente")]
        public string Dependiente { get; set; }

        [Required(ErrorMessage = "El campo {0} es obligatorio")]
        [StringLength(100, MinimumLength = 3, ErrorMessage = "La longitud debe estar entre 3 y 100 caracteres")]
        [Display(Name = "Ajustador")]
        public string Ajustador { get; set; }

        [Required(ErrorMessage = "El campo {0} es obligatorio")]
        [Display(Name = "Fecha de Ingreso")]
        [DataType(DataType.Date)]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:yyyy-MM-dd}")]
        public DateTime FechaAviso { get; set; }

        [Required(ErrorMessage = "El campo {0} es obligatorio")]
        [Display(Name = "Fecha de Siniestro")]
        [DataType(DataType.Date)]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:yyyy-MM-dd}")]
        public DateTime FechaSiniestro { get; set; }

        [Required(ErrorMessage = "El campo {0} es obligatorio")]
        [StringLength(300, MinimumLength = 3, ErrorMessage = "La longitud debe estar entre 3 y 300 caracteres")]
        [Display(Name = "Lugar de Ocurrencia")]
        public string LugarOcurrencia { get; set; }

        [Required(ErrorMessage = "El campo {0} es obligatorio")]
        [Range(1, double.MaxValue, ErrorMessage = "El valor debe ser mayor que cero")]
        [Display(Name = "Monto reclamado")]
        public double MontoReclamado { get; set; }

        [Display(Name="Responsable")]
        public string Responsable { get; set; }

        //Llave Foraneas
        public int PolizaId { get; set; }
        public int BienAseguradoId { get; set; }
        public int CoberturaReclamoId { get; set; }

        //Padre
        public virtual Poliza Poliza { get; set; }
        public virtual BienAsegurado BienAsegurado { get; set; }
        public virtual CoberturaReclamo CoberturaReclamo { get; set; }

        //Hijo
        public virtual ICollection<DetalleDocumento> DetalleDocumentos { get; set; }
        public virtual ICollection<DetallePago> DetallePagos { get; set; }
        public virtual ICollection<ArchivosReclamos> ArchivosReclamos { get; set; }


    }
}