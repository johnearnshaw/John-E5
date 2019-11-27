using reblGreen.AI.JohnE5.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace reblGreen.AI.JohnE5.Classes
{
    [Serializable]
    public class RankedCategory : Category, IRankedCategory
    {
        public float Score { get; set; }

        
        /// <summary>
        /// 
        /// </summary>
        public int CompareTo(IRank other)
        {
            return other.Score.CompareTo(Score);
        }


        /// <summary>
        /// 
        /// </summary>
        public static IRankedCategory FromCategory(ICategory category)
        {
            return new RankedCategory()
            {
                Id = category.Id,
                Name = category.Name,
                LinkedCategories = category.LinkedCategories,
                ParentCategory = category.ParentCategory,
                Score = 0
            };
        }
    }
}
