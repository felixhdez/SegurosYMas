using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.Entity;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Inspinia_MVC5_SeedProject.Models
{
    [Table("Tramites", Schema ="mvc")]
    public partial class Tramite
    {
        public Tramite()
        {
            this.ArchivosTramites = new HashSet<ArchivosTrámites>();
        }

        [Key]
        public int Id { get; set; }
        
        [Display(Name = "Tipo")]
        public string Tipo { get; set; }

        [Required(ErrorMessage = "El campo {0} es obligatorio")]
        [StringLength(60, MinimumLength = 3, ErrorMessage = "La longitud debe estar entre 3 y 60 caracteres")]
        [Display(Name = "Modalidad")]
        public string Modalidad { get; set; }

        [Required(ErrorMessage = "El campo {0} es obligatorio")]
        [StringLength(300, MinimumLength = 3, ErrorMessage = "La longitud debe estar entre 3 y 300 caracteres")]
        [Display(Name = "Descripción")]
        public string Descripcion { get; set; }

        [Required(ErrorMessage = "El campo {0} es obligatorio")]
        [Display(Name = "Fecha de Recepción")]
        [DataType(DataType.Date)]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:yyyy-MM-dd}")]
        public DateTime FechaRecepcion { get; set; }

        [Required(ErrorMessage = "El campo {0} es obligatorio")]
        [StringLength(60, MinimumLength = 3, ErrorMessage = "La longitud debe estar entre 3 y 60 caracteres")]
        [Display(Name = "Nombre del Ejecutivo")]
        public string NombreEjecutivo { get; set; }

        [Required(ErrorMessage = "El campo {0} es obligatorio")]
        [Display(Name = "Fecha de Envío")]
        [DataType(DataType.Date)]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:yyyy-MM-dd}")]
        public DateTime FechaEnvio { get; set; }

        [Required(ErrorMessage = "El campo {0} es obligatorio")]
        [Display(Name = "Fecha de Recibido")]
        [DataType(DataType.Date)]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:yyyy-MM-dd}")]
        public DateTime FechaRecibido { get; set; }

        [Required(ErrorMessage = "El campo {0} es obligatorio")]
        [StringLength(60, MinimumLength = 3, ErrorMessage = "La longitud debe estar entre 3 y 60 caracteres")]
        [Display(Name = "Recibido por")]
        public string RecibidoPor { get; set; }
        
        [Display(Name = "Estado de finalización")]
        public bool Finalizacion { get; set; }

        //Llaves foráneas
        public int PolizaId { get; set; }

        //Padres
        public virtual Poliza Poliza { get; set; }

        //Hijos
        public virtual ICollection<ArchivosTrámites> ArchivosTramites { get; set; }
    }
}