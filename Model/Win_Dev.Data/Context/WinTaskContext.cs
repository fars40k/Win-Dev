namespace Win_Dev.Data
{
    using System;
    using System.Data.Entity;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Linq;

    public partial class WinTaskContext : DbContext
    {
        public WinTaskContext()
            : base("WinTaskManager")
        {
        }

        public virtual DbSet<Goal> Goals { get; set; }
        public virtual DbSet<Person> Personel { get; set; }
        public virtual DbSet<Project> Projects { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Person>()
                .HasMany(s => s.Projects)
                .WithMany(c => c.Personel)
                .Map(cs =>
                {
                    cs.ToTable("PersonelToProjects");
                    cs.MapLeftKey("PersonID");
                    cs.MapRightKey("ProjectID");
            
                });

            modelBuilder.Entity<Person>()
              .HasMany(s => s.Goals)
              .WithMany(c => c.Personel)
              .Map(cs =>
              {
                  cs.ToTable("PersonelToGoals");
                  cs.MapLeftKey("PersonID");
                  cs.MapRightKey("GoalID");
                
              });

            modelBuilder.Entity<Project>()
              .HasMany(g => g.Goals)
              .WithMany(p => p.Projects)
              .Map(cs =>
              {
                  cs.ToTable("GoalsToProjects");
                  cs.MapLeftKey("GoalID");
                  cs.MapRightKey("ProjectID");
                  
              });


            modelBuilder.Entity<Goal>()
                .Property(e => e.Name)
                .IsFixedLength();

            modelBuilder.Entity<Goal>()
                .Property(e => e.Description)
                .IsUnicode(false);

            modelBuilder.Entity<Person>()
                .Property(e => e.FirstName)
                .IsFixedLength();

            modelBuilder.Entity<Person>()
                .Property(e => e.SurName)
                .IsFixedLength();

            modelBuilder.Entity<Person>()
                .Property(e => e.LastName)
                .IsFixedLength();

            modelBuilder.Entity<Person>()
                .Property(e => e.Division)
                .IsFixedLength();

            modelBuilder.Entity<Person>()
                .Property(e => e.Occupation)
                .IsFixedLength();

            modelBuilder.Entity<Project>()
                .Property(e => e.Name)
                .IsFixedLength();

            modelBuilder.Entity<Project>()
                .Property(e => e.Description)
                .IsUnicode(false);
        }


    }
        
}
