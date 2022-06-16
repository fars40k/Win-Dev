namespace Win_Dev.Data
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class Project
    {
        public Guid ProjectID { get; set; }

        [Required]
        [StringLength(50)]
        public string Name { get; set; }

        [Required]
        [StringLength(4000)]
        public string Description { get; set; }

        public DateTime CreationDate { get; set; }

        public DateTime ExpireDate { get; set; }

        public byte Percentage { get; set; }

        public int StatusKey { get; set; }

        public virtual ICollection<Goal> Goals { get; set; }
        public virtual ICollection<Person> Personel { get; set; }

        public Project()
        {
            Personel = new List<Person>();
            Goals = new List<Goal>();
        }  
    }
}
