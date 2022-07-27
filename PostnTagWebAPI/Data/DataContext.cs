using Microsoft.EntityFrameworkCore;
using PostnTagWebAPI.Models;

namespace PostnTagWebAPI.Data
{
    public class DataContext : DbContext
    {
        public DataContext(DbContextOptions<DataContext> options) : base(options)
        {

        }

        public DbSet<Post> Posts { get; set; }
        public DbSet<Tag> Tags { get; set; }
        public DbSet<PostTag> PostTags { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<PostTag>()
                .HasKey(pc => new { pc.PostId, pc.TagId });

            modelBuilder.Entity<PostTag>()
                .HasOne(p => p.Post)
                .WithMany(pc => pc.PostTags)
                .HasForeignKey(c => c.PostId);

            modelBuilder.Entity<PostTag>()
                .HasOne(p => p.Tag)
                .WithMany(pc => pc.PostTags)
                .HasForeignKey(c => c.TagId);
        }

    }
}
