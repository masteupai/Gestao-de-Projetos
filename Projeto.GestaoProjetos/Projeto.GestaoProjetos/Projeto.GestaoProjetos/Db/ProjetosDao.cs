using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using Projeto.GestaoProjetos.Models;
using Projeto.GestaoProjetos.ViewModel;

namespace Projeto.GestaoProjetos.Db
{
    public class ProjetosDao
    {
        public static void IncluirProjeto(CadProjeto projeto)
        {
            using (var ctx = new ProjetoConnection())
            {
                ctx.CadProjetos.Add(projeto);
                ctx.SaveChanges();
            }
        }
        public static IEnumerable<CadProjeto> ListarProjetos()
        {
            using (var ctx = new ProjetoConnection())
            {
                return ctx.CadProjetos.Include(c => c.ClienteInfo).ToList();
            }
        }      
        public static IEnumerable<ProjetosViewModel> ListarProjetosInfo()
        {
            using (var ctx = new ProjetoConnection())
            {
                var projetos = ctx.CadProjetos.ToList();
                List<ProjetosViewModel> listaproj = new List<ProjetosViewModel>();

                foreach (var item in projetos)
                {
                    listaproj.Add(new ProjetosViewModel() { IdSkill = item.IDPROJETO, Descricao = item.Descricao + " - " + item.IDCLIENTE });
                };
                return listaproj;
            }
        }
        
        public static void AlterarProjeto(CadProjeto projeto)
        {
            using (var ctx = new ProjetoConnection())
            {
                ctx.Entry<CadProjeto>(projeto).State = EntityState.Modified;
                ctx.SaveChanges();
            }
        }


        public static CadProjeto BuscarProjeto(int idProjeto)
        {
            using (var ctx = new ProjetoConnection())
            {
                var projeto = ctx.CadProjetos.Include(c=>c.ClienteInfo).Include(p=>p.ProjetoColaborador).FirstOrDefault(p => p.IDPROJETO.Equals(idProjeto));
                return projeto;
            }
        }

        public static IEnumerable<CadProjeto> ListaProjetosCliente(int id)
        {
            return ListarProjetos().Where(p => p.IDCLIENTE == id).ToList();
        }
    }
}