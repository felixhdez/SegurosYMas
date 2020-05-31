using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Inspinia_MVC5_SeedProject.Models
{
    [Table("Roles", Schema = "mvc")]
    public partial class Roles
    {

        public Roles()//Constructor para enlazar hijo
        {
            this.Usuarios = new HashSet<Usuarios>();
        }

        [Key]
        public int Id { get; set; }

        [Display(Name = "Descripción")]
        public string Descripcion { get; set; }


        //Hijo
        public virtual ICollection<Usuarios> Usuarios { get; set; }
    }
}