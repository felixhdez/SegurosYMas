using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.Entity;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Inspinia_MVC5_SeedProject.Models
{
    [Table("DetalleBienAsegurados", Schema = "mvc")]
    public partial class DetalleBienAsegurado
    {
        public DetalleBienAsegurado()//Constructor para enlazar hijo
        {
            this.Reclamos = new HashSet<Reclamo>();
        }

        [Key]
        public int Id { get; set; }

        //Llave Foraneas
        public int BienAseguradoId { get; set; }
        public int PolizaId { get; set; }

        //Padres
        public virtual BienAsegurado BienAsegurado { get; set; }
        public virtual Poliza Poliza { get; set; }

        //Hijo
        public virtual ICollection<Reclamo> Reclamos { get; set; }

    }
}