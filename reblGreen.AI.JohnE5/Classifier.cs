using reblGreen.AI.JohnE5.Classes;
using reblGreen.AI.JohnE5.Interfaces;
using System;
using System.Collections.Generic;

namespace reblGreen.AI.JohnE5
{
    public class Classifier
    {
        readonly float TrainingRate = 0.01f;
        public readonly IModel Model;
        

        /// <summary>
        /// 
        /// </summary>
        public Classifier (IModel model)
        {
            Model = model;
        }


        /// <summary>
        /// 
        /// </summary>
        public Classifier(IModel model, float trainingRate) : this(model)
        {
            TrainingRate = trainingRate;
        }


        /// <summary>
        /// 
        /// </summary>
        public List<IRankedCategory> GetWeightedCategories(string[] tokens, string negatedPrefix = null)
        {
            return Model.GetWeightedCategories(tokens, negatedPrefix);
        }


        /// <summary>
        /// 
        /// </summary>
        public void TrainModel(ICategory category, string[] tokens)
        {
            Model.TrainModel(category, tokens, TrainingRate);
        }


        /// <summary>
        /// 
        /// </summary>
        public List<IRankedCategory> GetTrainingSuggestions()
        {
            List<IRankedCategory> ret = new List<IRankedCategory>();

            Dictionary<int, int> wordCounts = new Dictionary<int, int>();

            foreach (IToken phrase in Model.Tokens)
            {
                foreach (IRank rank in phrase.Scores)
                {
                    ICategory cat = Model.GetCategory(rank.Id);

                    if (wordCounts.ContainsKey(cat.Id))
                    {
                        wordCounts[cat.Id]++;
                    }
                    else
                    {
                        wordCounts.Add(cat.Id, 1);
                    }

                    IRankedCategory c = new RankedCategory()
                    {
                        Id = cat.Id,
                        Name = cat.Name,
                    };

                    int index = ret.IndexOf(c);

                    if (index < 0)
                    {
                        ret.Add(c);
                    }
                    else
                    {
                        ret[index].Score++;
                    }
                }
            }

            foreach (IRankedCategory cat in ret)
            {
                cat.Score = cat.Score / wordCounts[((ICategory)cat).Id];
            }

            ret.Sort();
            ret.Reverse();

            return ret;
        }
    }
}
