using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Projeto.GestaoProjetos.Db
{
    public enum TipoOperacaoBd
    {
        Detached = 1,
        Unchanged = 2,
        Added = 4,
        Deleted = 8,
        Modified = 16
    }
}