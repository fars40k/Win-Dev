namespace Win_Dev.Business
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using Win_Dev.Data;

    /// <summary>
    ///  Wrap for data access class
    /// </summary>
    public partial class BusinessPerson 
    {
        public Person Person;

        public Guid PersonID
        {
            get => Person.PersonID;
            set => Person.PersonID = value;
        }

        public string FirstName
        {
            get => Person.FirstName;
            set => Person.FirstName = value;
        }

        public string SurName
        {
            get => Person.SurName;
            set => Person.SurName = value;
        }

        public string LastName
        {
            get => Person.LastName;
            set => Person.LastName = value;
        }

        public string Division
        {
            get => Person.Division;
            set => Person.Division = value;
        }

        public string Occupation
        {
            get => Person.Occupation;
            set => Person.Occupation = value;
        }

        public ICollection<Project> Projects
        {
            get => Person.Projects;
            set => Person.Projects = value;
        }
        public ICollection<Goal> Goals
        {
            get => Person.Goals;
            set => Person.Goals = value;
        }

        public BusinessPerson() : base()
        {
            
        }

        public BusinessPerson(Person newPerson) : base()
        {
            Person = newPerson;
        }

        public override string ToString()
        {
            string buffer = "";
            buffer += FirstName ?? "?";
            buffer += " " + SurName ?? "?";
            buffer += " " + LastName ?? "?";
            return (buffer);
        }

    }
}
