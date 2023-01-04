using System;
using System.Collections.Generic;

namespace SocialActivistAPI.Models;

public partial class UserType
{
    public int Id { get; set; }

    public string Title { get; set; } = null!;

    public virtual ICollection<User> Users { get; } = new List<User>();
}
