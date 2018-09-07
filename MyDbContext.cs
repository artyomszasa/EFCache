using System;
using EFCache.Data;
using Microsoft.EntityFrameworkCore;

namespace EFCache
{
    public class MyDbContext : DbContext
    {
        [DbFunction("costy_operation")]
        public static int CostyOperation(string text) => throw new InvalidOperationException();

        public MyDbContext(DbContextOptions<MyDbContext> options) : base(options) { }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.Entity<Root>(b =>
            {
                b.HasKey(e => e.Id);
                b.Property(e => e.Text).HasMaxLength(320).IsRequired(true).IsUnicode(true);
            });
            builder.Entity<Word>(b =>
            {
                b.HasKey(e => e.Id);
                b.Property(e => e.Data).HasMaxLength(2000).IsRequired(true).IsUnicode(false);
                b.HasOne(e => e.Root).WithMany(e => e.Words).HasForeignKey(e => e.RootId).OnDelete(DeleteBehavior.Cascade);
            });
        }
    }
}