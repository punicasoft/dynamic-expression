using Punica.Linq.Dynamic.Expressions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Punica.Linq.Dynamic.Abstractions
{
    public interface INewExpression : IExpression
    {
        void AddArgument(Argument token);
    }
}
