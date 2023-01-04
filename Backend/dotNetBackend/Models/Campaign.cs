using System;
using System.Collections.Generic;

namespace dotNetBackend.Models;

public partial class Campaign
{
    public int Id { get; set; }

    public string Hashtag { get; set; } = null!;

    public string LandingPage { get; set; } = null!;

    public int UserId { get; set; }

    public DateTime CreateDate { get; set; }

    public DateTime UpdateDate { get; set; }

    public int CreateUserId { get; set; }

    public int UpdateUserId { get; set; }

    public int StatusId { get; set; }

    public virtual User CreateUser { get; set; } = null!;

    public virtual ICollection<ProductToCampaignQty> ProductToCampaignQties { get; } = new List<ProductToCampaignQty>();

    public virtual Status Status { get; set; } = null!;

    public virtual User UpdateUser { get; set; } = null!;

    public virtual User User { get; set; } = null!;

    public virtual ICollection<UserToCampaignBalance> UserToCampaignBalances { get; } = new List<UserToCampaignBalance>();

    public virtual ICollection<Transaction> Transactions { get; } = new List<Transaction>();
}
