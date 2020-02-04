using Projeto.GestaoProjetos.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Data.Entity;
using System.Web;

namespace Projeto.GestaoProjetos.Db
{
    public class ProjColaboradorDao
    {
        public static void IncluirColabProjeto(ProjetosColaborador projetosColaborador)
        {
            using (var ctx = new ProjetoConnection())
            {
                var projetocolab = ctx.Proj_Colaboradores.FirstOrDefault(p => p.IDCOLABORADOR.Equals(projetosColaborador.IDCOLABORADOR));
                if (projetocolab == null) projetocolab = new ProjetosColaborador();
                if (projetocolab == null || projetocolab.IDCOLABORADOR != projetosColaborador.IDCOLABORADOR && projetocolab.IDPROJETO != projetosColaborador.IDPROJETO)
                {
                    ctx.Proj_Colaboradores.Add(projetosColaborador);
                    ctx.SaveChanges();

                }
                else
                {
                    throw new Exception("Este colaborador já está alocado neste projeto!");
                }

            }
        }
        public static ProjetosColaborador BuscarProjetoColaborador(int id)
        {
            using (var ctx = new ProjetoConnection())
            {
                return ctx.Proj_Colaboradores.Include(p => p.Projeto).Include(p=>p.ColaboradorInfo).Include(s=>s.SkillInfo).FirstOrDefault(s => s.IDPROJ_COLAB == id);
            }
        }


        public static IEnumerable<ProjetosColaborador> ListarProjetosColaborador(int id)
        {
            using (var ctx = new ProjetoConnection())
            {
                return ctx.Proj_Colaboradores.Where(p => p.IDCOLABORADOR == id)
                    .Include(p => p.Projeto)
                    .Include(p => p.ColaboradorInfo)
                    .Include(p => p.SkillInfo)
                    .ToList();
            }
        }
        public static IEnumerable<ProjetosColaborador> ListarColaboradoresProjeto(int id)
        {
            using (var ctx = new ProjetoConnection())
            {
                return ctx.Proj_Colaboradores.Where(p => p.IDPROJETO == id)
                    .Include(p => p.Projeto)
                    .Include(p => p.ColaboradorInfo)
                    .Include(p => p.SkillInfo)
                    .ToList();
            }
        }
        public static IEnumerable<ProjetosColaborador> ListarColaboradoresProjeto()
        {
            using (var ctx = new ProjetoConnection())
            {
                return ctx.Proj_Colaboradores.Include(c => c.ColaboradorInfo)/*.Include(i=>i.Projeto).Include(c=>c.TBCOLABORADORES).Include(s=>s.TBSKILLS)*/.ToList();
            }
        }

        public static void DeleteProjColab(ProjetosColaborador projetoColaborador)
        {
            using (var ctx = new ProjetoConnection())
            {
                if (projetoColaborador.IDPROJ_COLAB == 0) throw new Exception("Projeto/Colaborador Nulo!");
                ctx.Entry<ProjetosColaborador>(projetoColaborador).State = EntityState.Deleted;
                ctx.SaveChanges();
            }
        }

    }
}