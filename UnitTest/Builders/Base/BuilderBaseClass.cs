using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UnitTest.Builders.Base
{
    public interface IBuilderBaseClass <out T>
    {
        T Build();
    }
   public abstract class BuilderBaseClass<T> : IBuilderBaseClass<T>  where T : class, new ()
      
    {
        protected T _class;

    protected BuilderBaseClass()
    {
        _class = new T();
    }

    public T Build()
    {
        return _class;
    }
}
}
