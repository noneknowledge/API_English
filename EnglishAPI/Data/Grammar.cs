using System;
using System.Collections.Generic;

namespace EnglishAPI.Data;

public partial class Grammar
{
    public int GrammarId { get; set; }

    public string? Formula { get; set; }

    public string? Example { get; set; }

    public string? Note { get; set; }

    public int? LessionId { get; set; }

    public virtual Lession? Lession { get; set; }
}
