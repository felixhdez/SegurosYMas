using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.Entity;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Inspinia_MVC5_SeedProject.Models
{
    [Table("ArchivosReclamos", Schema = "mvc")]
    public class ArchivosReclamos
    {
        [Key]
        public int Id { get; set; }

        public string Foto { get; set; }

        //Llave foránea
        public int ReclamoId { get; set; }

        //Padres
        public virtual Reclamo Reclamo { get; set; }
    }
}