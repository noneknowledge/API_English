using System;
using System.Collections.Generic;

namespace EnglishAPI.Data;

public partial class UserLession
{
    public int UserId { get; set; }

    public int LessionId { get; set; }

    public int? HighScore { get; set; }

    public string? Comment { get; set; }

    public string? Status { get; set; }

    public virtual Lession Lession { get; set; } = null!;

    public virtual User User { get; set; } = null!;
}
