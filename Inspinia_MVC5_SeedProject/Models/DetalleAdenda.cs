using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.Entity;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Inspinia_MVC5_SeedProject.Models
{
    [Table("DetalleAdendas", Schema = "mvc")]
    public partial class DetalleAdenda
    {
        [Key]
        public int Id { get; set; }

        //Llaves foraneas
        public int AdendaId { get; set; }
        public int BienAseguradoId { get; set; }

        //Padre
        public virtual Adenda Adenda { get; set; }
        public virtual BienAsegurado BienAsegurado { get; set; }
    }
}