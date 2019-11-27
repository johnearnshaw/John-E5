using reblGreen.AI.JohnE5.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace reblGreen.AI.JohnE5.Classes
{
    [Serializable]
    public class Token : IToken
    {
        public string Value { get; internal set; }
        public List<IRank> Scores { get; internal set; }

        internal Token(string value)
        {
            Value = value;
            Scores = new List<IRank>();
        }


        public override bool Equals(object obj)
        {
            if (obj is Token t)
            {
                return t.Value.ToLowerInvariant().Equals(Value.ToLowerInvariant());
            }
            return base.Equals(obj);
        }

        public override int GetHashCode()
        {
            return Value.ToLowerInvariant().GetHashCode();
        }

        public override string ToString()
        {
            return Value.ToLowerInvariant();
        }

        public static bool operator ==(Token x, IToken y)
        {
            return x.Value.ToLowerInvariant() == y.Value.ToLowerInvariant();
        }

        public static bool operator !=(Token x, IToken y)
        {
            return x.Value.ToLowerInvariant() != y.Value.ToLowerInvariant();
        }
    }
}
