namespace Win_Dev.Data
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("Personel")]
    public partial class Person
    {
        [Key]
        public Guid PersonID { get; set; }

        [StringLength(30)]
        public string FirstName { get; set; }

        [StringLength(30)]
        public string SurName { get; set; }

        [StringLength(30)]
        public string LastName { get; set; }

        [StringLength(50)]
        public string Division { get; set; }

        [StringLength(30)]
        public string Occupation { get; set; }

        public virtual ICollection<Project> Projects { get; set; }
        public virtual ICollection<Goal> Goals { get; set; }

        public Person()
        {
            Projects = new List<Project>();
            Goals = new List<Goal>();
        }
    }
}
