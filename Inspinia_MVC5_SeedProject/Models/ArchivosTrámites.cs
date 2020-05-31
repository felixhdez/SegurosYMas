using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.Entity;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Inspinia_MVC5_SeedProject.Models
{
    [Table("ArchivosTramites", Schema ="mvc")]
    public class ArchivosTrámites
    {
        [Key]
        public int Id { get; set; }

        public string Foto { get; set; }

        //Llave foránea
        public int TramiteId { get; set; }

        //Padres
        public virtual Tramite Tramite { get; set; }
    }
}