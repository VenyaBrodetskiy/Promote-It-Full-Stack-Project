using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;

namespace dotNetBackend.Models;

public partial class MasaProjectDbContext : DbContext
{
    public MasaProjectDbContext()
    {
    }

    public MasaProjectDbContext(DbContextOptions<MasaProjectDbContext> options)
        : base(options)
    {
    }

    public virtual DbSet<BusinessOwner> BusinessOwners { get; set; }

    public virtual DbSet<Campaign> Campaigns { get; set; }

    public virtual DbSet<NonProfitOrganization> NonProfitOrganizations { get; set; }

    public virtual DbSet<Product> Products { get; set; }

    public virtual DbSet<ProductToCampaignQty> ProductToCampaignQties { get; set; }

    public virtual DbSet<SocialActivist> SocialActivists { get; set; }

    public virtual DbSet<Status> Statuses { get; set; }

    public virtual DbSet<Transaction> Transactions { get; set; }

    public virtual DbSet<TransactionState> TransactionStates { get; set; }

    public virtual DbSet<User> Users { get; set; }

    public virtual DbSet<UserToCampaignBalance> UserToCampaignBalances { get; set; }

    public virtual DbSet<UserType> UserTypes { get; set; }

    // I pass connection string from Program.cs and constructor
    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        => optionsBuilder.LogTo(Console.WriteLine, Microsoft.Extensions.Logging.LogLevel.Error);
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<BusinessOwner>(entity =>
        {
            entity.HasKey(e => e.UserId);

            entity.ToTable("business_owner");

            entity.HasIndex(e => e.UserId, "Unique_business_owner").IsUnique();

            entity.Property(e => e.Email)
                .HasMaxLength(50)
                .HasColumnName("email");
            entity.Property(e => e.CreateDate)
                .HasColumnType("datetime")
                .HasColumnName("create_date");
            entity.Property(e => e.CreateUserId).HasColumnName("create_user_id");
            entity.Property(e => e.Name)
                .HasMaxLength(50)
                .HasColumnName("name");
            entity.Property(e => e.StatusId).HasColumnName("status_id");
            entity.Property(e => e.TwitterHandle)
                .HasMaxLength(100)
                .HasColumnName("twitter_handle");
            entity.Property(e => e.UpdateDate)
                .HasColumnType("datetime")
                .HasColumnName("update_date");
            entity.Property(e => e.UpdateUserId).HasColumnName("update_user_id");
            entity.Property(e => e.UserId).HasColumnName("user_id");

            entity.HasOne(d => d.CreateUser).WithMany(p => p.BusinessOwnerCreateUsers)
                .HasForeignKey(d => d.CreateUserId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_business_owner_create_user");

            entity.HasOne(d => d.Status).WithMany(p => p.BusinessOwners)
                .HasForeignKey(d => d.StatusId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_business_owner_status");

            entity.HasOne(d => d.UpdateUser).WithMany(p => p.BusinessOwnerUpdateUsers)
                .HasForeignKey(d => d.UpdateUserId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_business_owner_update_user");

            entity.HasOne(d => d.User).WithOne(p => p.BusinessOwnerUser)
                .HasForeignKey<BusinessOwner>(d => d.UserId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_business_owner_user");
        });

        modelBuilder.Entity<Campaign>(entity =>
        {
            entity.ToTable("campaign");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.CreateDate)
                .HasColumnType("datetime")
                .HasColumnName("create_date");
            entity.Property(e => e.CreateUserId).HasColumnName("create_user_id");
            entity.Property(e => e.Hashtag)
                .HasMaxLength(100)
                .HasColumnName("hashtag");
            entity.Property(e => e.LandingPage)
                .HasMaxLength(250)
                .HasColumnName("landing_page");
            entity.Property(e => e.StatusId).HasColumnName("status_id");
            entity.Property(e => e.UpdateDate)
                .HasColumnType("datetime")
                .HasColumnName("update_date");
            entity.Property(e => e.UpdateUserId).HasColumnName("update_user_id");
            entity.Property(e => e.UserId).HasColumnName("user_id");

            entity.HasOne(d => d.CreateUser).WithMany(p => p.CampaignCreateUsers)
                .HasForeignKey(d => d.CreateUserId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_campaign_create_user");

            entity.HasOne(d => d.Status).WithMany(p => p.Campaigns)
                .HasForeignKey(d => d.StatusId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_campaign_status");

            entity.HasOne(d => d.UpdateUser).WithMany(p => p.CampaignUpdateUsers)
                .HasForeignKey(d => d.UpdateUserId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_campaign_update_user");

            entity.HasOne(d => d.User).WithMany(p => p.CampaignUsers)
                .HasForeignKey(d => d.UserId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_campaign_user");
        });

        modelBuilder.Entity<NonProfitOrganization>(entity =>
        {
            entity.HasKey(e => e.UserId);

            entity.ToTable("non_profit_organization");

            entity.HasIndex(e => e.UserId, "Unique_non_profit_organization").IsUnique();

            entity.Property(e => e.Email)
                .HasMaxLength(50)
                .HasColumnName("email");
            entity.Property(e => e.CreateDate)
                .HasColumnType("datetime")
                .HasColumnName("create_date");
            entity.Property(e => e.CreateUserId).HasColumnName("create_user_id");
            entity.Property(e => e.Name)
                .HasMaxLength(50)
                .HasColumnName("name");
            entity.Property(e => e.StatusId).HasColumnName("status_id");
            entity.Property(e => e.UpdateDate)
                .HasColumnType("datetime")
                .HasColumnName("update_date");
            entity.Property(e => e.UpdateUserId).HasColumnName("update_user_id");
            entity.Property(e => e.UserId).HasColumnName("user_id");
            entity.Property(e => e.Website)
                .HasMaxLength(250)
                .HasColumnName("website");

            entity.HasOne(d => d.CreateUser).WithMany(p => p.NonProfitOrganizationCreateUsers)
                .HasForeignKey(d => d.CreateUserId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_non_profit_organization_create_user");

            entity.HasOne(d => d.Status).WithMany(p => p.NonProfitOrganizations)
                .HasForeignKey(d => d.StatusId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_non_profit_organization_status");

            entity.HasOne(d => d.UpdateUser).WithMany(p => p.NonProfitOrganizationUpdateUsers)
                .HasForeignKey(d => d.UpdateUserId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_non_profit_organization_update_user");

            entity.HasOne(d => d.User).WithOne(p => p.NonProfitOrganizationUser)
                .HasForeignKey<NonProfitOrganization>(d => d.UserId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_non_profit_organization_user");
        });

        modelBuilder.Entity<Product>(entity =>
        {
            entity.ToTable("product");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.CreateDate)
                .HasColumnType("datetime")
                .HasColumnName("create_date");
            entity.Property(e => e.CreateUserId).HasColumnName("create_user_id");
            entity.Property(e => e.Price).HasColumnName("price");
            entity.Property(e => e.StatusId).HasColumnName("status_id");
            entity.Property(e => e.Title)
                .HasMaxLength(50)
                .HasColumnName("title");
            entity.Property(e => e.UpdateDate)
                .HasColumnType("datetime")
                .HasColumnName("update_date");
            entity.Property(e => e.UpdateUserId).HasColumnName("update_user_id");
            entity.Property(e => e.UserId).HasColumnName("user_id");

            entity.HasOne(d => d.CreateUser).WithMany(p => p.ProductCreateUsers)
                .HasForeignKey(d => d.CreateUserId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_product_create_user");

            entity.HasOne(d => d.Status).WithMany(p => p.Products)
                .HasForeignKey(d => d.StatusId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_product_status");

            entity.HasOne(d => d.UpdateUser).WithMany(p => p.ProductUpdateUsers)
                .HasForeignKey(d => d.UpdateUserId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_product_update_user");

            entity.HasOne(d => d.User).WithMany(p => p.ProductUsers)
                .HasForeignKey(d => d.UserId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_product_user");
        });

        modelBuilder.Entity<ProductToCampaignQty>(entity =>
        {
            entity.ToTable("product_to_campaign_qty");

            entity.HasIndex(e => new { e.ProductId, e.CampaignId }, "Unique_product_to_campaign_qty").IsUnique();

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.CampaignId).HasColumnName("campaign_id");
            entity.Property(e => e.CreateDate)
                .HasColumnType("datetime")
                .HasColumnName("create_date");
            entity.Property(e => e.CreateUserId).HasColumnName("create_user_id");
            entity.Property(e => e.ProductId).HasColumnName("product_id");
            entity.Property(e => e.ProductQty).HasColumnName("product_qty");
            entity.Property(e => e.StatusId).HasColumnName("status_id");
            entity.Property(e => e.UpdateDate)
                .HasColumnType("datetime")
                .HasColumnName("update_date");
            entity.Property(e => e.UpdateUserId).HasColumnName("update_user_id");

            entity.HasOne(d => d.Campaign).WithMany(p => p.ProductToCampaignQties)
                .HasForeignKey(d => d.CampaignId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_product_to_campaign_qty_campaign");

            entity.HasOne(d => d.CreateUser).WithMany(p => p.ProductToCampaignQtyCreateUsers)
                .HasForeignKey(d => d.CreateUserId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_product_to_campaign_qty_create_user");

            entity.HasOne(d => d.Product).WithMany(p => p.ProductToCampaignQties)
                .HasForeignKey(d => d.ProductId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_product_to_campaign_qty_product");

            entity.HasOne(d => d.Status).WithMany(p => p.ProductToCampaignQties)
                .HasForeignKey(d => d.StatusId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_product_to_campaign_qty_status");

            entity.HasOne(d => d.UpdateUser).WithMany(p => p.ProductToCampaignQtyUpdateUsers)
                .HasForeignKey(d => d.UpdateUserId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_product_to_campaign_qty_update_user");
        });

        modelBuilder.Entity<SocialActivist>(entity =>
        {
            entity.HasKey(e => e.UserId);

            entity.ToTable("social_activist");

            entity.HasIndex(e => e.UserId, "Unique_social_activist").IsUnique();

            entity.Property(e => e.Email)
                .HasMaxLength(50)
                .HasColumnName("email");
            entity.Property(e => e.Address)
                .HasMaxLength(250)
                .HasColumnName("address");
            entity.Property(e => e.CreateDate)
                .HasColumnType("datetime")
                .HasColumnName("create_date");
            entity.Property(e => e.CreateUserId).HasColumnName("create_user_id");
            entity.Property(e => e.PhoneNumber)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("phone_number");
            entity.Property(e => e.StatusId).HasColumnName("status_id");
            entity.Property(e => e.TwitterHandle)
                .HasMaxLength(100)
                .HasColumnName("twitter_handle");
            entity.Property(e => e.UpdateDate)
                .HasColumnType("datetime")
                .HasColumnName("update_date");
            entity.Property(e => e.UpdateUserId).HasColumnName("update_user_id");
            entity.Property(e => e.UserId).HasColumnName("user_id");

            entity.HasOne(d => d.CreateUser).WithMany(p => p.SocialActivistCreateUsers)
                .HasForeignKey(d => d.CreateUserId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_social_activist_create_user");

            entity.HasOne(d => d.Status).WithMany(p => p.SocialActivists)
                .HasForeignKey(d => d.StatusId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_social_activist_status");

            entity.HasOne(d => d.UpdateUser).WithMany(p => p.SocialActivistUpdateUsers)
                .HasForeignKey(d => d.UpdateUserId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_social_activist_update_user");

            entity.HasOne(d => d.User).WithOne(p => p.SocialActivistUser)
                .HasForeignKey<SocialActivist>(d => d.UserId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_social_activist_user");
        });

        modelBuilder.Entity<Status>(entity =>
        {
            entity.ToTable("status");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Title)
                .HasMaxLength(50)
                .HasColumnName("title");
        });

        modelBuilder.Entity<Transaction>(entity =>
        {
            entity.ToTable("transaction");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.CreateDate)
                .HasColumnType("datetime")
                .HasColumnName("create_date");
            entity.Property(e => e.CreateUserId).HasColumnName("create_user_id");
            entity.Property(e => e.ProductId).HasColumnName("product_id");
            entity.Property(e => e.CampaignId).HasColumnName("campaign_id");
            entity.Property(e => e.StateId).HasColumnName("state_id");
            entity.Property(e => e.StatusId).HasColumnName("status_id");
            entity.Property(e => e.UpdateDate)
                .HasColumnType("datetime")
                .HasColumnName("update_date");
            entity.Property(e => e.UpdateUserId).HasColumnName("update_user_id");
            entity.Property(e => e.UserId).HasColumnName("user_id");

            entity.HasOne(d => d.CreateUser).WithMany(p => p.TransactionCreateUsers)
                .HasForeignKey(d => d.CreateUserId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_transaction_create_user");

            entity.HasOne(d => d.Product).WithMany(p => p.Transactions)
                .HasForeignKey(d => d.ProductId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_transaction_product");

            entity.HasOne(d => d.Campaign).WithMany(p => p.Transactions)
               .HasForeignKey(d => d.CampaignId)
               .OnDelete(DeleteBehavior.ClientSetNull)
               .HasConstraintName("FK_transaction_campaign");

            entity.HasOne(d => d.State).WithMany(p => p.Transactions)
                .HasForeignKey(d => d.StateId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_transaction_transaction_state");

            entity.HasOne(d => d.Status).WithMany(p => p.Transactions)
                .HasForeignKey(d => d.StatusId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_transaction_status");

            entity.HasOne(d => d.UpdateUser).WithMany(p => p.TransactionUpdateUsers)
                .HasForeignKey(d => d.UpdateUserId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_transaction_update_user");

            entity.HasOne(d => d.User).WithMany(p => p.TransactionUsers)
                .HasForeignKey(d => d.UserId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_transaction_user");
        });

        modelBuilder.Entity<TransactionState>(entity =>
        {
            entity.ToTable("transaction_state");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Title)
                .HasMaxLength(50)
                .HasColumnName("title");
        });

        modelBuilder.Entity<User>(entity =>
        {
            entity.ToTable("user");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.CreateDate)
                .HasColumnType("datetime")
                .HasColumnName("create_date");
            entity.Property(e => e.CreateUserId).HasColumnName("create_user_id");
            entity.Property(e => e.Login)
                .HasMaxLength(50)
                .HasColumnName("login");
            entity.Property(e => e.Password)
                .HasMaxLength(250)
                .IsUnicode(false)
                .HasColumnName("password");
            entity.Property(e => e.StatusId).HasColumnName("status_id");
            entity.Property(e => e.UpdateDate)
                .HasColumnType("datetime")
                .HasColumnName("update_date");
            entity.Property(e => e.UpdateUserId).HasColumnName("update_user_id");
            entity.Property(e => e.UserTypeId).HasColumnName("user_type_id");

            entity.HasOne(d => d.CreateUser).WithMany(p => p.InverseCreateUser)
                .HasForeignKey(d => d.CreateUserId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_user_create_user");

            entity.HasOne(d => d.Status).WithMany(p => p.Users)
                .HasForeignKey(d => d.StatusId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_user_status");

            entity.HasOne(d => d.UpdateUser).WithMany(p => p.InverseUpdateUser)
                .HasForeignKey(d => d.UpdateUserId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_user_update_user");

            entity.HasOne(d => d.UserType).WithMany(p => p.Users)
                .HasForeignKey(d => d.UserTypeId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_user_user_type");
        });

        modelBuilder.Entity<UserToCampaignBalance>(entity =>
        {
            entity.ToTable("user_to_campaign_balance");

            entity.HasIndex(e => new { e.UserId, e.CampaignId }, "Unique_balance_user_to_campaign").IsUnique();

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Balance).HasColumnName("balance");
            entity.Property(e => e.PreviousTweetCount).HasColumnName("previous_tweet_count");
            entity.Property(e => e.CampaignId).HasColumnName("campaign_id");
            entity.Property(e => e.CreateDate)
                .HasColumnType("datetime")
                .HasColumnName("create_date");
            entity.Property(e => e.CreateUserId).HasColumnName("create_user_id");
            entity.Property(e => e.StatusId).HasColumnName("status_id");
            entity.Property(e => e.UpdateDate)
                .HasColumnType("datetime")
                .HasColumnName("update_date");
            entity.Property(e => e.UpdateUserId).HasColumnName("update_user_id");
            entity.Property(e => e.UserId).HasColumnName("user_id");

            entity.HasOne(d => d.Campaign).WithMany(p => p.UserToCampaignBalances)
                .HasForeignKey(d => d.CampaignId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_balance_user_to_campaign_campaign");

            entity.HasOne(d => d.CreateUser).WithMany(p => p.UserToCampaignBalanceCreateUsers)
                .HasForeignKey(d => d.CreateUserId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_balance_user_to_campaign_create_user");

            entity.HasOne(d => d.Status).WithMany(p => p.UserToCampaignBalances)
                .HasForeignKey(d => d.StatusId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_balance_user_to_campaign_status");

            entity.HasOne(d => d.UpdateUser).WithMany(p => p.UserToCampaignBalanceUpdateUsers)
                .HasForeignKey(d => d.UpdateUserId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_balance_user_to_campaign_update_user");

            entity.HasOne(d => d.User).WithMany(p => p.UserToCampaignBalanceUsers)
                .HasForeignKey(d => d.UserId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_balance_user_to_campaign_user");
        });

        modelBuilder.Entity<UserType>(entity =>
        {
            entity.ToTable("user_type");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Title)
                .HasMaxLength(50)
                .HasColumnName("title");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
