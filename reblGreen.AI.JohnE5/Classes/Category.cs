using reblGreen.AI.JohnE5.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace reblGreen.AI.JohnE5.Classes
{
    [Serializable]
    public class Category : ICategory
    {
        internal Category()
        {

        }

        public int Id { get; internal set; }

        public string Name { get; internal set; }

        public int ParentCategory { get; internal set; } = -1;

        public List<int> LinkedCategories { get; internal set; }


        /// <summary>
        /// Set the parent category id for this category.
        /// </summary>
        public void SetParent(ICategory parent)
        {
            if (parent == null)
            {
                throw new Exception("Invalid parent.");
            }

            ParentCategory = parent.Id;
        }


        /// <summary>
        /// Link a category id to this category.
        /// </summary>
        public void LinkCategory(ICategory link)
        {
            if (LinkedCategories == null)
            {
                LinkedCategories = new List<int>();
            }

            if (!LinkedCategories.Contains(link.Id))
            {
                LinkedCategories.Add(link.Id);
            }
        }


        public override bool Equals(object obj)
        {
            if (obj is Category c)
            {
                return c.Id.Equals(Id);
            }
            return base.Equals(obj);
        }

        public override int GetHashCode()
        {
            return Id.GetHashCode();
        }

        public override string ToString()
        {
            return Name;
        }

        public static bool operator ==(Category x, ICategory y)
        {
            return x.Id == y.Id;
        }

        public static bool operator !=(Category x, ICategory y)
        {
            return x.Id != y.Id;
        }
    }
}
