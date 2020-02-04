using Projeto.GestaoProjetos.Models;
using Projeto.GestaoProjetos.ViewModel;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace Projeto.GestaoProjetos.Db
{
    public class ColaboradoresDao
    {
        //public static void IncluirSkills(Skill skill)
        //{
        //    using (var ctx = new ProjetoConnection())
        //    {
        //        ctx.Skills.Add(skill);
        //        ctx.SaveChanges();
        //    }
        //}
        //public static IEnumerable<Skill> ListarSkills()
        //{
        //    using (var ctx = new ProjetoConnection())
        //    {
        //        var skills = ctx.Skills.ToList();
        //        return skills;

        //    }
        //}
        public static Skill BuscarSkills(int idSkill)
        {
            using (var ctx = new ProjetoConnection())
            {
                var skill = ctx.Skills.FirstOrDefault(p => p.IDSKILL.Equals(idSkill));
                return skill;
            }
        }
        //public static void AlterarSkill(Skill skill)
        //{
        //    using (var ctx = new ProjetoConnection())
        //    {
        //        ctx.Entry<Skill>(skill).State = EntityState.Modified;
        //        ctx.SaveChanges();
        //    }
        //}
        //public static void ExcluirSkill(Skill skill)
        //{
        //    using (var ctx = new ProjetoConnection())
        //    {
        //        ctx.Entry<Skill>(skill).State = EntityState.Deleted;
        //        ctx.SaveChanges();
        //    }
        //}
        


        




        public static IEnumerable<Colaborador> ListarColaboradores()
        {
            using (var ctx = new ProjetoConnection())
            {
                var colaboradores = ctx.Colaboradores.ToList();
                return colaboradores;
            }
        }

        
        
        public static void IncluirColaborador(Colaborador colaborador)
        {
            using (var ctx = new ProjetoConnection())
            {
                ctx.Colaboradores.Add(colaborador);
                ctx.SaveChanges();
            }
        }

        public static Colaborador BuscarColaborador(int idColaborador)
        {
            using (var ctx = new ProjetoConnection())
            {
                var colaborador = ctx.Colaboradores.Include(p=>p.ProjetoColaborador).FirstOrDefault(p => p.IDCOLABORADOR.Equals(idColaborador));
                return colaborador;
            }
        }
        public static void AlterarColaborador(Colaborador colaborador)
        {
            using (var ctx = new ProjetoConnection())
            {
                ctx.Entry<Colaborador>(colaborador).State = EntityState.Modified;
                ctx.SaveChanges();
            }
        }
        public static void ExcluirColaborador(Colaborador colaborador)
        {
            using (var ctx = new ProjetoConnection())
            {
                ctx.Entry<Colaborador>(colaborador).State = EntityState.Deleted;
                ctx.SaveChanges();
            }
        }
        public static IEnumerable<SkillsViewModel> ListarSkillsInfo()
        {
            using (var ctx = new ProjetoConnection())
            {
                var skills = ctx.Skills.ToList();
                var listaskills = new List<SkillsViewModel>();
                foreach (var item in skills)
                {
                    listaskills.Add(new SkillsViewModel() { IdSkill = item.IDSKILL, Descricao = item.Descricao + " - " + item.Nivel });
                }

                return listaskills;

            }
        }
        public static SkillsViewModel BuscarSkillInfo(int id)
        {
            using (var ctx = new ProjetoConnection())
            {
                var objSkill = BuscarSkills(id);
                var skill = new SkillsViewModel() { IdSkill = objSkill.IDSKILL, Descricao = objSkill.Descricao + " - " + objSkill.Nivel };
                
                return skill;
            }
        }
    }
}