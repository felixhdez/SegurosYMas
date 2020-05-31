using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.Entity;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Inspinia_MVC5_SeedProject.Models
{
    [Table("Cuotas", Schema = "mvc")]
    public partial class Cuota
    {
        public Cuota()//Constructor para enlazar hijo
        {
            this.DetalleCuotasAdendas = new HashSet<DetalleCuotasAdenda>();
            this.DetalleCuotas = new HashSet<DetalleCuota>();
            this.ReciboCuotas = new HashSet<ReciboCuota>();
        }

        [Key]
        public int Id { get; set; }
        
        [Display(Name = "Número de Cuotas")]
        public int NumCoutas { get; set; }
        
        [Display(Name = "Tipo de Cuotas")]
        public string TipoCuotas { get; set; }

        [Display(Name = "Recibo de Prima")]
        public string ReciboDePrima { get; set; }

        //Hijo
        public virtual ICollection<DetalleCuotasAdenda> DetalleCuotasAdendas { get; set; }
        public virtual ICollection<DetalleCuota> DetalleCuotas { get; set; }
        public virtual ICollection<ReciboCuota> ReciboCuotas { get; set; }
    }
}