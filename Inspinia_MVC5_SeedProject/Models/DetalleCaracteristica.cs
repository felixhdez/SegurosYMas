using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.Entity;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Inspinia_MVC5_SeedProject.Models
{
    [Table("DetalleCaracteristicas", Schema = "mvc")]
    public partial class DetalleCaracteristica
    {
        [Key]
        public int Id { get; set; }
        
        [StringLength(300, MinimumLength = 1, ErrorMessage = "La longitud debe estar entre 1 y 300 caracteres")]
        [Display(Name = "Valor")]
        public string Valor { get; set; }

        //Llaves Foraneas
        public int CaracteristicaId { get; set; }
        public int BienAseguradoId { get; set; }

        //Padres
        public virtual Caracteristica Caracteristica { get; set; }
        public virtual BienAsegurado BienAsegurado { get; set; }
    }
}