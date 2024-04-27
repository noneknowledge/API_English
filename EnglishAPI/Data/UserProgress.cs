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
}
