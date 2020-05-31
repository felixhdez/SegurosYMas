using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.Entity;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Inspinia_MVC5_SeedProject.Models
{
    [Table("Polizas", Schema = "mvc")]
    public partial class Poliza
    {
        public Poliza()//Contructor para enlazar hijos
        {
            this.Bitacoras = new HashSet<Bitacora>();
            this.DetallesBienesAsegurados = new HashSet<DetalleBienAsegurado>();
            this.Reclamos = new HashSet<Reclamo>();
            //this.DetalleAdendas = new HashSet<DetalleAdenda>();
            this.Adendas = new HashSet<Adenda>();
            this.DetalleCuotas = new HashSet<DetalleCuota>();
            this.Tramites = new HashSet<Tramite>();
            this.ArchivosPólizas = new HashSet<ArchivosPólizas>();
        }

        [Key]
        public int Id { get; set; }
        
        [Required(ErrorMessage = "El campo {0} es obligatorio")]
        [StringLength(100, MinimumLength = 3, ErrorMessage = "La longitud debe estar entre 3 y 100 caracteres")]
        [Display(Name = "Número de Póliza")]
        public string NumPoliza { get; set; }

        [Display(Name = "Tipo de Moneda")]
        public string TipoMoneda { get; set; }

        [Required(ErrorMessage = "El campo {0} es obligatorio")]
        [Display(Name = "Fecha de Emisión")]
        [DataType(DataType.Date)]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:yyyy-MM-dd}")]
        public DateTime FechaEmision { get; set; }

        [Required(ErrorMessage = "El campo {0} es obligatorio")]
        [Display(Name = "Desde")]
        [DataType(DataType.Date)]
        [DisplayFormat(ApplyFormatInEditMode =true, DataFormatString ="{0:yyyy-MM-dd}")]
        public DateTime FechaDesde { get; set; }

        [Required(ErrorMessage = "El campo {0} es obligatorio")]
        [Display(Name = "Hasta")]
        [DataType(DataType.Date)]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:yyyy-MM-dd}")]
        public DateTime FechaHasta { get; set; }

        
        [Display(Name = "Tipo de Poliza")]
        public string Tipo { get; set; }
        
        [Display(Name = "Forma de Pago")]
        public string FormaDePago { get; set; }

        [Required(ErrorMessage = "El campo {0} es obligatorio")]
        [Display(Name = "Prima Neta")]
        public double PrimaNeta { get; set; }

        [Required(ErrorMessage = "El campo {0} es obligatorio")]
        [Display(Name = "Derecho de Emisión")]
        public double Emision { get; set; }

        [Required(ErrorMessage = "El campo {0} es obligatorio")]
        [Display(Name = "Iva")]
        public double Iva { get; set; }

        [Required(ErrorMessage = "El campo {0} es obligatorio")]
        [Display(Name = "Otros")]
        public double Otros { get; set; }

        [Display(Name = "Comisión por Porcentaje")]
        public double ComisionPorcentaje { get; set; }
        
        [Display(Name = "Comisión por Valor")]
        public double ComisionValor { get; set; }
        
        [Display(Name = "Tipo de Cambio")]
        public double TipoDeCambio { get; set; }

        //Llaves Foraneas
        
        public int ProductoId { get; set; }
        public int AgenteId { get; set; }
        public int ClienteId { get; set; }
        public int ContactoIntermediarioId { get; set; }
        public int PersonaId { get; set; }

        //Padres

        public virtual Producto Producto { get; set; }

        public virtual Agente Agente { get; set; }

        public virtual Cliente Cliente { get; set; }

        public virtual ContactoIntermediario ContactoIntermediario { get; set; }

        public virtual Persona Persona { get; set; }

        //Hijo
        public virtual ICollection<Bitacora> Bitacoras { get; set; }
        public virtual ICollection<DetalleBienAsegurado> DetallesBienesAsegurados { get; set; }
        public virtual ICollection<Reclamo> Reclamos { get; set; }
        //public virtual ICollection<DetalleAdenda> DetalleAdendas { get; set; }
        public virtual ICollection<Adenda> Adendas { get; set; }
        public virtual ICollection<DetalleCuota> DetalleCuotas { get; set; }

        public virtual ICollection<Tramite> Tramites { get; set; }

        public virtual ICollection<ArchivosPólizas> ArchivosPólizas { get; set; }
    }
}