using System;
using System.Collections.Generic;

namespace EnglishAPI.Data;

public partial class Avatar
{
    public int AvatarId { get; set; }

    public string? Description { get; set; }

    public string? Image { get; set; }

    public virtual ICollection<User> Users { get; set; } = new List<User>();
}
