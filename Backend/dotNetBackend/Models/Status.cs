using System;
using System.Collections.Generic;

namespace dotNetBackend.Models;

public partial class Status
{
    public int Id { get; set; }

    public string Title { get; set; } = null!;

    public virtual ICollection<BusinessOwner> BusinessOwners { get; } = new List<BusinessOwner>();

    public virtual ICollection<Campaign> Campaigns { get; } = new List<Campaign>();

    public virtual ICollection<NonProfitOrganization> NonProfitOrganizations { get; } = new List<NonProfitOrganization>();

    public virtual ICollection<ProductToCampaignQty> ProductToCampaignQties { get; } = new List<ProductToCampaignQty>();

    public virtual ICollection<Product> Products { get; } = new List<Product>();

    public virtual ICollection<SocialActivist> SocialActivists { get; } = new List<SocialActivist>();

    public virtual ICollection<Transaction> Transactions { get; } = new List<Transaction>();

    public virtual ICollection<UserToCampaignBalance> UserToCampaignBalances { get; } = new List<UserToCampaignBalance>();

    public virtual ICollection<User> Users { get; } = new List<User>();
}
