using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.Entity;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Inspinia_MVC5_SeedProject.Models
{
    [Table("DetalleCoberturas", Schema = "mvc")]
    public partial class DetalleCobertura
    {     

        [Key]
        public int Id { get; set; }
        
        [Display(Name = "Suma Asegurada")]
        public double SumaAsegurada { get; set; }

        [Display(Name = "Deducible")]
        public double Deducible { get; set; }
        
        [Display(Name = "Prima")]
        public double Prima { get; set; }

        //Llaves Foraneas
        public int BienAseguradoId { get; set; }
        public int CoberturaId { get; set; }

        //Padre
        public virtual BienAsegurado BienAsegurados { get; set; }
        public virtual Cobertura Coberturas { get; set; }
        
    }
}