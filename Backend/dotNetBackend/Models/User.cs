using System;
using System.Collections.Generic;

namespace SocialActivistAPI.Models;

public partial class User
{
    public int Id { get; set; }

    public string Login { get; set; } = null!;

    public string Password { get; set; } = null!;

    public int UserTypeId { get; set; }

    public DateTime CreateDate { get; set; }

    public DateTime UpdateDate { get; set; }

    public int CreateUserId { get; set; }

    public int UpdateUserId { get; set; }

    public int StatusId { get; set; }

    public virtual ICollection<BusinessOwner> BusinessOwnerCreateUsers { get; } = new List<BusinessOwner>();

    public virtual ICollection<BusinessOwner> BusinessOwnerUpdateUsers { get; } = new List<BusinessOwner>();

    public virtual BusinessOwner? BusinessOwnerUser { get; set; }

    public virtual ICollection<Campaign> CampaignCreateUsers { get; } = new List<Campaign>();

    public virtual ICollection<Campaign> CampaignUpdateUsers { get; } = new List<Campaign>();

    public virtual ICollection<Campaign> CampaignUsers { get; } = new List<Campaign>();

    public virtual User CreateUser { get; set; } = null!;

    public virtual ICollection<User> InverseCreateUser { get; } = new List<User>();

    public virtual ICollection<User> InverseUpdateUser { get; } = new List<User>();

    public virtual ICollection<NonProfitOrganization> NonProfitOrganizationCreateUsers { get; } = new List<NonProfitOrganization>();

    public virtual ICollection<NonProfitOrganization> NonProfitOrganizationUpdateUsers { get; } = new List<NonProfitOrganization>();

    public virtual NonProfitOrganization? NonProfitOrganizationUser { get; set; }

    public virtual ICollection<Product> ProductCreateUsers { get; } = new List<Product>();

    public virtual ICollection<ProductToCampaignQty> ProductToCampaignQtyCreateUsers { get; } = new List<ProductToCampaignQty>();

    public virtual ICollection<ProductToCampaignQty> ProductToCampaignQtyUpdateUsers { get; } = new List<ProductToCampaignQty>();

    public virtual ICollection<Product> ProductUpdateUsers { get; } = new List<Product>();

    public virtual ICollection<Product> ProductUsers { get; } = new List<Product>();

    public virtual ICollection<SocialActivist> SocialActivistCreateUsers { get; } = new List<SocialActivist>();

    public virtual ICollection<SocialActivist> SocialActivistUpdateUsers { get; } = new List<SocialActivist>();

    public virtual SocialActivist? SocialActivistUser { get; set; }

    public virtual Status Status { get; set; } = null!;

    public virtual ICollection<Transaction> TransactionCreateUsers { get; } = new List<Transaction>();

    public virtual ICollection<Transaction> TransactionUpdateUsers { get; } = new List<Transaction>();

    public virtual ICollection<Transaction> TransactionUsers { get; } = new List<Transaction>();

    public virtual User UpdateUser { get; set; } = null!;

    public virtual ICollection<UserToCampaignBalance> UserToCampaignBalanceCreateUsers { get; } = new List<UserToCampaignBalance>();

    public virtual ICollection<UserToCampaignBalance> UserToCampaignBalanceUpdateUsers { get; } = new List<UserToCampaignBalance>();

    public virtual ICollection<UserToCampaignBalance> UserToCampaignBalanceUsers { get; } = new List<UserToCampaignBalance>();

    public virtual UserType UserType { get; set; } = null!;
}
