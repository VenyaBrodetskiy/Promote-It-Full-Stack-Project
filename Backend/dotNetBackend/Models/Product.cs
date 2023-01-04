using System;
using System.Collections.Generic;

namespace SocialActivistAPI.Models;

public partial class Product
{
    public int Id { get; set; }

    public string Title { get; set; } = null!;

    public int Price { get; set; }

    public int UserId { get; set; }

    public DateTime CreateDate { get; set; }

    public DateTime UpdateDate { get; set; }

    public int CreateUserId { get; set; }

    public int UpdateUserId { get; set; }

    public int StatusId { get; set; }

    public virtual User CreateUser { get; set; } = null!;

    public virtual ICollection<ProductToCampaignQty> ProductToCampaignQties { get; } = new List<ProductToCampaignQty>();

    public virtual Status Status { get; set; } = null!;

    public virtual ICollection<Transaction> Transactions { get; } = new List<Transaction>();

    public virtual User UpdateUser { get; set; } = null!;

    public virtual User User { get; set; } = null!;
}
