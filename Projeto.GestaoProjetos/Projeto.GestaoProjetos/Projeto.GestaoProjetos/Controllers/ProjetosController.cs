using Projeto.GestaoProjetos.Db;
using Projeto.GestaoProjetos.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Data.Entity;
using System.Web;
using System.Web.Mvc;
using Projeto.GestaoProjetos.ViewModel;

namespace Projeto.GestaoProjetos.Controllers
{
    [Authorize(Roles = "ADMIN")]
    public class ProjetosController : Controller
    {
        public ClientesDaoImpl clientesDao { get; set; }
        public ProjetosController()
        {
            clientesDao = new ClientesDaoImpl();
        }
        // GET: Projetos
        public ActionResult Index()
        {
            ViewBag.Projetos = ProjetosDao.ListarProjetos();
            return View();
        }
        [HttpGet]
        public ActionResult IncluirProjeto()
        {
            List<TipoStatusProjeto> tiposstatus = new List<TipoStatusProjeto>()
                 {
                new TipoStatusProjeto{Codigo=1,Descricao="EM ANDAMENTO"},
                new TipoStatusProjeto{Codigo=2,Descricao="PAUSADO"},
                new TipoStatusProjeto{Codigo=2,Descricao="FINALIZADO"}
                 };
            List<TipoStatusProjeto> tiposescopo = new List<TipoStatusProjeto>()
                 {
                new TipoStatusProjeto{Codigo=1,Descricao="ABERTO"},
                new TipoStatusProjeto{Codigo=2,Descricao="FECHADO"}
                 };


            ViewBag.TiposEscopo = new SelectList(tiposescopo, "Codigo", "Descricao");
            ViewBag.TiposStatus = new SelectList(tiposstatus, "Codigo", "Descricao");
            ViewBag.Clientes = new SelectList(clientesDao.Listar().ToList(), "IDCLIENTE", "RazaoSocial");
            return View();
        }
        [HttpPost]
        public ActionResult IncluirProjeto(CadProjeto projeto)
        {
            try
            {
                if (!ModelState.IsValid) return IncluirProjeto();
                ProjetosDao.IncluirProjeto(projeto);
                TempData["Mensagem"] = "Projeto cadastrado com sucesso...";
                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                ViewBag.MensagemErro = ex.Message;
                return View("_erro");
            }
        }
        [HttpGet]
        public ActionResult AlterarProjeto(int id)
        {
            List<TipoStatusProjeto> tiposstatus = new List<TipoStatusProjeto>()
                 {
                new TipoStatusProjeto{Codigo=1,Descricao="EM ANDAMENTO"},
                new TipoStatusProjeto{Codigo=2,Descricao="PAUSADO"},
                new TipoStatusProjeto{Codigo=3,Descricao="FINALIZADO"}
                 };
            List<TipoStatusProjeto> tiposescopo = new List<TipoStatusProjeto>()
                 {
                new TipoStatusProjeto{Codigo=1,Descricao="ABERTO"},
                new TipoStatusProjeto{Codigo=2,Descricao="FECHADO"}
                 };

            ViewBag.TiposEscopo = new SelectList(tiposescopo, "Codigo", "Descricao");
            ViewBag.TiposStatus = new SelectList(tiposstatus, "Codigo", "Descricao");
            ViewBag.Clientes = new SelectList(clientesDao.Listar().ToList(), "IDCLIENTE", "RazaoSocial");
            var projeto = ProjetosDao.BuscarProjeto(id);
            return View(projeto);
        }
        [HttpPost]
        public ActionResult AlterarProjeto(CadProjeto projeto)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    throw new Exception("ModelState is invalid!");
                }
                ProjetosDao.AlterarProjeto(projeto);
                TempData["Mensagem"] = "Projeto alterado com sucesso...";
                return RedirectToAction("Index");

            }
            catch (Exception ex)
            {
                ViewBag.MensagemErro = ex.Message;
                return View("_erro");
            }
        }





        public ActionResult ListarSkillAjax(int id)
        {
            if (Request.IsAjaxRequest())
            {
                var colaborador = ColaboradoresDao.BuscarColaborador(id);
                ViewBag.Skills = new SelectList(ColaboradoresDao.ListarSkillsInfo(), "IdSkill", "Descricao");
                return PartialView("_SkillProjeto", colaborador);
            }
            else
            {
                return View();
            }
        }
        [HttpGet]
        public ActionResult IncluirColabProjeto()
        {
            ViewBag.Skills = new SelectList(ColaboradoresDao.ListarSkillsInfo(), "IdSkill", "Descricao");
            ViewBag.Colaboradores = new SelectList(ColaboradoresDao.ListarColaboradores(), "IDCOLABORADOR", "Nome");
            ViewBag.Projetos = new SelectList(ProjetosDao.ListarProjetos(), "IDPROJETO", "Descricao");
            return View();
        }
        [HttpPost]
        public ActionResult IncluirColabProjeto(ProjetosColaborador projetoColaborador, double ValorHora)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    throw new Exception("ModelState is invalid!");
                }
                //var colaborador = ColaboradoresDao.BuscarColaborador(projetoColaborador.IDCOLABORADOR);
                projetoColaborador.ValorColaborador = ColaboradoresDao.BuscarColaborador(projetoColaborador.IDCOLABORADOR).ValorHora;
                projetoColaborador.ValorHoraProjColab = ValorHora;
                ProjColaboradorDao.IncluirColabProjeto(projetoColaborador);
                TempData["Mensagem"] = "Colaborador alocado com sucesso...";
                return RedirectToAction("Index");

            }
            catch (Exception ex)
            {
                ViewBag.MensagemErro = ex.Message;
                return View("_erro");
            }
        }

        public ActionResult MostrarDetalhesProjeto(int id)
        {
            var projeto = ProjetosDao.BuscarProjeto(id);
            var horasTrabalhadas = AtividadesDao.TotalHorasProjetos(id);
            var colaboradores = ProjColaboradorDao.ListarColaboradoresProjeto(id);
            double valorPagoColaboradores = 0;
            double valorPagoColaboradoresProjeto = 0;
            int horas;
            foreach (var item in colaboradores)
            {
                horas = AtividadesDao.TotalHorasColaborador(item.IDPROJ_COLAB);
                valorPagoColaboradores += (item.ValorColaborador * horas);
                valorPagoColaboradoresProjeto += (item.ValorHoraProjColab * horas);
            }
            double valorTotal = 0;
            if (projeto.Escopo == 1)
            {
                if (projeto.ValorDespesas != 0 || projeto.ValorDespesas != null)
                {
                    valorTotal += ((projeto.ValorProjeto * horasTrabalhadas) + valorPagoColaboradoresProjeto) - (double)projeto.ValorDespesas - valorPagoColaboradores;
                }
                else
                {
                    valorTotal += ((projeto.ValorProjeto * horasTrabalhadas) + valorPagoColaboradoresProjeto) - valorPagoColaboradores;
                }
            }
            else
            {
                if (projeto.ValorDespesas != 0 || projeto.ValorDespesas != null) valorTotal = projeto.ValorProjeto - valorPagoColaboradores - (double)projeto.ValorDespesas;
                else valorTotal = projeto.ValorProjeto - valorPagoColaboradores;
            }
            var valores = new ProjetoValoresViewModel()
            {
                NumHorasProj = (int)projeto.NumHoras,
                HorasRestantes = AtividadesDao.TotalHorasRestante(id),
                HorasTrabalhadas = horasTrabalhadas,
                ValorTotal = valorTotal,
                ValorPagoColaboradores = valorPagoColaboradores,
                ValorPedidoColaboradores = valorPagoColaboradoresProjeto,
                ValorDespesas = (double)projeto.ValorDespesas,
                ValorOrca = projeto.ValorProjeto
            };
            ViewBag.Valores = valores;
            ViewBag.Colaboradores = colaboradores;
            return View(projeto);
        }
        public ActionResult ListarColaboradoresProjetoAjax(int id)
        {
            if (Request.IsAjaxRequest())
            {
                var colaboradores = ProjColaboradorDao.ListarColaboradoresProjeto();
                return PartialView("_ColaboradoresProjeto", colaboradores);
            }
            else
            {
                return View();
            }
        }

        private ActionResult VerificarProjColab(int id, string view)
        {
            try
            {
                var projcolab = ProjColaboradorDao.BuscarProjetoColaborador(id);
                if (projcolab == null)
                {
                    throw new Exception("Projeto/Colab não vinculados, ou não informados!");
                }

                return View(view, projcolab);
            }
            catch (Exception ex)
            {
                ViewBag.MensagemErro = ex.Message;
                return View("_erro");
            }
        }
        [HttpGet]
        public ActionResult RemoverColaboradorProjeto(int id)
        {
            return VerificarProjColab(id, "RemoverColaboradorProjeto");
        }
        [HttpPost]
        public ActionResult RemoverColaboradorProjeto(ProjetosColaborador projeto)
        {
            try
            {
                var projcolab = ProjColaboradorDao.BuscarProjetoColaborador(projeto.IDPROJ_COLAB);
                var atividades = AtividadesDao.ListarPontosPorcolaborador(projcolab.IDPROJ_COLAB);
                if (!ModelState.IsValid)
                {
                    return View();
                }
                if (atividades.Count() > 0) throw new Exception("Colaborador possui atividades registradas, não é possivel remover ele do projeto!");
                else
                {
                    ProjColaboradorDao.DeleteProjColab(projcolab);
                    TempData["Mensagem"] = "Desvinculado com sucesso...";
                    return RedirectToAction("MostrarDetalhesProjeto", new { id = projcolab.IDPROJETO });
                }
            }
            catch (Exception ex)
            {
                ViewBag.MensagemErro = ex.Message;
                return View("_erro");
            }
        }



    }
}