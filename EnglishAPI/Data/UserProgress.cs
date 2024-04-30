using System;
using System.Collections.Generic;

namespace EnglishAPI.Data;

public partial class UserProgress
{
    public int Id { get; set; }

    public int UserId { get; set; }

    public int? VocabId { get; set; }

    public int? SentenceId { get; set; }

    public int? ReadingId { get; set; }

    public string? IsTrue { get; set; }

    public string? AdditionalAnswer { get; set; }

    public virtual Reading? Reading { get; set; }

    public virtual Sentence? Sentence { get; set; }

    public virtual User User { get; set; } = null!;

    public virtual Vocabulary? Vocab { get; set; }
}
