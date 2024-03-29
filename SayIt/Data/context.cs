using Microsoft.EntityFrameworkCore;
using SayIt.Models.Likes;
using SayIt.Models.Posts;
using SayIt.Models.Profile;
using SayIt.Models.Tables;

namespace SayIt.Data;

public class Context : DbContext
{
   public DbSet<Post> Posts { get; set; }
   
   public DbSet<User> Users { get; set; }

   public DbSet<Profile> Profiles { get; set; }
   
   public DbSet<Like> Likes { get; set; }

   public Context(DbContextOptions<Context> options) : base(options)
   {
   }

   protected override void OnModelCreating(ModelBuilder modelBuilder)
   {
      modelBuilder.Entity<User>()
         .HasMany(usr => usr.Posts)
         .WithOne(pst => pst.Author)
         .HasPrincipalKey(usr => usr.Username)
         .OnDelete(DeleteBehavior.NoAction);

      modelBuilder.Entity<User>()
         .HasOne(u => u.Extra)
         .WithOne(p => p.CorrespondingUser)
         .HasForeignKey<Profile>(p => p.UserId)
         .OnDelete(DeleteBehavior.Cascade);
      
      modelBuilder.Entity<Like>()
         .HasKey(l => new { l.UserId, l.PostId });

      modelBuilder.Entity<Like>()
         .HasOne(l => l.User)
         .WithMany(u => u.Likes)
         .HasForeignKey(l => l.UserId);

      modelBuilder.Entity<Like>()
         .HasOne(l => l.Post)
         .WithMany(p => p.Likes)
         .HasForeignKey(l => l.PostId);
      
      base.OnModelCreating(modelBuilder);
   }
}