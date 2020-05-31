using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.Entity;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Inspinia_MVC5_SeedProject.Models
{
    [Table("BienesAsegurados", Schema = "mvc")]
    public partial class BienAsegurado
    {
        public BienAsegurado()//Construtor para enlazar hijo
        {
            this.DetalleCaracteristicas = new HashSet<DetalleCaracteristica>();
            this.DetallesBienesAsegurados = new HashSet<DetalleBienAsegurado>();
            this.DetalleCoberturas = new HashSet<DetalleCobertura>();
            this.DetalleAdendas = new HashSet<DetalleAdenda>();
            this.Reclamos = new HashSet<Reclamo>();
        }

        [Key]
        public int Id { get; set; }

        [Required(ErrorMessage = "El campo {0} es obligatorio")]
        [Range(0, int.MaxValue, ErrorMessage = "El valor debe ser mayor que cero")]
        [Display(Name = "Número de Certificado")]
        public int NumCertificado { get; set; }
        public bool Estado { get; set; }
        
        [StringLength(300, ErrorMessage = "La longitud no debe ser mayor a 300 caracteres")]
        [Display(Name = "Observación")]
        public string Observacion { get; set; }

        //Hijo
        public virtual ICollection<DetalleCaracteristica> DetalleCaracteristicas { get; set; }
        public virtual ICollection<DetalleBienAsegurado> DetallesBienesAsegurados { get; set; }
        public virtual ICollection<DetalleCobertura> DetalleCoberturas { get; set; }
        public virtual ICollection<DetalleAdenda> DetalleAdendas { get; set; }
        public virtual ICollection<Reclamo> Reclamos { get; set; }

    }
}