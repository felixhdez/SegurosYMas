using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.Entity;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Inspinia_MVC5_SeedProject.Models
{
    [Table("ArchivosPolizas", Schema = "mvc")]
    public class ArchivosPólizas
    {
        [Key]
        public int Id { get; set; }

        public string Foto { get; set; }

        //Llave foránea
        public int PolizaId { get; set; }

        //Padres
        public virtual Poliza Poliza { get; set; }
    }
}