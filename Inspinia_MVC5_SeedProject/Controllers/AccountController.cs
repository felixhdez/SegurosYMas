using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.Owin.Security;
using Inspinia_MVC5_SeedProject.Models;

namespace Inspinia_MVC5_SeedProject.Controllers
{
    [Authorize]
    public class AccountController : Controller
    {
        private SeguroBD db = new SeguroBD();
        private SeguridadUserBD dbs = new SeguridadUserBD();
        private ApplicationDbContext context = new ApplicationDbContext();
        public AccountController()
            : this(new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(new ApplicationDbContext())))
        {
        }

        public string Encriptar(string _cadenaAencriptar)
        {
            string result = string.Empty;
            byte[] encryted = System.Text.Encoding.Unicode.GetBytes(_cadenaAencriptar);
            result = Convert.ToBase64String(encryted);
            return result;
        }

        public AccountController(UserManager<ApplicationUser> userManager)
        {
            UserManager = userManager;
        }

        public UserManager<ApplicationUser> UserManager { get; private set; }

        //
        // GET: /Account/Login
        [AllowAnonymous]
        public ActionResult Login(string returnUrl)
        {
            var actual = User.Identity.GetUserId();
            if (actual != null)
            {
                ViewBag.Login = 1;
            }
            else
            {
                ViewBag.Login = 0;
            }
            ViewBag.ReturnUrl = returnUrl;
            return View();
        }

        //
        // POST: /Account/Login
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Login(LoginViewModel model, string returnUrl)
        {
            if (ModelState.IsValid)
            {
                var user = await UserManager.FindAsync(model.UserName, model.Password);
                if (user != null && !user.LockoutEnabled)
                {
                    var busq = dbs.Usuarios.Where(x => x.AspUser == user.Id).FirstOrDefault();
                    if (busq == null || busq.Rol.Descripcion != "Cliente")
                    {
                        await SignInAsync(user, model.RememberMe);
                        return RedirectToLocal(returnUrl);
                    }
                }
                else
                {
                    ModelState.AddModelError("", "Usuario o contraseña inválido.");
                }
            }
            ViewBag.Login = 0;
            // If we got this far, something failed, redisplay form
            return View(model);
        }

        //
        // GET: /Account/Register
        [Authorize(Roles = "Administración,Gerencia")]
        public ActionResult Register()
        {
            ViewBag.ListaPermisos = new SelectList(context.Roles.ToList(), "Name", "Name");
            ViewBag.ListaClientes = new SelectList(db.Clientes.ToList(), "Id", "MostrarIdent");
            return View();
        }

        //
        // POST: /Account/Register
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Register(RegisterViewModel model)
        {
            if (ModelState.IsValid)
            {
                if (model.Permiso == "Cliente" && model.ClienteId == null)
                {
                    AddErrors(new IdentityResult("Debe seleccionar un cliente."));
                }
                else
                {
                    var user = new ApplicationUser() { UserName = model.UserName, Email = model.UserName };
                    var result = await UserManager.CreateAsync(user, model.Password);
                    if (result.Succeeded)
                    {
                        try
                        {
                            var r = dbs.Roles.Where(x => x.Descripcion == model.Permiso).Select(y => y.Id).FirstOrDefault();
                            Usuarios u = new Usuarios();
                            u.Nombre = model.Nombres;
                            u.Apellido = model.Apellidos;
                            u.Estado = true;
                            u.IdNUser = model.ClienteId;
                            u.User = model.UserName;
                            u.Pass = Encriptar(model.Password);
                            u.RolId = r;
                            u.AspUser = user.Id;
                            dbs.Usuarios.Add(u);
                            dbs.SaveChanges();
                        }
                        catch (Exception)
                        {

                        }

                        //await SignInAsync(user, isPersistent: false); //esta linea hace que el usuario registrado se mande a logear
                        await this.UserManager.AddToRoleAsync(user.Id, model.Permiso);
                        return RedirectToAction("Index", "Usuarios");

                    }
                    else
                    {
                        AddErrors(result);
                    }
                }
            }

            ViewBag.ListaPermisos = new SelectList(context.Roles.ToList(), "Name", "Name");
            ViewBag.ListaClientes = new SelectList(db.Clientes.ToList(), "Id", "NombreCompleto");
            // If we got this far, something failed, redisplay form
            return View(model);
        }

        [Authorize(Roles = "Administración,Gerencia")]
        public async Task<ActionResult> Edit(string id)
        {
            var usuario = await UserManager.FindByIdAsync(id);
            
            if (usuario != null)
            {
                var busq = dbs.Usuarios.Where(x => x.AspUser == usuario.Id).FirstOrDefault();
                EditViewModel e = new EditViewModel();
                e.Id = usuario.Id;
                e.Apellidos = busq.Apellido;
                e.Nombres = busq.Nombre;
                e.UserName = usuario.UserName;
                ViewBag.ListaPermisos = new SelectList(context.Roles.ToList(), "Name", "Name", busq.Rol.Descripcion);
                ViewBag.ListaClientes = new SelectList(db.Clientes.ToList(), "Id", "MostrarIdent", busq.IdNUser);
                return View(e);
            }
            else
            {
                return RedirectToAction("Index", "Usuarios", null);
            }
        }

        [HttpPost]
        public async Task<ActionResult> Edit(EditViewModel model)
        {
            var usuario = await UserManager.FindByIdAsync(model.Id);
            if (ModelState.IsValid)
            {
                if (usuario != null)
                {
                    usuario.Email = model.UserName;
                    usuario.UserName = model.UserName;

                    if (!string.IsNullOrEmpty(model.Password))
                    {
                        IdentityResult validacionClave = await UserManager.PasswordValidator.ValidateAsync(model.Password);

                        if (validacionClave.Succeeded)
                        {
                            Usuarios u = dbs.Usuarios.Where(x => x.AspUser == usuario.Id).FirstOrDefault();
                            usuario.PasswordHash = UserManager.PasswordHasher.HashPassword(model.Password);
                            var r = this.UserManager.RemoveFromRole(usuario.Id, u.Rol.Descripcion);
                            var r2 = this.UserManager.AddToRole(usuario.Id, model.Permiso);
                            IdentityResult resultado = await UserManager.UpdateAsync(usuario);
                            if (resultado.Succeeded && r.Succeeded && r2.Succeeded)
                            {
                                try
                                {

                                    var t = dbs.Roles.Where(x => x.Descripcion == model.Permiso).Select(y => y.Id).FirstOrDefault();
                                    u.Nombre = model.Nombres;
                                    u.Apellido = model.Apellidos;
                                    u.IdNUser = model.ClienteId;
                                    u.Pass = Encriptar(model.Password);
                                    u.User = model.UserName;
                                    u.RolId = t;
                                    dbs.Entry(u).State = System.Data.Entity.EntityState.Modified;
                                    dbs.SaveChanges();
                                    return RedirectToAction("Index", "Usuarios", null);
                                }
                                catch (Exception)
                                {

                                }
                            }
                            else
                            {
                                AddErrors(resultado);
                            }
                        }
                        //else
                        //{
                        //    AddErrors(validacionClave);
                        //}
                    }
                    else
                    {
                        ModelState.AddModelError("", "La clave no puede estar vacia.");
                    }
                }
                else
                {
                    ModelState.AddModelError("", "Usuario no encontrado");
                }
            }
            var busq = dbs.Usuarios.Where(x => x.AspUser == usuario.Id).FirstOrDefault();
            EditViewModel e = new EditViewModel();
            e.Id = usuario.Id;
            e.Apellidos = busq.Apellido;
            e.Nombres = busq.Nombre;
            e.UserName = usuario.UserName;
            ViewBag.ListaPermisos = new SelectList(context.Roles.ToList(), "Name", "Name");
            ViewBag.ListaClientes = new SelectList(db.Clientes.ToList(), "Id", "NombreCompleto");
            return View(e);
        }

        [Authorize(Roles = "Administración,Gerencia")]
        public async Task<ActionResult> Deshabilitar(string id)
        {
            var usuario = await UserManager.FindByIdAsync(id);
            usuario.LockoutEnabled = true;
            IdentityResult resultado = await UserManager.UpdateAsync(usuario);
            if (resultado.Succeeded)
            {
                try
                {
                    Usuarios u = dbs.Usuarios.Where(x => x.AspUser == usuario.Id).FirstOrDefault();
                    u.Estado = false;
                    dbs.Entry(u).State = System.Data.Entity.EntityState.Modified;
                    dbs.SaveChanges();
                }
                catch (Exception)
                {
                    
                }
            }
            return RedirectToAction("Index", "Usuarios", null);
        }

        //
        // POST: /Account/Disassociate
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Disassociate(string loginProvider, string providerKey)
        {
            ManageMessageId? message = null;
            IdentityResult result = await UserManager.RemoveLoginAsync(User.Identity.GetUserId(), new UserLoginInfo(loginProvider, providerKey));
            if (result.Succeeded)
            {
                message = ManageMessageId.RemoveLoginSuccess;
            }
            else
            {
                message = ManageMessageId.Error;
            }
            return RedirectToAction("Manage", new { Message = message });
        }

        //
        // GET: /Account/Manage
        public ActionResult Manage(ManageMessageId? message)
        {
            ViewBag.StatusMessage =
                message == ManageMessageId.ChangePasswordSuccess ? "Su contraseña ha sido cambiada."
                : message == ManageMessageId.SetPasswordSuccess ? "Su contraseña ha sido establecida."
                : message == ManageMessageId.RemoveLoginSuccess ? "Se eliminó el inicio de sesión externo."
                : message == ManageMessageId.Error ? "Ha ocurrido un error."
                : "";
            ViewBag.HasLocalPassword = HasPassword();
            ViewBag.ReturnUrl = Url.Action("Manage");
            return View();
        }

        //
        // POST: /Account/Manage
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Manage(ManageUserViewModel model)
        {
            bool hasPassword = HasPassword();
            ViewBag.HasLocalPassword = hasPassword;
            ViewBag.ReturnUrl = Url.Action("Manage");
            if (hasPassword)
            {
                if (ModelState.IsValid)
                {
                    IdentityResult result = await UserManager.ChangePasswordAsync(User.Identity.GetUserId(), model.OldPassword, model.NewPassword);
                    if (result.Succeeded)
                    {
                        return RedirectToAction("Manage", new { Message = ManageMessageId.ChangePasswordSuccess });
                    }
                    else
                    {
                        AddErrors(result);
                    }
                }
            }
            else
            {
                // User does not have a password so remove any validation errors caused by a missing OldPassword field
                ModelState state = ModelState["OldPassword"];
                if (state != null)
                {
                    state.Errors.Clear();
                }

                if (ModelState.IsValid)
                {
                    IdentityResult result = await UserManager.AddPasswordAsync(User.Identity.GetUserId(), model.NewPassword);
                    if (result.Succeeded)
                    {
                        return RedirectToAction("Manage", new { Message = ManageMessageId.SetPasswordSuccess });
                    }
                    else
                    {
                        AddErrors(result);
                    }
                }
            }

            // If we got this far, something failed, redisplay form
            return View(model);
        }

        //
        // POST: /Account/ExternalLogin
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public ActionResult ExternalLogin(string provider, string returnUrl)
        {
            // Request a redirect to the external login provider
            return new ChallengeResult(provider, Url.Action("ExternalLoginCallback", "Account", new { ReturnUrl = returnUrl }));
        }

        //
        // GET: /Account/ExternalLoginCallback
        [AllowAnonymous]
        public async Task<ActionResult> ExternalLoginCallback(string returnUrl)
        {
            var loginInfo = await AuthenticationManager.GetExternalLoginInfoAsync();
            if (loginInfo == null)
            {
                return RedirectToAction("Login");
            }

            // Sign in the user with this external login provider if the user already has a login
            var user = await UserManager.FindAsync(loginInfo.Login);
            if (user != null)
            {
                await SignInAsync(user, isPersistent: false);
                return RedirectToLocal(returnUrl);
            }
            else
            {
                // If the user does not have an account, then prompt the user to create an account
                ViewBag.ReturnUrl = returnUrl;
                ViewBag.LoginProvider = loginInfo.Login.LoginProvider;
                return View("ExternalLoginConfirmation", new ExternalLoginConfirmationViewModel { UserName = loginInfo.DefaultUserName });
            }
        }

        //
        // POST: /Account/LinkLogin
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult LinkLogin(string provider)
        {
            // Request a redirect to the external login provider to link a login for the current user
            return new ChallengeResult(provider, Url.Action("LinkLoginCallback", "Account"), User.Identity.GetUserId());
        }

        //
        // GET: /Account/LinkLoginCallback
        public async Task<ActionResult> LinkLoginCallback()
        {
            var loginInfo = await AuthenticationManager.GetExternalLoginInfoAsync(XsrfKey, User.Identity.GetUserId());
            if (loginInfo == null)
            {
                return RedirectToAction("Manage", new { Message = ManageMessageId.Error });
            }
            var result = await UserManager.AddLoginAsync(User.Identity.GetUserId(), loginInfo.Login);
            if (result.Succeeded)
            {
                return RedirectToAction("Manage");
            }
            return RedirectToAction("Manage", new { Message = ManageMessageId.Error });
        }

        //
        // POST: /Account/ExternalLoginConfirmation
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> ExternalLoginConfirmation(ExternalLoginConfirmationViewModel model, string returnUrl)
        {
            if (User.Identity.IsAuthenticated)
            {
                return RedirectToAction("Manage");
            }

            if (ModelState.IsValid)
            {
                // Get the information about the user from the external login provider
                var info = await AuthenticationManager.GetExternalLoginInfoAsync();
                if (info == null)
                {
                    return View("ExternalLoginFailure");
                }
                var user = new ApplicationUser() { UserName = model.UserName };
                var result = await UserManager.CreateAsync(user);
                if (result.Succeeded)
                {
                    result = await UserManager.AddLoginAsync(user.Id, info.Login);
                    if (result.Succeeded)
                    {
                        await SignInAsync(user, isPersistent: false);
                        return RedirectToLocal(returnUrl);
                    }
                }
                AddErrors(result);
            }

            ViewBag.ReturnUrl = returnUrl;
            return View(model);
        }

        //
        // POST: /Account/LogOff
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult LogOff()
        {             
            AuthenticationManager.SignOut();
            return RedirectToAction("Index", "Home");
        }

        //
        // GET: /Account/ExternalLoginFailure
        [AllowAnonymous]
        public ActionResult ExternalLoginFailure()
        {
            return View();
        }

        [ChildActionOnly]
        public ActionResult RemoveAccountList()
        {
            var linkedAccounts = UserManager.GetLogins(User.Identity.GetUserId());
            ViewBag.ShowRemoveButton = HasPassword() || linkedAccounts.Count > 1;
            return (ActionResult)PartialView("_RemoveAccountPartial", linkedAccounts);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing && UserManager != null)
            {
                UserManager.Dispose();
                UserManager = null;
            }
            base.Dispose(disposing);
        }

        #region Helpers
        // Used for XSRF protection when adding external logins
        private const string XsrfKey = "XsrfId";

        private IAuthenticationManager AuthenticationManager
        {
            get
            {
                return HttpContext.GetOwinContext().Authentication;
            }
        }

        private async Task SignInAsync(ApplicationUser user, bool isPersistent)
        {
            AuthenticationManager.SignOut(DefaultAuthenticationTypes.ExternalCookie);
            var identity = await UserManager.CreateIdentityAsync(user, DefaultAuthenticationTypes.ApplicationCookie);
            AuthenticationManager.SignIn(new AuthenticationProperties() { IsPersistent = isPersistent }, identity);
        }

        private void AddErrors(IdentityResult result)
        {
            foreach (var error in result.Errors)
            {
                ModelState.AddModelError("", error);
            }
        }

        private bool HasPassword()
        {
            var user = UserManager.FindById(User.Identity.GetUserId());
            if (user != null)
            {
                return user.PasswordHash != null;
            }
            return false;
        }

        public enum ManageMessageId
        {
            ChangePasswordSuccess,
            SetPasswordSuccess,
            RemoveLoginSuccess,
            Error
        }

        private ActionResult RedirectToLocal(string returnUrl)
        {
            if (Url.IsLocalUrl(returnUrl))
            {
                return Redirect(returnUrl);
            }
            else
            {
                return RedirectToAction("Index", "Home");
            }
        }

        private class ChallengeResult : HttpUnauthorizedResult
        {
            public ChallengeResult(string provider, string redirectUri) : this(provider, redirectUri, null)
            {
            }

            public ChallengeResult(string provider, string redirectUri, string userId)
            {
                LoginProvider = provider;
                RedirectUri = redirectUri;
                UserId = userId;
            }

            public string LoginProvider { get; set; }
            public string RedirectUri { get; set; }
            public string UserId { get; set; }

            public override void ExecuteResult(ControllerContext context)
            {
                var properties = new AuthenticationProperties() { RedirectUri = RedirectUri };
                if (UserId != null)
                {
                    properties.Dictionary[XsrfKey] = UserId;
                }
                context.HttpContext.GetOwinContext().Authentication.Challenge(properties, LoginProvider);
            }
        }
        #endregion
    }
}