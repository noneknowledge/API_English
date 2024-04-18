using System;
using System.Collections.Generic;

namespace EnglishAPI.Data;

public partial class Sentence
{
    public int SenId { get; set; }

    public string? BlankSentence { get; set; }

    public string? FillWord { get; set; }

    public int? LessionId { get; set; }

    public string? Hint { get; set; }

    public string? Vietnamese { get; set; }

    public virtual Lession? Lession { get; set; }
}
