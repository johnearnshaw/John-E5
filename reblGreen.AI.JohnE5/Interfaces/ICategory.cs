using System;
using System.Collections.Generic;
using System.Text;

namespace reblGreen.AI.JohnE5.Interfaces
{
    public interface ICategory
    {
        int Id { get; }
        string Name { get; }
        int ParentCategory { get; }
        List<int> LinkedCategories { get; }

        void SetParent(ICategory parent);
        void LinkCategory(ICategory link);
    }
}
