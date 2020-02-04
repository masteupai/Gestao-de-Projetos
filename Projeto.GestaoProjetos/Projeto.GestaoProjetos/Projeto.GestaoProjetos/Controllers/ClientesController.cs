using Biblioteca.Classes;
using Projeto.GestaoProjetos.Db;
using Projeto.GestaoProjetos.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
#region
#endregion
namespace Projeto.GestaoProjetos.Controllers
{
    [Authorize(Roles ="ADMIN")]
    public class ClientesController : Controller
    {
        public ClientesDaoImpl clientesDao { get; set; }
        public ClientesController()
        {
            clientesDao = new ClientesDaoImpl();
        }
        // GET: Clientes
        public ActionResult Index()
        {   var lista = clientesDao.Listar();
            return View(lista);
        }
        [HttpGet]
        public ActionResult IncluirCliente()
        {
            return View();
        }
        [HttpPost]
        public ActionResult IncluirCliente(Cliente cliente)
        {
            try
            {  
                if(cliente.Cnpj == null)
                {
                    return View();
                }      
                cliente.Cnpj = cliente.Cnpj.Trim();
                cliente.Cnpj = cliente.Cnpj.Replace(".", "").Replace("-", "").Replace("/", "");
                if (!cliente.Cnpj.ValidaCnpj())
                {
                    ModelState.AddModelError("Cnpj", "Informe um Cnpj válido!");
                    return View();
                }
                
                if (!ModelState.IsValid)
                {
                    return View();
                }
                clientesDao.ExecutarTarefa(cliente,TipoOperacaoBd.Added);
                TempData["Mensagem"] = "Cliente incluído com sucesso...";

                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                ViewBag.MensagemErro = ex.Message;
                return View("_erro");
            }         
        }      
        [HttpGet]
        public ActionResult ListarCliente()
        {
            var lista = clientesDao.Listar();
            return View(lista);
        }
        private ActionResult VerificarCliente(int id, string view)
        {
            try
            {
                var cliente = clientesDao.Buscar(id);
                if (cliente == null)
                {
                    throw new Exception("Cliente não existe, ou não informado!");
                }

                return View(view, cliente);
            }
            catch (Exception ex)
            {
                ViewBag.MensagemErro = ex.Message;
                return View("_erro");
            }
        }
        [HttpGet]
        public ActionResult AlterarCliente(int id)
        {
            return VerificarCliente(id, "AlterarCliente");
        }
        [HttpPost]
        public ActionResult AlterarCliente(Cliente cliente)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return View();
                }
                clientesDao.ExecutarTarefa(cliente,TipoOperacaoBd.Modified);
                TempData["Mensagem"] = "Cliente alterado!";
                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                ViewBag.MensagemErro = ex.Message;
                return View("_erro");
            }
        }
        [HttpGet]
        public ActionResult ExcluirCliente(int id)
        {
            return VerificarCliente(id, "ExcluirCliente");
        }
        [HttpPost]
        public ActionResult ExcluirCliente(Cliente cliente)
        {
            try
            {
                clientesDao.ExecutarTarefa(cliente,TipoOperacaoBd.Deleted);
                TempData["Mensagem"] = "Cliente Excluído!";
                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                ViewBag.MenssagemErro = ex.Message;
                return View("_erro");
            }



        }
        [HttpGet]
        public ActionResult ListaDeProjetosCliente(int id)
        {
            var lista = ProjetosDao.ListaProjetosCliente(id);
            return View(lista);
        }
    }
}