using System;
using System.Collections.Generic;

namespace EnglishAPI.Data;

public partial class Lession
{
    public int LessionId { get; set; }

    public string Title { get; set; } = null!;

    public string? Description { get; set; }

    public string? Image { get; set; }

    public string? Vietnamese { get; set; }

    public virtual ICollection<Grammar> Grammars { get; set; } = new List<Grammar>();

    public virtual ICollection<Reading> Readings { get; set; } = new List<Reading>();

    public virtual ICollection<Sentence> Sentences { get; set; } = new List<Sentence>();

    public virtual ICollection<UserLession> UserLessions { get; set; } = new List<UserLession>();

    public virtual ICollection<Vocabulary> Vocabularies { get; set; } = new List<Vocabulary>();
}
