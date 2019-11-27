using System;
using System.Collections.Generic;
using System.Text;

namespace reblGreen.AI.JohnE5.Interfaces
{
    public interface IModel
    {
        List<ICategory> Categories { get; }
        List<IToken> Tokens { get; }
        ICategory AddCategory(string name);
        ICategory GetCategory(int category);
        ICategory GetCategory(ICategory category);
        bool RemoveCategory(int id);
        List<ICategory> GetCategoryHierarchy(ICategory category);
        List<IToken> GetCategoryTokens(ICategory category);
        List<IRankedCategory> GetAllCategories();
        List<IRankedCategory> GetWeightedCategories(string[] tokens, string negatedPrefix = null);
        void TrainModel(ICategory category, string[] tokens, float trainingRate);
        void Normalize();
        bool Save();
    }
}
