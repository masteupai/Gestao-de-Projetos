using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Microsoft.Owin.Security;
using Projeto.GestaoProjetos.Models;

namespace Projeto.GestaoProjetos.Controllers
{
   
    public class AutenticacaoController : Controller
    { // GET: Autenticacao
        public ActionResult Index()
        {
            return View();
        }

        [HttpGet]
        [Authorize(Roles = "ADMIN")]
        public ActionResult CadastroUsuario()
        {
            var roleManager = new RoleManager<IdentityRole>(new RoleStore<IdentityRole>());
            var lista = roleManager.Roles.ToList();

            var listaRoles = new List<string>();

            foreach (var item in lista)
            {
                listaRoles.Add(item.Name);
            }

            ViewBag.Roles = new SelectList(listaRoles);
            return View();

        }
        [HttpPost]
        [Authorize(Roles = "ADMIN")]
        public ActionResult CadastroUsuario(UsuarioView usuario)
        {
            if (!ModelState.IsValid) return CadastroUsuario();
            //dados de armazenamento do usuário
            var usuarioStore = new UserStore<IdentityUser>();
            var usuarioManager = new UserManager<IdentityUser>(usuarioStore);

            //criar a identidade do usuário
            var usuarioInfo = new IdentityUser()
            {
                UserName = usuario.Nome

            };

            //com a identidade, criamos o usuario 
            IdentityResult result = usuarioManager.Create(usuarioInfo, usuario.Senha);

            //se o usuario foi criado, o autenticamos
            if (result.Succeeded)
            {
                //define um papel para o usuario (role)
                usuarioManager.AddToRole(usuarioInfo.Id, usuario.Nivel);

                var autentica = System.Web.HttpContext.Current.GetOwinContext().Authentication;

                var identidade = usuarioManager.CreateIdentity(usuarioInfo, DefaultAuthenticationTypes.ApplicationCookie);

                autentica.SignIn(new AuthenticationProperties() { IsPersistent = false }, identidade);  //fechar o browser sai do login por estar no false e nao amarzenar em cookie
                TempData["Mensagem"] = "Usuario cadastrado com  sucesso!";
                return RedirectToAction("Index", "Home");
            }
            else
            {
                ViewBag.MensagemErro = result.Errors.FirstOrDefault();
                return View("_erro");
            }

        }
        [HttpGet]
        public ActionResult Login()
        {
            return View();
        }
        [HttpPost]
        public ActionResult Login(LoginView usuario, string ReturnUrl)
        {
            if (!ModelState.IsValid) return View();

            var usuarioStore = new UserStore<IdentityUser>();
            var usuarioManager = new UserManager<IdentityUser>(usuarioStore);

            var usuarioInfo = usuarioManager.Find(usuario.Nome, usuario.Senha);

            if (usuarioInfo != null)
            {
                var autentica = System.Web.HttpContext.Current.GetOwinContext().Authentication;
                var identidade = usuarioManager.CreateIdentity(usuarioInfo, DefaultAuthenticationTypes.ApplicationCookie);
                autentica.SignIn(new AuthenticationProperties() { IsPersistent = true }, identidade);

                if (ReturnUrl == null) return RedirectToAction("Index", "Home");
                else return Redirect(ReturnUrl);
            }
            else
            {
                ViewBag.MensagemErro = "Usuário ou senha inválidos";
                return View("_erro");
            }
        }

        public ActionResult Logout()
        {
            var autentica = System.Web.HttpContext.Current.GetOwinContext().Authentication;
            autentica.SignOut();
            return RedirectToAction("Index", "Home");
        }

        //roles
        [HttpGet]
        [Authorize(Roles = "ADMIN")]
        public ActionResult IncluirRole()
        {
            return View();
        }
        [HttpPost]
        [Authorize(Roles = "ADMIN")]
        public ActionResult IncluirRole(string role)
        {
            try
            {
                var roleManager = new RoleManager<IdentityRole>(new RoleStore<IdentityRole>());
                var objRole = new IdentityRole();
                objRole.Name = role;
                roleManager.Create(objRole);
                ViewBag.Resposta = "Role " + role + " incluída com sucesso";
                return View();
            }
            catch (Exception ex)
            {
                ViewBag.MensagemErro = ex.Message;
                return View("_erro");
            }
        }
    }
}