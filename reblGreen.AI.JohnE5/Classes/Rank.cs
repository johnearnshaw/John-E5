using reblGreen.AI.JohnE5.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace reblGreen.AI.JohnE5.Classes
{
    [Serializable]
    internal class Rank : IRank, IComparable<IRank>
    {
        /// <summary>
        /// The category Id in which this rank is linked to.
        /// </summary>
        public int Id { get; set; }
        public float Score { get; set; }


        public int CompareTo(IRank other)
        {
            return other.Id.CompareTo(Id);
        }


        public override bool Equals(object obj)
        {
            if (obj is IRank r)
            {
                return r.Id == Id;
            }
            return base.Equals(obj);
        }

        public override int GetHashCode()
        {
            return Id.GetHashCode();
        }

        public override string ToString()
        {
            return Score.ToString();
        }

        public static bool operator ==(Rank x, IRank y)
        {
            return x.Id == y.Id;
        }

        public static bool operator !=(Rank x, IRank y)
        {
            return x.Id != y.Id;
        }
    }
}
