namespace Win_Dev.Data
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Linq;

    /// <summary>
    ///  Wrap for data access class
    /// </summary>
    public partial class BusinessProject
    {
        public Project Project;

        public Guid ProjectID
        {
            get => Project.ProjectID;
            set => Project.ProjectID = value;
        }

        public string Name
        {
            get => Project.Name;
            set => Project.Name = value;
        }

        public string Description
        {
            get => Project.Description;
            set => Project.Description = value;
        }

        public DateTime CreationDate
        {
            get => Project.CreationDate;
            set => Project.CreationDate = value;
        }

        public DateTime ExpireDate
        {
            get => Project.ExpireDate;
            set => Project.ExpireDate = value;
        }

        public byte Percentage
        {
            get => Project.Percentage;
            set => Project.Percentage = value;
        }

        public int StatusKey
        {
            get => Project.StatusKey;
            set => Project.StatusKey = value;
        }

        public ICollection<Person> Personel
        {
            get => Project.Personel.ToList<Person>();
            set
            {
                Project.Personel.Clear();
                foreach (Person item in value)
                Project.Personel.Add(item);
            }
        }

        public ICollection<Goal> Goals
        {
            get => Project.Goals.ToList<Goal>();
            set
            {
                Project.Goals.Clear();
                foreach (Goal item in value)
                Project.Goals.Add(item);
            }
        }

        public BusinessProject() : base()
        {     
            
        }

        public BusinessProject(Project newProject) : base()
        {
            Project = newProject;
        }

        public override string ToString()
        {
            return Name;
        }
    }
}
