using System.ComponentModel.DataAnnotations;

namespace Inspinia_MVC5_SeedProject.Models
{
    public class ExternalLoginConfirmationViewModel
    {
        [Required]
        [Display(Name = "User name")]
        public string UserName { get; set; }
    }

    public class ManageUserViewModel
    {
        [Required(ErrorMessage = "El campo {0} es requerido.")]
        [DataType(DataType.Password)]
        [Display(Name = "Clave actual")]
        public string OldPassword { get; set; }

        [Required(ErrorMessage = "El campo {0} es requerido.")]
        [StringLength(100, ErrorMessage = "La longitud del campo {0} debe ser de {2} caracteres.", MinimumLength = 6)]
        [DataType(DataType.Password)]
        [Display(Name = "Nueva Clave")]
        public string NewPassword { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "Confirmar clave nueva")]
        [Compare("NewPassword", ErrorMessage = "La nueva clave y su confirmación no coinciden")]
        public string ConfirmPassword { get; set; }
    }

    public class LoginViewModel
    {
        [Required(ErrorMessage = "El campo {0} es requerido.")]
        [Display(Name = "Usuario")]
        [EmailAddress]
        public string UserName { get; set; }

        [Required(ErrorMessage = "El campo {0} es requerido.")]
        [DataType(DataType.Password)]
        [Display(Name = "Clave")]
        public string Password { get; set; }

        [Display(Name = "Remember me?")]
        public bool RememberMe { get; set; }
    }

    public class RegisterViewModel
    {
        [Required(ErrorMessage ="El campo {0} es requerido.")]
        [Display(Name = "Correo")]
        public string UserName { get; set; }

        [Required(ErrorMessage = "El campo {0} es requerido.")]
        public string Nombres { get; set; }
        [Required(ErrorMessage = "El campo {0} es requerido.")]
        public string Apellidos { get; set;}

        [Required(ErrorMessage = "El campo {0} es requerido.")]
        [Display(Name ="Rol")]
        public string Permiso { get; set; }
        public int? ClienteId { get; set; }

        [Required(ErrorMessage = "El campo {0} es requerido.")]
        [StringLength(100, ErrorMessage = "La longitud del campo {0} debe ser de {2} caracteres.", MinimumLength = 6)]
        [DataType(DataType.Password)]
        [Display(Name = "Clave")]
        public string Password { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "Confirmar Clave")]
        [Compare("Password", ErrorMessage = "La clave y confirmación de clave no coinciden")]
        public string ConfirmPassword { get; set; }
    }

    public class EditViewModel
    {
        [Key]
        public string Id { get; set; }
        [Required(ErrorMessage = "El campo {0} es requerido.")]
        [Display(Name = "Correo")]
        public string UserName { get; set; }

        [Required(ErrorMessage = "El campo {0} es requerido.")]
        public string Nombres { get; set; }
        [Required(ErrorMessage = "El campo {0} es requerido.")]
        public string Apellidos { get; set; }

        [Required(ErrorMessage = "El campo {0} es requerido.")]
        [Display(Name = "Rol")]
        public string Permiso { get; set; }
        public int? ClienteId { get; set; }

        [Required(ErrorMessage = "El campo {0} es requerido.")]
        [StringLength(100, ErrorMessage = "La longitud del campo {0} debe ser de {2} caracteres.", MinimumLength = 6)]
        [DataType(DataType.Password)]
        [Display(Name = "Clave")]
        public string Password { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "Confirmar Clave")]
        [Compare("Password", ErrorMessage = "La clave y confirmación de clave no coinciden")]
        public string ConfirmPassword { get; set; }
    }
}
