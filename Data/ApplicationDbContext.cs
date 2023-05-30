using IdentityServer4.EntityFramework.Options;
using Microsoft.AspNetCore.ApiAuthorization.IdentityServer;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using RobeazyCore.Models;

namespace RobeazyCore.Data
{
    public partial class ApplicationDbContext : ApiAuthorizationDbContext<ApplicationUser>
    {

        public ApplicationDbContext(
            DbContextOptions options,
            IOptions<OperationalStoreOptions> operationalStoreOptions) : base(options, operationalStoreOptions)
        {
        }

        //public virtual DbSet<AspNetRole> AspNetRoles { get; set; }
        //public virtual DbSet<AspNetUserClaim> AspNetUserClaims { get; set; }
        //public virtual DbSet<AspNetUserLogin> AspNetUserLogins { get; set; }
        //public virtual DbSet<AspNetUserRole> AspNetUserRoles { get; set; }
        //public virtual DbSet<AspNetUser> AspNetUsers { get; set; }
        public virtual DbSet<ContributionLike> ContributionLikes { get; set; }
        public virtual DbSet<Contribution> Contributions { get; set; }
        public virtual DbSet<Feedback> Feedbacks { get; set; }
        public virtual DbSet<GuitarTabContribution> GuitarTabContributions { get; set; }
        public virtual DbSet<GuitarTab> GuitarTabs { get; set; }
        //public virtual DbSet<MigrationHistory> MigrationHistory { get; set; }
        public virtual DbSet<MovieScriptContribution> MovieScriptContributions { get; set; }
        public virtual DbSet<MovieScript> MovieScripts { get; set; }
        public virtual DbSet<Report> Reports { get; set; }
        public virtual DbSet<Story> Stories { get; set; }
        public virtual DbSet<StoryLike> StoryLikes { get; set; }
        public virtual DbSet<TextStory> TextStories { get; set; }
        public virtual DbSet<TextStoryContribution> TextStoryContributions { get; set; }

        //protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        //{
        //    if (!optionsBuilder.IsConfigured)
        //    {
        //        //#warning To protect potentially sensitive information in your connection string, you should move it out of source code. See http://go.microsoft.com/fwlink/?LinkId=723263 for guidance on storing connection strings.
        //        optionsBuilder.UseSqlServer("Data Source=CARTER-PC;Initial Catalog=robeazyLocalTest2;Integrated Security=True");
        //    }
        //}

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
        }

        //protected override void OnModelCreating(ModelBuilder modelBuilder)
        //{
            //    //modelBuilder.Entity<AspNetRoles>(entity =>
            //    //{
            //    //    entity.HasIndex(e => e.Name)
            //    //        .HasName("RoleNameIndex")
            //    //        .IsUnique();

            //    //    entity.Property(e => e.Id).HasMaxLength(128);

            //    //    entity.Property(e => e.Name)
            //    //        .IsRequired()
            //    //        .HasMaxLength(256);
            //    //});

            //    //modelBuilder.Entity<AspNetUserClaims>(entity =>
            //    //{
            //    //    entity.HasIndex(e => e.UserId)
            //    //        .HasName("IX_UserId");

            //    //    entity.Property(e => e.UserId)
            //    //        .IsRequired()
            //    //        .HasMaxLength(128);

            //    //    entity.HasOne(d => d.User)
            //    //        .WithMany(p => p.AspNetUserClaims)
            //    //        .HasForeignKey(d => d.UserId)
            //    //        .HasConstraintName("FK_dbo.AspNetUserClaims_dbo.AspNetUsers_UserId");
            //    //});

            //    //modelBuilder.Entity<AspNetUserLogins>(entity =>
            //    //{
            //    //    entity.HasKey(e => new { e.LoginProvider, e.ProviderKey, e.UserId })
            //    //        .HasName("PK_dbo.AspNetUserLogins");

            //    //    entity.HasIndex(e => e.UserId)
            //    //        .HasName("IX_UserId");

            //    //    entity.Property(e => e.LoginProvider).HasMaxLength(128);

            //    //    entity.Property(e => e.ProviderKey).HasMaxLength(128);

            //    //    entity.Property(e => e.UserId).HasMaxLength(128);

            //    //    entity.HasOne(d => d.User)
            //    //        .WithMany(p => p.AspNetUserLogins)
            //    //        .HasForeignKey(d => d.UserId)
            //    //        .HasConstraintName("FK_dbo.AspNetUserLogins_dbo.AspNetUsers_UserId");
            //    //});

            //    //modelBuilder.Entity<AspNetUserRoles>(entity =>
            //    //{
            //    //    entity.HasKey(e => new { e.UserId, e.RoleId })
            //    //        .HasName("PK_dbo.AspNetUserRoles");

            //    //    entity.HasIndex(e => e.RoleId)
            //    //        .HasName("IX_RoleId");

            //    //    entity.HasIndex(e => e.UserId)
            //    //        .HasName("IX_UserId");

            //    //    entity.Property(e => e.UserId).HasMaxLength(128);

            //    //    entity.Property(e => e.RoleId).HasMaxLength(128);

            //    //    entity.HasOne(d => d.Role)
            //    //        .WithMany(p => p.AspNetUserRoles)
            //    //        .HasForeignKey(d => d.RoleId)
            //    //        .HasConstraintName("FK_dbo.AspNetUserRoles_dbo.AspNetRoles_RoleId");

            //    //    entity.HasOne(d => d.User)
            //    //        .WithMany(p => p.AspNetUserRoles)
            //    //        .HasForeignKey(d => d.UserId)
            //    //        .HasConstraintName("FK_dbo.AspNetUserRoles_dbo.AspNetUsers_UserId");
            //    //});

            //    //modelBuilder.Entity<AspNetUsers>(entity =>
            //    //{
            //    //    entity.HasIndex(e => e.UserName)
            //    //        .HasName("UserNameIndex")
            //    //        .IsUnique();

            //    //    entity.Property(e => e.Id).HasMaxLength(128);

            //    //    entity.Property(e => e.Email).HasMaxLength(256);

            //    //    entity.Property(e => e.LockoutEndDateUtc).HasColumnType("datetime");

            //    //    entity.Property(e => e.UserName)
            //    //        .IsRequired()
            //    //        .HasMaxLength(256);
            //    //});

            //    modelBuilder.Entity<ContributionLike>(entity =>
            //    {
            //        entity.HasKey(e => e.Id)
            //            .HasName("PK_dbo.ContributionLikes");

            //        entity.HasIndex(e => e.ContributionId)
            //            .HasName("IX_ContributionId");

            //        entity.HasIndex(e => e.UserId)
            //            .HasName("IX_UserId");

            //        entity.Property(e => e.ContributionId).HasColumnName("ContributionId");

            //        entity.Property(e => e.CreateDate).HasColumnType("datetime");

            //        entity.Property(e => e.UserId)
            //            .IsRequired()
            //            .HasColumnName("UserId")
            //            .HasMaxLength(128);

            //        entity.HasOne(d => d.Contribution)
            //            .WithMany(p => p.ContributionLikes)
            //            .HasForeignKey(d => d.ContributionId)
            //            .HasConstraintName("FK_dbo.ContributionLikes_dbo.Contributions_ContributionId");

            //        entity.HasOne(d => d.User)
            //            .WithMany(p => p.ContributionLikes)
            //            .HasForeignKey(d => d.UserId)
            //            .OnDelete(DeleteBehavior.ClientSetNull)
            //            .HasConstraintName("FK_dbo.ContributionLikes_dbo.AspNetUsers_UserId");
            //    });

            //    modelBuilder.Entity<Contribution>(entity =>
            //    {
            //        entity.HasKey(e => e.Id)
            //            .HasName("PK_dbo.Contributions");

            //        entity.HasIndex(e => e.ContributorId)
            //            .HasName("IX_ContributorId");

            //        entity.HasIndex(e => e.StoryId)
            //            .HasName("IX_StoryId");

            //        entity.Property(e => e.ContributorId)
            //            .IsRequired()
            //            .HasColumnName("ContributorId")
            //            .HasMaxLength(128);

            //        entity.Property(e => e.CreateDate)
            //        .IsRequired()
            //        .HasColumnType("datetime");

            //        entity.Property(e => e.StoryId)
            //        .IsRequired()
            //        .HasColumnName("StoryId");

            //        entity.HasOne(d => d.Contributor)
            //            .WithMany(p => p.Contributions)
            //            .HasForeignKey(d => d.ContributorId)
            //            .OnDelete(DeleteBehavior.ClientSetNull)
            //            .HasConstraintName("FK_dbo.Contributions_dbo.AspNetUsers_ContributorId");

            //        entity.HasOne(d => d.Story)
            //            .WithMany(p => p.Contributions)
            //            .HasForeignKey(d => d.StoryId)
            //            .OnDelete(DeleteBehavior.ClientSetNull)
            //            .HasConstraintName("FK_dbo.Contributions_dbo.Stories_StoryId");
            //    });

            //    modelBuilder.Entity<Feedback>(entity =>
            //    {
            //        entity.HasKey(e => e.Id)
            //            .HasName("PK_dbo.Feedbacks");

            //        entity.HasIndex(e => e.UserId)
            //            .HasName("IX_UserId");

            //        entity.Property(e => e.Content).IsRequired();

            //        entity.Property(e => e.CreateDate)
            //        .IsRequired()
            //        .HasColumnType("datetime");

            //        entity.Property(e => e.UserId)
            //            .IsRequired()
            //            .HasColumnName("UserId")
            //            .HasMaxLength(128);

            //        entity.HasOne(d => d.User)
            //            .WithMany(p => p.Feedbacks)
            //            .HasForeignKey(d => d.UserId)
            //            .OnDelete(DeleteBehavior.ClientSetNull)
            //            .HasConstraintName("FK_dbo.Feedbacks_dbo.AspNetUsers_UserId");
            //    });

            //    modelBuilder.Entity<GuitarTabContribution>(entity =>
            //    {
            //        entity.HasKey(e => e.Id)
            //            .HasName("PK_dbo.GuitarTabContributions");

            //        entity.HasIndex(e => e.ContributionId)
            //            .HasName("IX_ContributionId");

            //        entity.HasIndex(e => e.PreviousContributionId)
            //            .HasName("IX_PreviousContributionId");

            //        entity.Property(e => e.ContributionId)
            //        .IsRequired()
            //        .HasColumnName("ContributionId");

            //        entity.Property(e => e.GuitarOne).IsRequired();

            //        entity.Property(e => e.PreviousContributionId).HasColumnName("PreviousContributionId");

            //        entity.HasOne(d => d.Contribution)
            //            .WithMany(p => p.GuitarTabContributions)
            //            .HasForeignKey(d => d.ContributionId)
            //            .HasConstraintName("FK_dbo.GuitarTabContributions_dbo.Contributions_ContributionId");

            //        entity.HasOne(d => d.PreviousContribution)
            //            .WithMany(p => p.GuitarTabContributions)
            //            .HasForeignKey(d => d.PreviousContributionId)
            //            .HasConstraintName("FK_dbo.GuitarTabContributions_dbo.GuitarTabContributions_PreviousContributionId");
            //    });

            //    modelBuilder.Entity<GuitarTab>(entity =>
            //    {
            //        entity.HasKey(e => e.StoryId)
            //            .HasName("PK_dbo.GuitarTabs");

            //        entity.HasIndex(e => e.StoryId)
            //            .HasName("IX_StoryId");

            //        entity.Property(e => e.StoryId)
            //        .IsRequired()
            //        .HasColumnName("StoryId");

            //        entity.HasOne(d => d.Story)
            //            .WithMany(p => p.GuitarTabs)
            //            .HasForeignKey(d => d.StoryId)
            //            .HasConstraintName("FK_dbo.GuitarTabs_dbo.Stories_StoryId");
            //    });

            //    //modelBuilder.Entity<MigrationHistory>(entity =>
            //    //{
            //    //    entity.HasKey(e => new { e.MigrationId, e.ContextKey })
            //    //        .HasName("PK_dbo.__MigrationHistory");

            //    //    entity.ToTable("__MigrationHistory");

            //    //    entity.Property(e => e.MigrationId).HasMaxLength(150);

            //    //    entity.Property(e => e.ContextKey).HasMaxLength(300);

            //    //    entity.Property(e => e.Model).IsRequired();

            //    //    entity.Property(e => e.ProductVersion)
            //    //        .IsRequired()
            //    //        .HasMaxLength(32);
            //    //});

            //    modelBuilder.Entity<MovieScriptContribution>(entity =>
            //    {
            //        entity.HasKey(e => e.ContributionId)
            //            .HasName("PK_dbo.MovieScriptContributions");

            //        entity.HasIndex(e => e.ContributionId)
            //            .HasName("IX_ContributionId");

            //        entity.HasIndex(e => e.PreviousContributionId)
            //            .HasName("IX_PreviousContributionId");

            //        entity.Property(e => e.Content).IsRequired();


            //        entity.HasOne(d => d.Contribution)
            //            .WithMany(p => p.MovieScriptContributions)
            //            .HasForeignKey(d => d.ContributionId)
            //            .HasConstraintName("FK_dbo.MovieScriptContributions_dbo.Contributions_Contribution_ContributionId");

            //        entity.HasOne(d => d.PreviousContribution)
            //            .WithMany(p => p.MovieScriptContributions)
            //            .HasForeignKey(d => d.PreviousContribution.ContributionId)
            //            .HasConstraintName("FK_dbo.MovieScriptContributions_dbo.Contributions_PreviousContribution_ContributionId");
            //    });

            //    modelBuilder.Entity<MovieScript>(entity =>
            //    {
            //        entity.HasKey(e => e.StoryId)
            //            .HasName("PK_dbo.MovieScripts");

            //        entity.HasIndex(e => e.Story.StoryId)
            //            .HasName("IX_Story_StoryId");

            //        entity.Property(e => e.Story.StoryId).HasColumnName("Story_StoryId");

            //        entity.HasOne(d => d.Story)
            //            .WithMany(p => p.MovieScripts)
            //            .HasForeignKey(d => d.Story.StoryId)
            //            .HasConstraintName("FK_dbo.MovieScripts_dbo.Stories_Story_StoryId");
            //    });

            //    modelBuilder.Entity<Report>(entity =>
            //    {
            //        entity.HasKey(e => e.ReportId)
            //            .HasName("PK_dbo.Reports");

            //        entity.HasIndex(e => e.Contribution.ContributionId)
            //            .HasName("IX_Contribution_ContributionId");

            //        entity.HasIndex(e => e.Story.StoryId)
            //            .HasName("IX_Story_StoryId");

            //        entity.HasIndex(e => e.User.Id)
            //            .HasName("IX_User_Id");

            //        entity.Property(e => e.Contribution.ContributionId).HasColumnName("Contribution_ContributionId");

            //        entity.Property(e => e.CreateDate).HasColumnType("datetime");

            //        entity.Property(e => e.Reason).IsRequired();

            //        entity.Property(e => e.Story.StoryId).HasColumnName("Story_StoryId");

            //        entity.Property(e => e.User.Id)
            //            .IsRequired()
            //            .HasColumnName("User_Id")
            //            .HasMaxLength(128);

            //        entity.HasOne(d => d.Contribution)
            //            .WithMany(p => p.Reports)
            //            .HasForeignKey(d => d.Contribution.ContributionId)
            //            .HasConstraintName("FK_dbo.Reports_dbo.Contributions_Contribution_ContributionId");

            //        entity.HasOne(d => d.Story)
            //            .WithMany(p => p.Reports)
            //            .HasForeignKey(d => d.Story.StoryId)
            //            .HasConstraintName("FK_dbo.Reports_dbo.Stories_Story_StoryId");

            //        entity.HasOne(d => d.User)
            //            .WithMany(p => p.Reports)
            //            .HasForeignKey(d => d.User.Id)
            //            .OnDelete(DeleteBehavior.ClientSetNull)
            //            .HasConstraintName("FK_dbo.Reports_dbo.AspNetUsers_User_Id");
            //    });

            //    modelBuilder.Entity<Story>(entity =>
            //    {
            //        entity.HasKey(e => e.StoryId)
            //            .HasName("PK_dbo.Stories");

            //        entity.HasIndex(e => e.AuthorId)
            //            .HasName("IX_Author_Id");

            //        entity.Property(e => e.AuthorId)
            //            .IsRequired()
            //            .HasColumnName("Author_Id")
            //            .HasMaxLength(128);

            //        entity.Property(e => e.CreateDate).HasColumnType("datetime");

            //        entity.Property(e => e.Title).IsRequired();

            //        entity.HasOne(d => d.Author)
            //            .WithMany(p => p.Stories)
            //            .HasForeignKey(d => d.AuthorId)
            //            .OnDelete(DeleteBehavior.ClientSetNull)
            //            .HasConstraintName("FK_dbo.Stories_dbo.AspNetUsers_Author_Id");
            //    });

            //    modelBuilder.Entity<StoryLike>(entity =>
            //    {
            //        entity.HasKey(e => e.LikeId)
            //            .HasName("PK_dbo.StoryLikes");

            //        entity.HasIndex(e => e.Story.StoryId)
            //            .HasName("IX_Story_StoryId");

            //        entity.HasIndex(e => e.User.Id)
            //            .HasName("IX_User_Id");

            //        entity.Property(e => e.CreateDate).HasColumnType("datetime");

            //        entity.Property(e => e.Story.StoryId).HasColumnName("Story_StoryId");

            //        entity.Property(e => e.User.Id)
            //            .IsRequired()
            //            .HasColumnName("User_Id")
            //            .HasMaxLength(128);

            //        entity.HasOne(d => d.Story)
            //            .WithMany(p => p.StoryLikes)
            //            .HasForeignKey(d => d.Story.StoryId)
            //            .HasConstraintName("FK_dbo.StoryLikes_dbo.Stories_Story_StoryId");

            //        entity.HasOne(d => d.User)
            //            .WithMany(p => p.StoryLikes)
            //            .HasForeignKey(d => d.User.Id)
            //            .OnDelete(DeleteBehavior.ClientSetNull)
            //            .HasConstraintName("FK_dbo.StoryLikes_dbo.AspNetUsers_User_Id");
            //    });

            //    modelBuilder.Entity<TextStory>(entity =>
            //    {
            //        entity.HasKey(e => e.StoryId)
            //            .HasName("PK_dbo.TextStories");

            //        entity.HasIndex(e => e.Story.StoryId)
            //            .HasName("IX_Story_StoryId");

            //        entity.Property(e => e.Story.StoryId).HasColumnName("Story_StoryId");

            //        entity.HasOne(d => d.Story)
            //            .WithMany(p => p.TextStories)
            //            .HasForeignKey(d => d.Story.StoryId)
            //            .HasConstraintName("FK_dbo.TextStories_dbo.Stories_Story_StoryId");
            //    });

            //    modelBuilder.Entity<TextStoryContribution>(entity =>
            //    {
            //        entity.HasKey(e => e.ContributionId)
            //            .HasName("PK_dbo.TextStoryContributions");

            //        entity.HasIndex(e => e.Contribution.ContributionId)
            //            .HasName("IX_Contribution_ContributionId");

            //        entity.HasIndex(e => e.PreviousContribution.ContributionId)
            //            .HasName("IX_PreviousContribution_ContributionId");

            //        entity.Property(e => e.Content).IsRequired();

            //        entity.Property(e => e.Contribution.ContributionId).HasColumnName("Contribution_ContributionId");

            //        entity.Property(e => e.PreviousContribution.ContributionId).HasColumnName("PreviousContribution_ContributionId");

            //        entity.HasOne(d => d.Contribution)
            //            .WithMany(p => p.TextStoryContributions)
            //            .HasForeignKey(d => d.Contribution.ContributionId)
            //            .HasConstraintName("FK_dbo.TextStoryContributions_dbo.Contributions_Contribution_ContributionId");

            //        entity.HasOne(d => d.PreviousContribution)
            //            .WithMany(p => p.TextStoryContributions)
            //            .HasForeignKey(d => d.PreviousContribution.ContributionId)
            //            .HasConstraintName("FK_dbo.TextStoryContributions_dbo.TextStoryContributions_PreviousContribution_ContributionId");
            //    });

            //    OnModelCreatingPartial(modelBuilder);
            //}

            //partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
        //}
    }
}
