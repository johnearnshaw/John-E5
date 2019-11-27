using System;
using System.Collections.Generic;
using System.Text;

namespace reblGreen.AI.JohnE5.Interfaces
{
    public interface IRank : IComparable<IRank>
    {
        int Id { get; }
        float Score { get; set; }
    }
}
