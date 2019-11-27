using reblGreen.AI.JohnE5.Interfaces;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;

namespace reblGreen.AI.JohnE5.Classes
{
    [Serializable]
    public class BinaryModel : IModel
    {
        [NonSerialized]
        public string FilePath;

        public List<ICategory> Categories { get; set; }
        public List<IToken> Tokens { get; set; }

        private BinaryModel (string filePath)
        {
            FilePath = filePath;
        }


        public static IModel FromFile(string filePath)
        {
            return DeserializeModelFromFile(filePath);
        }


        public bool Save()
        {
            return SerializeModelToFile();
        }


        /// <summary>
        /// Returns a newly initialized empty Category object to the model.
        /// </summary>
        public ICategory AddCategory(string name)
        {
            if (string.IsNullOrWhiteSpace(name))
            {
                throw new Exception("The category name must be a valid string.");
            }

            var lastCat = Categories.LastOrDefault();
            var lastIndex = 0;

            if (lastCat != null)
            {
                lastIndex = lastCat.Id+1;
            }

            var cat = new Category { Id = lastIndex, Name = name };
            Categories.Add(cat);
            return cat;
        }


        /// <summary>
        /// 
        /// </summary>
        public ICategory GetCategory(int category)
        {
            return Categories.FirstOrDefault(x => x.Id == category);
        }


        /// <summary>
        /// 
        /// </summary>
        public ICategory GetCategory(ICategory category)
        {
            return GetCategory(category.Id);
        }


        /// <summary>
        /// Removes a category from the model.
        /// Removing a category can be very expensive since it is required to itterate each token's scores
        /// and remove reference to the category id which is being removed.
        /// </summary>
        public bool RemoveCategory(int id)
        {
            var cat = Categories.FirstOrDefault(c => c.Id == id);
            if (cat == null)
            {
                return false;
            }

            Categories.Remove(cat);
            foreach (var t in Tokens)
            {
                foreach (var r in t.Scores)
                {
                    if (r.Id == id)
                    {
                        t.Scores.Remove(r);
                    }
                }
            }
            return true;
        }


        /// <summary>
        /// 
        /// </summary>
        public List<ICategory> GetCategoryHierarchy(ICategory category)
        {
            ICategory cat = GetCategory(category);

            if (category == null)
            {
                throw new Exception("Invalid category: Category not found with Id " + category.Id);
            }

            List<ICategory> list = new List<ICategory>() { cat, };

            if (cat.ParentCategory > -1)
            {
                ICategory current = cat;
                while (current.ParentCategory != -1)
                {
                    ICategory next = GetCategory(new Category() { Id = cat.ParentCategory });

                    if (next == null)
                    {
                        break;
                    }

                    list.Add(next);
                    current = next;
                }
            }

            return list;
        }


        /// <summary>
        /// 
        /// </summary>
        public List<IToken> GetCategoryTokens(ICategory category)
        {
            ICategory cat = GetCategory(category);

            if (cat == null)
            {
                throw new Exception("Invalid category.");
            }

            IRank rank = new Rank() { Id = cat.Id };
            List<IToken> ret = Tokens.Where(x => x.Scores.Contains(rank)).ToList();

            ret.Sort((x, y) => x.Scores[x.Scores.IndexOf(rank)].CompareTo(y.Scores[y.Scores.IndexOf(rank)]));

            return ret;
        }


        /// <summary>
        /// 
        /// </summary>
        public List<IRankedCategory> GetAllCategories()
        {
            List<IRankedCategory> ret = new List<IRankedCategory>();

            double highestScore = 0;

            foreach (IToken token in Tokens)
            {
                foreach (IRank rank in token.Scores)
                {
                    if (highestScore < rank.Score)
                    {
                        highestScore = rank.Score;
                    }

                    var c = GetCategory(rank.Id);

                    var ranked = new RankedCategory()
                    {
                        Id = c.Id,
                        Name = c.Name,
                        LinkedCategories = c.LinkedCategories,
                        ParentCategory = c.ParentCategory
                    };

                    int index = ret.IndexOf(ranked);
                    if (index < 0)
                    {
                        ret.Add(ranked);
                    }
                    else
                    {
                        ret[index].Score++;
                    }
                }
            }

            ret.Sort();
            return ret;
        }


        public List<IRankedCategory> GetWeightedCategories(string[] tokens, string negatedPrefix = null)
        {
            List<IRankedCategory> ret = new List<IRankedCategory>();

            float divisor = 0.0f;
            bool negated = false;
            string token;

            foreach (var t in tokens)
            {
                if (!string.IsNullOrEmpty(negatedPrefix) && t.StartsWith(negatedPrefix))
                {
                    negated = true;
                    token = t.Replace(negatedPrefix, "").ToLowerInvariant();
                }
                else
                {
                    negated = false;
                    token = t.ToLowerInvariant();
                }

                Dictionary<int, List<double>> dic = new Dictionary<int, List<double>>();

                List<IToken> knownTokens = Tokens.FindAll(x => x.Value == token);

                foreach (IToken known in knownTokens)
                {
                    foreach (IRank rank in known.Scores)
                    {
                        double val = negated ? 1.0f - rank.Score : rank.Score;

                        if (!dic.ContainsKey(rank.Id))
                        {
                            dic.Add(rank.Id, new List<double>() { val });
                        }
                        else
                        {
                            dic[rank.Id].Add(val);
                        }
                    }
                }

                foreach (KeyValuePair<int, List<double>> kv in dic)
                {
                    ICategory category = GetCategory(kv.Key);

                    if (category == null)
                    {
                        continue;
                    }

                    IRankedCategory ranked = new RankedCategory()
                    {
                        Id = category.Id,
                        Name = category.Name,
                        LinkedCategories = category.LinkedCategories,
                        ParentCategory = category.ParentCategory
                    };

                    foreach (var cat in kv.Value)
                    {
                        ranked.Score += (float)cat;
                    }


                    if (ranked.Score <= 0)
                    {
                        continue;
                    }

                    int index = ret.IndexOf(ranked);

                    if (index > -1)
                    {
                        ret[index].Score += ranked.Score;
                    }
                    else
                    {
                        ret.Add(ranked);
                    }

                    divisor += ranked.Score;
                }
            }

            foreach (IRankedCategory cat in ret)
            {
                cat.Score = cat.Score / divisor;
            }

            ret.Sort();
            return ret;
        }


        public void TrainModel(ICategory category, string[] tokens, float trainingRate)
        {
            category = GetCategory(category);

            if (category == null)
            {
                throw new Exception("Invalid category: Category not found with Id " + category.Id);
            }

            List<ICategory> categories = GetCategoryHierarchy(category);

            List<IToken> newTokens = new List<IToken>();
            Dictionary<string, int> uniques = new Dictionary<string, int>();

            float error;
            float currentScore;
            float wordWeight;
            float output;
            
            foreach (var t in tokens)
            {
                var token = t.ToLowerInvariant();

                if (uniques.ContainsKey(token))
                {
                    uniques[token]++;
                }
                else
                {
                    uniques.Add(token, 1);
                }
            }

            foreach (KeyValuePair<string, int> kv in uniques)
            {
                IToken token = Tokens.Find(x => x.Value == kv.Key);

                if (token == null)
                {
                    token = new Token(kv.Key);
                    Tokens.Add(token);
                }

                foreach (var cat in categories)
                {
                    IRank rank = new Rank() { Id = category.Id };

                    if (!token.Scores.Contains(rank))
                    {
                        token.Scores.Add(rank);
                    }

                    foreach (IRank r in token.Scores)
                    {
                        if (category.LinkedCategories != null
                            && category.LinkedCategories.Contains(rank.Id))
                        {
                            continue;
                        }

                        currentScore = r.Score;

                        if (r.Id == cat.Id)
                        {
                            output = 1.0f;
                        }
                        else
                        {
                            output = 0.0f;
                        }

                        wordWeight = 1.0f / token.Scores.Count;
                        error = output - currentScore;

                        r.Score += trainingRate * error * wordWeight;
                    }
                }
            }

            Save();
        }


        /// <summary>
        /// 
        /// </summary>
        public void Normalize()
        {
            float norm = 0.0f;
            float averageNorm = 0.0f;
            int catCount = 0;

            List<double> scores = new List<double>();

            foreach (IToken t in Tokens)
            {
                foreach (IRank rank in t.Scores)
                {
                    catCount++;
                    norm += rank.Score * rank.Score;
                }

                norm = (float)Math.Sqrt(norm);
                averageNorm += norm;
            }

            averageNorm = averageNorm / catCount;

            foreach (IToken t in Tokens)
            {
                foreach (IRank rank in t.Scores)
                {
                    rank.Score = rank.Score / averageNorm;
                }
            }
        }
        

        /// <summary>
        /// 
        /// </summary>
        public bool SerializeModelToFile()
        {
            try
            {
                if (!File.Exists(FilePath))
                {
                    File.Create(FilePath).Dispose();
                }

                using (FileStream fs = new FileStream(FilePath, FileMode.OpenOrCreate, FileAccess.ReadWrite, FileShare.Read))
                {
                    fs.Position = 0;
                    fs.SetLength(0);

                    BinaryFormatter binFormatter = new BinaryFormatter();
                    binFormatter.Serialize(fs, this);
                }
                return true;
            }
            catch (Exception ex)
            {
                throw;
                return false;
            }
        }


        /// <summary>
        /// 
        /// </summary>
        public static BinaryModel DeserializeModelFromFile(string filePath)
        {
            try
            {
                if (!File.Exists(filePath))
                {
                    File.Create(filePath).Dispose();
                    return new BinaryModel(filePath)
                    {
                        Categories = new List<ICategory>(),
                        Tokens = new List<IToken>()
                    };
                }

                using (FileStream fs = new FileStream(filePath, FileMode.Open, FileAccess.Read, FileShare.Read))
                {
                    var formatter = new BinaryFormatter();

                    try
                    {
                        var ret = (BinaryModel)formatter.Deserialize(fs);
                        ret.FilePath = filePath;
                        return ret;
                    }
                    catch { }
                }

                return new BinaryModel(filePath)
                {
                    Categories = new List<ICategory>(),
                    Tokens = new List<IToken>()
                };
            }
            catch (Exception ex)
            {    
                throw;
                return new BinaryModel(filePath);
            }
        }
    }
}
