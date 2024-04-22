using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace EnglishAPI.Data;

public partial class EnglishWebContext : DbContext
{
    public EnglishWebContext()
    {
    }

    public EnglishWebContext(DbContextOptions<EnglishWebContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Avatar> Avatars { get; set; }

    public virtual DbSet<Grammar> Grammars { get; set; }

    public virtual DbSet<Lession> Lessions { get; set; }

    public virtual DbSet<Reading> Readings { get; set; }

    public virtual DbSet<Sentence> Sentences { get; set; }

    public virtual DbSet<User> Users { get; set; }

    public virtual DbSet<UserLession> UserLessions { get; set; }

    public virtual DbSet<Vocabulary> Vocabularies { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see https://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseSqlServer("Server=MSI\\SQLEXPRESS; Database=EnglishWeb;Integrated Security=True;Encrypt=false");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Avatar>(entity =>
        {
            entity.ToTable("Avatar");

            entity.Property(e => e.AvatarId).HasColumnName("AvatarID");
            entity.Property(e => e.Description).HasMaxLength(255);
            entity.Property(e => e.Image).HasMaxLength(255);
        });

        modelBuilder.Entity<Grammar>(entity =>
        {
            entity.ToTable("Grammar");

            entity.Property(e => e.GrammarId).HasColumnName("GrammarID");
            entity.Property(e => e.Example)
                .HasMaxLength(10)
                .IsFixedLength();
            entity.Property(e => e.Formula).HasMaxLength(255);
            entity.Property(e => e.Note).HasMaxLength(255);
        });

        modelBuilder.Entity<Lession>(entity =>
        {
            entity.ToTable("Lession");

            entity.Property(e => e.LessionId).HasColumnName("LessionID");
            entity.Property(e => e.Description).HasMaxLength(255);
            entity.Property(e => e.Image).HasMaxLength(255);
            entity.Property(e => e.Title).HasMaxLength(255);
            entity.Property(e => e.Vietnamese).HasMaxLength(255);
        });

        modelBuilder.Entity<Reading>(entity =>
        {
            entity.HasKey(e => e.ReadId);

            entity.ToTable("Reading");

            entity.Property(e => e.ReadId).HasColumnName("ReadID");
            entity.Property(e => e.Answer).HasMaxLength(255);
            entity.Property(e => e.Answer2).HasMaxLength(255);
            entity.Property(e => e.LessionId).HasColumnName("LessionID");
            entity.Property(e => e.Question).HasMaxLength(255);
            entity.Property(e => e.Question2).HasMaxLength(255);

            entity.HasOne(d => d.Lession).WithMany(p => p.Readings)
                .HasForeignKey(d => d.LessionId)
                .HasConstraintName("FK_Reading_Lession");
        });

        modelBuilder.Entity<Sentence>(entity =>
        {
            entity.HasKey(e => e.SenId);

            entity.ToTable("Sentence");

            entity.Property(e => e.SenId).HasColumnName("SenID");
            entity.Property(e => e.BlankSentence).HasMaxLength(255);
            entity.Property(e => e.FillWord).HasMaxLength(255);
            entity.Property(e => e.Hint).HasMaxLength(255);
            entity.Property(e => e.LessionId).HasColumnName("LessionID");
            entity.Property(e => e.Vietnamese).HasMaxLength(255);

            entity.HasOne(d => d.Lession).WithMany(p => p.Sentences)
                .HasForeignKey(d => d.LessionId)
                .HasConstraintName("FK_Sentence_Lession");
        });

        modelBuilder.Entity<User>(entity =>
        {
            entity.ToTable("User");

            entity.Property(e => e.UserId).HasColumnName("UserID");
            entity.Property(e => e.AvatarId).HasColumnName("AvatarID");
            entity.Property(e => e.Email)
                .HasMaxLength(255)
                .IsUnicode(false);
            entity.Property(e => e.FullName).HasMaxLength(255);
            entity.Property(e => e.Password)
                .HasMaxLength(255)
                .IsUnicode(false);
            entity.Property(e => e.Phone).HasMaxLength(12);
            entity.Property(e => e.RandomKey)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.UserName)
                .HasMaxLength(255)
                .IsUnicode(false);

            entity.HasOne(d => d.Avatar).WithMany(p => p.Users)
                .HasForeignKey(d => d.AvatarId)
                .HasConstraintName("FK_User_Avatar");
        });

        modelBuilder.Entity<UserLession>(entity =>
        {
            entity.HasKey(e => new { e.UserId, e.LessionId });

            entity.ToTable("UserLession");

            entity.Property(e => e.UserId).HasColumnName("UserID");
            entity.Property(e => e.LessionId).HasColumnName("LessionID");
            entity.Property(e => e.Comment).HasMaxLength(255);
            entity.Property(e => e.Status).HasMaxLength(12);

            entity.HasOne(d => d.Lession).WithMany(p => p.UserLessions)
                .HasForeignKey(d => d.LessionId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_UserLession_Lession");

            entity.HasOne(d => d.User).WithMany(p => p.UserLessions)
                .HasForeignKey(d => d.UserId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_UserLession_User");
        });

        modelBuilder.Entity<Vocabulary>(entity =>
        {
            entity.HasKey(e => e.VocabId);

            entity.ToTable("Vocabulary");

            entity.Property(e => e.VocabId).HasColumnName("VocabID");
            entity.Property(e => e.Image).HasMaxLength(255);
            entity.Property(e => e.LessionId).HasColumnName("LessionID");
            entity.Property(e => e.Vietnamese).HasMaxLength(255);
            entity.Property(e => e.Vocab).HasMaxLength(255);
            entity.Property(e => e.WordClass).HasMaxLength(50);

            entity.HasOne(d => d.Lession).WithMany(p => p.Vocabularies)
                .HasForeignKey(d => d.LessionId)
                .HasConstraintName("FK_Vocabulary_Lession");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
