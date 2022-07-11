using System;
using System.Collections.Generic;
using System.Linq;
using Win_Dev.Data;

namespace Win_Dev.Business
{
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
            get => Project.PersonelWith.ToList<Person>();
            set
            {
                Project.PersonelWith.Clear();
                foreach (Person item in value)
                    Project.PersonelWith.Add(item);
            }
        }

        public ICollection<Goal> Goals
        {
            get => Project.GoalsIn.ToList<Goal>();
            set
            {
                Project.GoalsIn.Clear();
                foreach (Goal item in value)
                    Project.GoalsIn.Add(item);
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
