using Projeto.GestaoProjetos.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.Entity;

namespace Projeto.GestaoProjetos.Db
{
    public class AtividadesDao
    {
        public static void IncluirPonto(Atividade atividade)
        {
            using (var ctx = new ProjetoConnection())
            {
                ctx.Atividades.Add(atividade);
                ctx.SaveChanges();
            }
        } 
        public static IEnumerable<Atividade> ListarPontos()
        {
            using (var ctx = new ProjetoConnection())
            {
                return ctx.Atividades.Include(c => c.ProjetoColaborador).Include(p => p.ProjetoColaborador.Projeto).ToList();
            }
        }
        public static IEnumerable<Atividade> ListarPontosPorProjeto(int id)
        {
            using (var ctx = new ProjetoConnection())
            {
                return ctx.Atividades.Include(c => c.ProjetoColaborador).Include(p => p.ProjetoColaborador.Projeto).Where(a => a.IDPROJ_COLAB == id).ToList();
            }
        }
        public static IEnumerable<Atividade> ListarPontosPorcolaborador(int id)
        {            
            using (var ctx = new ProjetoConnection())
            {
                return ctx.Atividades.Include(c=>c.ProjetoColaborador).Include(p => p.ProjetoColaborador.Projeto).Where(a=>a.IDPROJ_COLAB == id).ToList();
            }
        }
                     
        public static int TotalHorasColaborador(int id)
        {
            using (var ctx = new ProjetoConnection())
            {
                var colaborador = ctx.Atividades.Where(i => i.IDPROJ_COLAB == id);
                if(colaborador.Count() > 0)
                {
                    var horas = ctx.Atividades.GroupBy(i => i.IDPROJ_COLAB).FirstOrDefault(i => i.Key == id)
                        .Sum(p => p.HoraFim.Subtract(p.HoraInicio).TotalHours);
                    return (int)horas;
                }
                else
                {
                    return 0;
                }
            }
        }
        public static int TotalHorasProjetos(int id)
        {
            using (var ctx = new ProjetoConnection())
            {
                var colab = ctx.Proj_Colaboradores.Where(p => p.IDPROJETO == id);
                var soma = 0;
                foreach (var item in colab)
                {
                    soma += TotalHorasColaborador(item.IDPROJ_COLAB);
                }
                return soma;
            }
        }

        public static int TotalHorasRestante(int id)
        {
            using (var ctx = new ProjetoConnection())
            {
                var horasRestante = ctx.CadProjetos.FirstOrDefault(p => p.IDPROJETO == id).NumHoras;
                return (int)horasRestante - TotalHorasProjetos(id);
            }
        }
    }
}