using System;
using System.Collections.Generic;

namespace EnglishAPI.Data;

public partial class User
{
    public int UserId { get; set; }

    public string UserName { get; set; } = null!;

    public string Password { get; set; } = null!;

    public string? RandomKey { get; set; }

    public string? FullName { get; set; }

    public string? Email { get; set; }

    public string? Phone { get; set; }

    public DateOnly? DateOfBirth { get; set; }

    public int? AvatarId { get; set; }

    public virtual Avatar? Avatar { get; set; }

    public virtual ICollection<UserLession> UserLessions { get; set; } = new List<UserLession>();
}
