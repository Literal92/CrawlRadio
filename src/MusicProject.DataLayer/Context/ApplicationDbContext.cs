using MusicProject.Common.GuardToolkit;
using MusicProject.Common.PersianToolkit;
using MusicProject.Entities.AuditableEntity;
using MusicProject.Entities.Identity;
using MusicProject.ViewModels.Identity.Settings;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System.Collections.Generic;
using System.Globalization;
using System.Threading.Tasks;
using System.Threading;
using System;
using System.Net.Mime;
using MusicProject.Entities;
using MusicProject.DataLayer.Mappings;
using MusicProject.Entities.Content;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.Extensions.DependencyInjection;
using MusicProject.Entities.Comment;


namespace MusicProject.DataLayer.Context
{
    /// <summary>
    /// More info: http://www.dotnettips.info/post/2577
    /// and http://www.dotnettips.info/post/2578
    /// plus http://www.dotnettips.info/post/2491
    /// </summary>
    public class ApplicationDbContext :
        IdentityDbContext<User, Role, int, UserClaim, UserRole, UserLogin, RoleClaim, UserToken>,
        IUnitOfWork
    {
        // we can't use constructor injection anymore, because we are using the `AddDbContextPool<>`
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options) { }

        #region BaseClass

        public virtual DbSet<AppLogItem> AppLogItems { get; set; }
        public virtual DbSet<AppSqlCache> AppSqlCache { get; set; }
        public virtual DbSet<AppDataProtectionKey> AppDataProtectionKeys { get; set; }

        public void AddRange<TEntity>(IEnumerable<TEntity> entities) where TEntity : class
        {
            Set<TEntity>().AddRange(entities);
        }

        public void ExecuteSqlCommand(string query)
        {
            Database.ExecuteSqlCommand(query);
        }

        public void ExecuteSqlCommand(string query, params object[] parameters)
        {
            Database.ExecuteSqlCommand(query, parameters);
        }

        public T GetShadowPropertyValue<T>(object entity, string propertyName) where T : IConvertible
        {
            var value = this.Entry(entity).Property(propertyName).CurrentValue;
            return value != null
                ? (T)Convert.ChangeType(value, typeof(T), CultureInfo.InvariantCulture)
                : default(T);
        }

        public object GetShadowPropertyValue(object entity, string propertyName)
        {
            return this.Entry(entity).Property(propertyName).CurrentValue;
        }

        public void MarkAsChanged<TEntity>(TEntity entity) where TEntity : class
        {
            Update(entity);
        }

        public void RemoveRange<TEntity>(IEnumerable<TEntity> entities) where TEntity : class
        {
            Set<TEntity>().RemoveRange(entities);
        }

        public override int SaveChanges(bool acceptAllChangesOnSuccess)
        {
            //var changedEntityNames = this.GetChangedEntityNames();
            ChangeTracker.DetectChanges();

            beforeSaveTriggers();

            ChangeTracker.AutoDetectChangesEnabled = false; // for performance reasons, to avoid calling DetectChanges() again.
            var result = base.SaveChanges(acceptAllChangesOnSuccess);
            ChangeTracker.AutoDetectChangesEnabled = true;
            return result;
        }

        public override int SaveChanges()
        {
            ChangeTracker.DetectChanges();

            beforeSaveTriggers();

            ChangeTracker.AutoDetectChangesEnabled = false; // for performance reasons, to avoid calling DetectChanges() again.
            var result = base.SaveChanges();
            ChangeTracker.AutoDetectChangesEnabled = true;
            return result;
        }

        public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = new CancellationToken())
        {
            ChangeTracker.DetectChanges();

            beforeSaveTriggers();

            ChangeTracker.AutoDetectChangesEnabled = false; // for performance reasons, to avoid calling DetectChanges() again.
            var result = base.SaveChangesAsync(cancellationToken);
            ChangeTracker.AutoDetectChangesEnabled = true;
            return result;
        }

        public override Task<int> SaveChangesAsync(bool acceptAllChangesOnSuccess, CancellationToken cancellationToken = new CancellationToken())
        {
            ChangeTracker.DetectChanges();

            beforeSaveTriggers();

            ChangeTracker.AutoDetectChangesEnabled = false; // for performance reasons, to avoid calling DetectChanges() again.
            var result = base.SaveChangesAsync(acceptAllChangesOnSuccess, cancellationToken);
            ChangeTracker.AutoDetectChangesEnabled = true;
            return result;
        }

        private void beforeSaveTriggers()
        {
            validateEntities();
            setShadowProperties();
            this.ApplyCorrectYeKe();
        }

        private void setShadowProperties()
        {
            // we can't use constructor injection anymore, because we are using the `AddDbContextPool<>`
            var httpContextAccessor = this.GetService<IHttpContextAccessor>();
            httpContextAccessor.CheckArgumentIsNull(nameof(httpContextAccessor));
            ChangeTracker.SetAuditableEntityPropertyValues(httpContextAccessor);
        }

        private void validateEntities()
        {
            var errors = this.GetValidationErrors();
            if (!string.IsNullOrWhiteSpace(errors))
            {
                // we can't use constructor injection anymore, because we are using the `AddDbContextPool<>`
                var loggerFactory = this.GetService<ILoggerFactory>();
                loggerFactory.CheckArgumentIsNull(nameof(loggerFactory));
                var logger = loggerFactory.CreateLogger<ApplicationDbContext>();
                logger.LogError(errors);
                throw new InvalidOperationException(errors);
            }
        }

        #endregion


        public virtual DbSet<ContactUs> ContactUs { set; get; }



        public DbSet<Tag> Tags { get; set; }
        public DbSet<Slider> Sliders { get; set; }
        public DbSet<ContentTag> ContentTags { get; set; }
        public DbSet<ContentListTag> ContentListTags { get; set; }
        public DbSet<CategoryTag> CategoryTags { get; set; }

        public DbSet<ContentList> ContentLists { get; set; }
        public DbSet<ContentSelected> ContentSelecteds { get; set; }



        public virtual DbSet<Content> Contents { get; set; }
        public virtual DbSet<ContentListFile> ContentListFiles { get; set; }
        public virtual DbSet<ContentRelated> ContentRelateds { get; set; }

        public virtual DbSet<Comment> Comments { get; set; }
        public virtual DbSet<ContentComment> ContentComments { get; set; }
        public virtual DbSet<CategoryComment> CategoryComments { get; set; }
        public virtual DbSet<TagComment> TagComments { get; set; }
        public virtual DbSet<PodcastComment> PodcastComments { get; set; }
        public virtual DbSet<ContentListComment> ContentListComments { get; set; }

        public virtual DbSet<ContentFile> ContentFiles { get; set; }
        public virtual DbSet<SessionRequest> SessionRequests { get; set; }
        public virtual DbSet<Order> Orders { get; set; }
        public virtual DbSet<Device> Devices { get; set; }


        protected override void OnModelCreating(ModelBuilder builder)
        {
            // it should be placed here, otherwise it will rewrite the following settings!
            base.OnModelCreating(builder);
            // we can't use constructor injection anymore, because we are using the `AddDbContextPool<>`
            var siteSettings = this.GetService<IOptionsSnapshot<SiteSettings>>();
            siteSettings.CheckArgumentIsNull(nameof(siteSettings));
            siteSettings.Value.CheckArgumentIsNull(nameof(siteSettings.Value));
            // Adds all of the ASP.NET Core Identity related mappings at once.
            builder.AddCustomIdentityMappings(siteSettings.Value);

            //// Custom application mappings
            //builder.Entity<Category1>(build =>
            //{
            //  build.Property(category => category.Name).HasMaxLength(450).IsRequired();
            //  build.Property(category => category.Title).IsRequired();
            //});

            //builder.Entity<Product>(build =>
            //{
            //  build.Property(product => product.Name).HasMaxLength(450).IsRequired();
            //  build.HasOne(product => product.Category)
            //               .WithMany(category => category.Products);
            //});

            builder.Entity<AppDataProtectionKey>(entity =>
            {
                entity.ToTable("AppDataProtectionKeys", "dbo");

            });


            builder.Entity<CustomUserToken>(entity =>
            {
                entity.ToTable("CustomUserTokens", "dbo");
                entity.HasOne(ut => ut.User)
            .WithMany(u => u.CustomUserTokens)
            .HasForeignKey(ut => ut.UserId);


            });

            builder.Entity<AppLogItem>(entity =>
             {
                 entity.ToTable("AppLogItems", "dbo");

             });

            builder.Entity<Tag>(entity =>
             {
                 entity.ToTable("Tags", "dbo");

             });

            builder.Entity<Device>(entity =>
            {
                entity.ToTable("Devices", "dbo");
                entity.HasOne(device => device.User)
              .WithMany(applicationUser => applicationUser.Devices);
            });

            #region Content

            builder.Entity<AppSetting>(entity =>
            {
                entity.ToTable("AppSettings", "dbo");
            });

            builder.Entity<SessionRequest>(entity =>
            {
                entity.ToTable("SessionRequests", "dbo");

            });

            builder.Entity<Order>(entity =>
            {
                entity.ToTable("Orders", "dbo");

            });


            builder.Entity<ContactUs>(entity =>
          {
              entity.ToTable("ContactUs", "dbo");

          });


            builder.Entity<Slider>(entity =>
            {
                entity.ToTable("Sliders", "dbo");
            });


     

            builder.Entity<Category>(entity =>
            {
                entity.ToTable("Categorys", "dbo");

                entity.Property(e => e.Pic).HasMaxLength(800);

                entity.Property(e => e.Title)
                    .IsRequired()
                    .HasMaxLength(350);

                entity.HasOne(d => d.Parent)
                    .WithMany(p => p.InverseParent)
                    .HasForeignKey(d => d.ParentId)
                    .HasConstraintName("FK_Categorys_Categorys");
            });

       

            builder.Entity<Content>(entity =>
            {
                entity.ToTable("Contents", "dbo");


                entity.Property(e => e.Lead).HasMaxLength(1500);
                entity.Property(e => e.SubTitle).HasMaxLength(500);
                entity.Property(e => e.HeadLine).HasMaxLength(500);
                entity.Property(e => e.SeoDescription).HasMaxLength(500);
                entity.Property(e => e.SeoKeyboard).HasMaxLength(500);
                entity.Property(e => e.SeoTitle).HasMaxLength(500);
                entity.Property(e => e.SeoUrl).HasMaxLength(500);

                entity.Property(e => e.Title)
                    .IsRequired()
                    .HasMaxLength(500);

                entity.HasOne(d => d.Category)
                    .WithMany(p => p.Contents)
                    .HasForeignKey(d => d.CategoryId)
                    .HasConstraintName("FK_Contents_Categorys");
            });

            builder.Entity<Comment>(entity =>
            {
                entity.ToTable("Comments", "dbo");

                entity.HasOne(d => d.Parent)
                    .WithMany(p => p.InverseParent)
                    .HasForeignKey(d => d.ParentId)
                    .HasConstraintName("FK_Comments_Comments");
            });

            builder.Entity<ContentComment>(entity =>
            {
                entity.ToTable("ContentComments", "dbo");
            });
            builder.Entity<PodcastComment>(entity =>
            {
                entity.ToTable("PodcastComments", "dbo");
            });
            builder.Entity<ContentListComment>(entity =>
            {
                entity.ToTable("ContentListComments", "dbo");
            });
            builder.Entity<CategoryComment>(entity =>
            {
                entity.ToTable("CategoryComments", "dbo");

            });
            builder.Entity<TagComment>(entity =>
            {
                entity.ToTable("TagComments", "dbo");

            });
            builder.Entity<ContentFile>(entity =>
            {
                entity.ToTable("ContentFiles", "dbo");
                entity.Property(e => e.Thumbnail).HasMaxLength(500);
                entity.HasOne(d => d.Content)
            .WithMany(p => p.ContentFiles)
            .HasForeignKey(d => d.ContentId)
            .HasConstraintName("FK_ContentFiles_Contents");
            });

            builder.Entity<ContentRelated>(entity =>
            {
                entity.ToTable("ContentRelateds", "dbo");

                entity.Property(e => e.Id).HasColumnName("id");

                entity.HasOne(d => d.Content)
            .WithMany(p => p.ContentRelatedsContent)
            .HasForeignKey(d => d.ContentId)
            .OnDelete(DeleteBehavior.ClientSetNull)
            .HasConstraintName("FK_ContentRelateds_Contents");

                entity.HasOne(d => d.Related)
            .WithMany(p => p.ContentRelatedsContentRelated)
            .HasForeignKey(d => d.RelatedId)
            .OnDelete(DeleteBehavior.ClientSetNull)
            .HasConstraintName("FK_ContentRelateds_Contents1");
            });

            builder.Entity<ContentTag>(entity =>
            {
                entity.ToTable("ContentTags", "dbo");
                entity.HasOne(d => d.Content)
            .WithMany(p => p.ContentTags)
            .HasForeignKey(d => d.ContentId)
            .HasConstraintName("FK_ContentTags_Contents");

                entity.HasOne(d => d.Tag)
            .WithMany(p => p.ContentTags)
            .HasForeignKey(d => d.TagId)
            .HasConstraintName("FK_ContentTags_Tags");
            });


            builder.Entity<ContentListTag>(entity =>
            {
                entity.ToTable("ContentListTags", "dbo");
                entity.HasOne(d => d.ContentList)
            .WithMany(p => p.ContentListTags)
            .HasForeignKey(d => d.ContentListId)
            .HasConstraintName("FK_ContentListTags_ContentLists");

                entity.HasOne(d => d.Tag)
            .WithMany(p => p.ContentListTags)
            .HasForeignKey(d => d.TagId)
            .HasConstraintName("FK_ContentListTags_Tags");
            });


            builder.Entity<CategoryTag>(entity =>
            {
                entity.ToTable("CategoryTags", "dbo");



                entity.HasOne(d => d.Category)
            .WithMany(p => p.CategoryTags)
            .HasForeignKey(d => d.CategoryId)
            .HasConstraintName("FK_CategoryTags_Categories");

                entity.HasOne(d => d.Tag)
            .WithMany(p => p.CategoryTags)
            .HasForeignKey(d => d.TagId)
            .HasConstraintName("FK_CategoryTags_Tags");
            });


            builder.Entity<PodcastTag>(entity =>
            {
                entity.ToTable("PodcastTags", "dbo");
                entity.HasOne(d => d.Podcast)
            .WithMany(p => p.PodcastTags)
            .HasForeignKey(d => d.PodcastId)
            .HasConstraintName("FK_PodcastTags_Podcasts");
                entity.HasOne(d => d.Tag)
            .WithMany(p => p.PodcastTags)
            .HasForeignKey(d => d.TagId)
            .HasConstraintName("FK_PodcastTags_Tags");
            });


            builder.Entity<Podcast>(entity =>
            {
                entity.ToTable("Podcasts", "dbo");

            });


            builder.Entity<ContentList>(entity =>
            {
                entity.ToTable("ContentLists", "dbo");

            });

            builder.Entity<ContentSelected>(entity =>
          {
              entity.ToTable("ContentSelecteds", "dbo");



              entity.HasOne(d => d.Content)
          .WithMany(p => p.ContentSelecteds)
          .HasForeignKey(d => d.ContentId)
          .HasConstraintName("FK_ContentSelecteds_Contents");

              entity.HasOne(d => d.ContentList)
          .WithMany(p => p.ContentSelecteds)
          .HasForeignKey(d => d.ContentListId)
          .HasConstraintName("FK_ContentSelecteds_ContentLists");
          });

            #endregion



            // This should be placed here, at the end.
            builder.AddAuditableShadowProperties();
        }
    }
}
