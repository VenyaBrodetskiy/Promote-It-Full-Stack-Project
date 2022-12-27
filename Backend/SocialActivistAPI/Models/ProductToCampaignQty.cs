using System;
using System.Collections.Generic;

namespace SocialActivistAPI.Models;

public partial class ProductToCampaignQty
{
    public int Id { get; set; }

    public int ProductId { get; set; }

    public int CampaignId { get; set; }

    public int ProductQty { get; set; }

    public DateTime CreateDate { get; set; }

    public DateTime UpdateDate { get; set; }

    public int CreateUserId { get; set; }

    public int UpdateUserId { get; set; }

    public int StatusId { get; set; }

    public virtual Campaign Campaign { get; set; } = null!;

    public virtual User CreateUser { get; set; } = null!;

    public virtual Product Product { get; set; } = null!;

    public virtual Status Status { get; set; } = null!;

    public virtual User UpdateUser { get; set; } = null!;
}
