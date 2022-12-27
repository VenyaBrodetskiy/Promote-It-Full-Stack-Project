using System;
using System.Collections.Generic;

namespace SocialActivistAPI.Models;

public partial class TransactionState
{
    public int Id { get; set; }

    public string Title { get; set; } = null!;

    public virtual ICollection<Transaction> Transactions { get; } = new List<Transaction>();
}
