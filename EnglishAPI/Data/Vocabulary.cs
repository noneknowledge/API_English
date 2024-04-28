using System;
using System.Collections.Generic;

namespace EnglishAPI.Data;

public partial class Vocabulary
{
    public int VocabId { get; set; }

    public string? Vocab { get; set; }

    public string? Vietnamese { get; set; }

    public string? Image { get; set; }

    public int? LessionId { get; set; }

    public string? WordClass { get; set; }

    public virtual Lession? Lession { get; set; }

    public virtual ICollection<UserProgress> UserProgresses { get; set; } = new List<UserProgress>();
}
