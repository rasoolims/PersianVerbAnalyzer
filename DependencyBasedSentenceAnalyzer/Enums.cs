using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DependencyBasedSentenceAnalyzer
{
    [Flags]
    public enum NumberType
    {
        INVALID = 0,
        SINGULAR = 1,
        PLURAL = 2
    }


    [Flags]
    public enum Chasbidegi
    {
        TANHA = 0,
        NEXT = 1,
        PREV = 2
    }
}
