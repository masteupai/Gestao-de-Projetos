using Biblioteca.Classes;
using Projeto.GestaoProjetos.Db;
using Projeto.GestaoProjetos.Models;
using System;
using System.Data.Entity;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Projeto.GestaoProjetos.ViewModel;

namespace Projeto.GestaoProjetos.Controllers
{
    [Authorize(Roles = "ADMIN")]
    public class ColaboradoresController : Controller
    {
        public ColaboradoresDaoImpl colaboradoresDao { get; set; }
        public SkillsDaoImpl skillsDao { get; set; }
        public ColaboradoresController()
        {
            colaboradoresDao = new ColaboradoresDaoImpl();
            skillsDao = new SkillsDaoImpl();
        }
        // GET: Colaboradores
        public ActionResult Index()
        {


            ViewBag.Colaboradores = colaboradoresDao.Listar();
            ViewBag.Skills = new SelectList(skillsDao.Listar().ToList(), "IdSkill", "Descricao");
            ViewBag.ListaSkills = ColaboradoresDao.ListarSkillsInfo().ToList();
            return View();
        }
        public ActionResult IncluirSkills()
        {
            return View();
        }
        [HttpPost]
        public ActionResult IncluirSkills(Skill skill)
        {
            try
            {

                if (skill == null)
                {
                    return View();
                }
                if (!ModelState.IsValid)
                {
                    return View();
                }
                skillsDao.ExecutarTarefa(skill, TipoOperacaoBd.Added);
                TempData["Mensagem"] = "Skill cadastrada com sucesso...";
                return RedirectToAction("ListarSkills");
            }
            catch (Exception ex)
            {
                ViewBag.MensagemErro = ex.Message;
                return View("_erro");
            }
        }
        [HttpGet]
        public ActionResult ListarSkills()
        {
            var lista = skillsDao.Listar();
            return View(lista);
        }
        private ActionResult VerificarSkill(int id, string view)
        {
            try
            {
                var skill = skillsDao.Buscar(id);
                if (skill == null)
                {
                    throw new Exception("Skill não existe, ou não informada!");
                }

                return View(view, skill);
            }
            catch (Exception ex)
            {
                ViewBag.MensagemErro = ex.Message;
                return View("_erro");
            }
        }
        [HttpGet]
        public ActionResult AlterarSkill(int id)
        {
            return VerificarSkill(id, "AlterarSkill");
        }
        [HttpPost]
        public ActionResult AlterarSkill(Skill skill)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return View();
                }
                skillsDao.ExecutarTarefa(skill, TipoOperacaoBd.Modified);
                TempData["Mensagem"] = "Skill alterada com sucesso...";
                return View("Index");
            }
            catch (Exception ex)
            {
                ViewBag.MensagemErro = ex.Message;
                return View("_erro");
            }
        }
        [HttpGet]
        public ActionResult ExcluirSkill(int id)
        {
            return VerificarSkill(id, "ExcluirSkill");
        }
        [HttpPost]
        public ActionResult ExcluirSkill(Skill skill)
        {
            try
            {
                skillsDao.ExecutarTarefa(skill, TipoOperacaoBd.Deleted);
                TempData["Mensagem"] = "Skill Excluída com sucesso...";
                return RedirectToAction("ListarSkills");
            }
            catch (Exception ex)
            {

                ViewBag.MenssagemErro = ex.Message;
                return View("_erro");
            }



        }
        [HttpGet]
        public ActionResult IncluirColaborador()
        {
            List<TipoPessoa> tipos = new List<TipoPessoa>()
                 {
                new TipoPessoa{Codigo=1,Descricao="CPF"},
                new TipoPessoa{Codigo=2,Descricao="CNPJ"}
                 };
            List<TipoStatus> tiposstatus = new List<TipoStatus>()
                 {
                new TipoStatus{Codigo=1,Descricao="ATIVO"},
                new TipoStatus{Codigo=2,Descricao="INATIVO"}
                 };
            ViewBag.TiposStatus = new SelectList(tiposstatus, "Codigo", "Descricao");
            ViewBag.Colaboradores = colaboradoresDao.Listar();
            ViewBag.TiposPessoa = new SelectList(tipos, "Codigo", "Descricao");
            ViewBag.Skills = new SelectList(ColaboradoresDao.ListarSkillsInfo().ToList(), "IdSkill", "Descricao");
            ViewBag.ListaSkills = ColaboradoresDao.ListarSkillsInfo().ToList();
            return View();
        }
        [HttpPost]
        public ActionResult IncluirColaborador(Colaborador colaborador)
        {
            try
            {
                colaborador.Telefone = colaborador.Telefone.Trim().Replace("-", "").Replace("(", "").Replace(")", "").Replace(" ", "").Replace(".", "");

                if (colaborador == null)
                {
                    return IncluirColaborador();
                }
                if (!ModelState.IsValid)
                {
                    return IncluirColaborador();
                }
                colaboradoresDao.ExecutarTarefa(colaborador, TipoOperacaoBd.Added);
                TempData["Mensagem"] = "Colaborador incluído com sucesso...";
                return RedirectToAction("IncluirColaborador");
            }
            catch (Exception ex)
            {
                ViewBag.MensagemErro = ex.Message;
                return View("_erro");
            }
        }
        private ActionResult VerificarColaborador(int id, string view)
        {
            try
            {
                var colaborador = colaboradoresDao.Buscar(id);
                if (colaborador == null)
                {
                    throw new Exception("Colaborador não existe, ou não informado!");
                }

                return View(view, colaborador);
            }
            catch (Exception ex)
            {
                ViewBag.MensagemErro = ex.Message;
                return View("_erro");
            }
        }
        public ActionResult DetalhesColaborador(int id)
        {
            try
            {
                if (Request.IsAjaxRequest())
                {
                    var projetoColab = ProjColaboradorDao.BuscarProjetoColaborador(id);
                    var projeto = ProjetosDao.BuscarProjeto(projetoColab.IDPROJETO);
                    var horaTrab = AtividadesDao.TotalHorasColaborador(projetoColab.IDPROJ_COLAB);
                    if (projeto.NumHoras == null) projeto.NumHoras = 0;
                    var valores = new ProjetoValoresViewModel()
                    {
                        ValorColaborador = projetoColab.ValorColaborador,
                        HorasTrabalhadas = horaTrab,
                        ValorPagoColaboradores = projetoColab.ValorColaborador * horaTrab,
                        ValorPedidoColaboradores = projetoColab.ValorHoraProjColab * horaTrab,
                        HorasRestantes = (int)projeto.NumHoras - horaTrab,
                        NumHorasProj = (int)projeto.NumHoras
                    };
                    ViewBag.Projetos = ProjColaboradorDao.ListarProjetosColaborador(id);
                    ViewBag.Skill = ColaboradoresDao.BuscarSkillInfo(ColaboradoresDao.BuscarColaborador(projetoColab.IDCOLABORADOR).IDSKILL).Descricao;
                    return PartialView("_ValoresColaborador", valores);
                }
                else
                {
                    ViewBag.Projetos = ProjColaboradorDao.ListarProjetosColaborador(id);
                    ViewBag.Skill = ColaboradoresDao.BuscarSkillInfo(ColaboradoresDao.BuscarColaborador(id).IDSKILL).Descricao;
                    return VerificarColaborador(id, "DetalhesColaborador");
                }
            }
            catch (Exception ex)
            {
                ViewBag.MensagemErro = ex.Message;
                return View("_erro");
            }
        }
        [HttpGet]
        public ActionResult AlterarColaborador(int id)
        {
            List<TipoPessoa> tipos = new List<TipoPessoa>()
                 {
                new TipoPessoa{Codigo=1,Descricao="CPF"},
                new TipoPessoa{Codigo=2,Descricao="CNPJ"}
                 };
            List<TipoStatus> tiposstatus = new List<TipoStatus>()
                 {
                new TipoStatus{Codigo=1,Descricao="ATIVO"},
                new TipoStatus{Codigo=2,Descricao="INATIVO"}
                 };
            ViewBag.TiposStatus = new SelectList(tiposstatus, "Codigo", "Descricao");
            ViewBag.TiposPessoa = new SelectList(tipos, "Codigo", "Descricao");
            ViewBag.Skills = new SelectList(ColaboradoresDao.ListarSkillsInfo().ToList(), "IdSkill", "Descricao");
            ViewBag.ListaSkills = ColaboradoresDao.ListarSkillsInfo().ToList();
            return VerificarColaborador(id, "AlterarColaborador");
        }
        [HttpPost]
        public ActionResult AlterarColaborador(Colaborador colaborador)
        {
            try
            {
                if (!ModelState.IsValid) return View();

                colaboradoresDao.ExecutarTarefa(colaborador, TipoOperacaoBd.Modified);
                TempData["Mensagem"] = "Colaborador alterado com sucesso...";
                return RedirectToAction("IncluirColaborador");
            }
            catch (Exception ex)
            {
                ViewBag.MensagemErro = ex.Message;
                return View("_erro");
            }
        }
        [HttpGet]
        public ActionResult PontoColaborador(int id)
        {

            var colabProj = ProjColaboradorDao.BuscarProjetoColaborador(id);
            var colaborador = ColaboradoresDao.BuscarColaborador(colabProj.IDCOLABORADOR);
            var projeto = ProjetosDao.BuscarProjeto(colabProj.IDPROJETO);
            var atividade = new Atividade() { IDPROJ_COLAB = id };
            ViewBag.Lista = AtividadesDao.ListarPontosPorProjeto(id).ToList();
            ViewBag.Colaborador = ColaboradoresDao.BuscarColaborador(colabProj.IDCOLABORADOR);
            ViewBag.Colab = new ColaboradorProjetoViewModel()
            {
                Id = id,
                Descricao = projeto.Descricao + " / " + colaborador.Nome
            };
            return View(atividade);


        }


        [HttpPost]
        public ActionResult PontoColaborador(Atividade atividade)
        {
            try
            {
                var colabProjeto = ProjColaboradorDao.BuscarProjetoColaborador(atividade.IDPROJ_COLAB);
                var listaAtividadesColab = AtividadesDao.ListarPontosPorcolaborador(colabProjeto.IDPROJ_COLAB);

                foreach (var item in listaAtividadesColab)
                {
                    if (atividade.HoraInicio == item.HoraInicio && atividade.HoraFim == item.HoraFim)
                    {
                        ModelState.AddModelError("HoraFim", "O calaborador já possui uma ativididade em andamento nesse horário no projeto " + item.ProjetoColaborador.Projeto.Descricao);
                        break;
                    }
                    else if (atividade.HoraInicio > atividade.HoraFim)
                    {
                        ModelState.AddModelError("HoraInicio", "O horário de entrada não pode ser maior que o horário de saída");
                        break;
                    }
                    else if (atividade.HoraInicio == atividade.HoraFim)
                    {
                        ModelState.AddModelError("HoraInicio", "O horário de entrada não pode ser igual ao horário de saída");
                        break;
                    }

                    else if (atividade.HoraInicio >= item.HoraInicio && atividade.HoraInicio <= item.HoraFim)//entrar no meio do projeto
                    {
                        ModelState.AddModelError("HoraInicio", "O calaborador já possui uma ativididade em andamento nesse horário no projeto " + item.ProjetoColaborador.Projeto.Descricao);
                        break;
                    }
                    else if ((atividade.HoraInicio < item.HoraInicio && atividade.HoraFim > item.HoraInicio && atividade.HoraFim <= item.HoraFim) || //entrar antes e saior no meio
                        (atividade.HoraInicio < item.HoraInicio && atividade.HoraFim > item.HoraFim)) //entrar antes e sair depois
                    {
                        ModelState.AddModelError("HoraFim", "O calaborador já possui uma ativididade em andamento nesse horário no projeto " + item.ProjetoColaborador.Projeto.Descricao);
                        break;
                    }

                }

                if (!ModelState.IsValid)
                {
                    return PontoColaborador(atividade.IDPROJ_COLAB);
                }
                AtividadesDao.IncluirPonto(atividade);
                TempData["Mensagem"] = "Atividade registrada com sucesso";
                return RedirectToAction("PontoColaborador", colabProjeto.IDCOLABORADOR);
            }
            catch (Exception ex)
            {
                ViewBag.MensagemErro = ex.Message;
                return View("_Erro");
            }
        }












    }
}
