using System;
using System.Collections.Generic;
using System.Text;

namespace Avis
{
    public abstract class UExpression
    {
        public UExpression(string name, string abbr)
        {
            _name = name;
            _abbr = abbr;
        }

        protected string _name;
        protected string _abbr;

        public virtual string Name => _name;

        public virtual string Abbr => _abbr;

        public abstract string Type { get; }
        public abstract object Data { set; get; }
    }
}
