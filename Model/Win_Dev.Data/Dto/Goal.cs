namespace Win_Dev.Data
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class Goal
    {
        public Guid GoalID { get; set; }

        [Required]
        [StringLength(50)]
        public string Name { get; set; }

        [Required]
        [StringLength(4000)]
        public string Description { get; set; }

        public DateTime CreationDate { get; set; }

        public DateTime ExpireDate { get; set; }

        public byte Percentage { get; set; }

        public int Priority { get; set; }

        public int StatusKey { get; set; }

        public virtual Project InProject { get; set;}
        public virtual ICollection<Person> Personel { get; set;}

        public Goal()
        {
            Personel = new List<Person>();           
        }
        
    }
}
