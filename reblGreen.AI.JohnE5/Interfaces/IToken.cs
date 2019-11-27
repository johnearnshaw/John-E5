using System;
using System.Collections.Generic;
using System.Text;

namespace reblGreen.AI.JohnE5.Interfaces
{
    public interface IToken
    {
        string Value { get; }
        List<IRank> Scores { get; }
    }
}
