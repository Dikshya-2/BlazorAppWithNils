using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace BlazorAppWithNils.Models;

public partial class TodoListContext : DbContext
{
    public TodoListContext()
    {
    }

    public TodoListContext(DbContextOptions<TodoListContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Cpr> Cprs { get; set; }

    public virtual DbSet<TodoList> TodoLists { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see https://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseSqlServer("Server=(localdb)\\mssqllocaldb;Database=TodoList;Trusted_Connection=True;MultipleActiveResultSets=true");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Cpr>(entity =>
        {
            entity.HasKey(e => e.Cprid).HasName("PK__Cpr__C0242C1E6B3A4F72");

            entity.ToTable("Cpr");

            entity.Property(e => e.CprNr).HasMaxLength(500);
            entity.Property(e => e.User).HasMaxLength(200);
        });

        modelBuilder.Entity<TodoList>(entity =>
        {
            entity.HasKey(e => e.TodoId).HasName("PK__TodoList__E5C578A1AD9DEF0F");

            entity.ToTable("TodoList");

            entity.Property(e => e.TodoId).HasColumnName("todoId");
            entity.Property(e => e.Item)
                .HasMaxLength(500)
                .HasColumnName("item");

            entity.HasOne(d => d.User).WithMany(p => p.TodoLists)
                .HasForeignKey(d => d.UserId)
                .HasConstraintName("FK__TodoList__UserId__267ABA7A");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
