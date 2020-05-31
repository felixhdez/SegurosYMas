using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Inspinia_MVC5_SeedProject.Models
{
    [Table("ReclamosTemporales", Schema = "mvc")]
    public partial class ReclamosTemp
    {

        [Key]
        public int Id { get; set; }

        [Display(Name = "Fecha")]
        public string Fecha { get; set; }

        [Display(Name = "Lugar")]
        public string Lugar { get; set; }

        [StringLength(1000, ErrorMessage = "La longitud debe ser menor a 1000 caracteres")]
        [Display(Name = "Comentario")]
        public string Comentarios { get; set; }

        public string Tipo { get; set; }

        public bool Visto { get; set; }

        //llave foranea
        public int UsuarioId { get; set; }


        //padre
        public virtual Usuarios Usuario { get; set; }

    }
}