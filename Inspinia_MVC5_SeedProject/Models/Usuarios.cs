using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Inspinia_MVC5_SeedProject.Models
{
    [Table("Usuarios", Schema = "mvc")]
    public partial class Usuarios
    {

        public Usuarios()//Constructor para enlazar hijo
        {
            this.ReclamosTemps = new HashSet<ReclamosTemp>();
        }

        [Key]
        public int Id { get; set; }

        [Display(Name = "Cliente ID")]
        public int? IdNUser { get; set; }

        [Required(ErrorMessage ="El campo {0} es obligatorio")]
        public string Nombre { get; set; }

        [Required(ErrorMessage = "El campo {0} es obligatorio")]
        public string Apellido { get; set; }

        [Display(Name = "Usuario")]
        public string User { get; set; }

        [Display(Name = "Contraseña")]
        public string Pass { get; set; }

        public string AspUser { get; set; }

        [Display(Name = "Estado")]
        public bool Estado { get; set; }

        //llave foranea
        public int RolId { get; set; }

        //padre
        public virtual Roles Rol { get; set; }


        //Hijo
        public virtual ICollection<ReclamosTemp> ReclamosTemps { get; set; }

    }
}