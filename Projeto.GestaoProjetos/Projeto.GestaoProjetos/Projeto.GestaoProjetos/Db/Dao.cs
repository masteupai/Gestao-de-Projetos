using Projeto.GestaoProjetos.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace Projeto.GestaoProjetos.Db
{
    public class Dao<T ,K> where T:class
    {
        public void ExecutarTarefa(T item, TipoOperacaoBd tipo)
        {
            using (var ctx = new ProjetoConnection())
            {
                ctx.Entry<T>(item).State = (EntityState)tipo;
                ctx.SaveChanges();
            }
        }
        //public void Adicionar(T item)
        //{
        //    using (var ctx = new ProjetoConnection())
        //    {
        //        ctx.Entry<T>(item).State = EntityState.Added;
        //        ctx.SaveChanges();
        //    }
        //}
        //public void Alterar(T item)
        //{
        //    using (var ctx = new ProjetoConnection())
        //    {
        //        ctx.Entry<T>(item).State = EntityState.Modified;
        //        ctx.SaveChanges();
        //    }
        //}
        //public void Deletar(T item)
        //{
        //    using (var ctx = new ProjetoConnection())
        //    {
        //        ctx.Entry<T>(item).State = EntityState.Deleted;
        //        ctx.SaveChanges();
        //    }
        //}
        public IEnumerable<T> Listar()
        {
            using (var ctx = new ProjetoConnection())
            {
                return ctx.Set<T>().ToList();
            }
        }
        public T Buscar(K chave)
        {
            using (var ctx = new ProjetoConnection())
            {
                return ctx.Set<T>().Find(chave);
            }
        }
    }
}