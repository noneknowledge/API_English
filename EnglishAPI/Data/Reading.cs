using System;
using System.Collections.Generic;

namespace EnglishAPI.Data;

public partial class Reading
{
    public int ReadId { get; set; }

    public string? Paragraph { get; set; }

    public string? Question { get; set; }

    public string? Answer { get; set; }

    public int? LessionId { get; set; }

    public string? Question2 { get; set; }

    public string? Answer2 { get; set; }

    public virtual Lession? Lession { get; set; }
}
