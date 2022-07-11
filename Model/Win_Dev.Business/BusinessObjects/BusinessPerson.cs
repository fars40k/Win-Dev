using System;
using System.Collections.Generic;
using Win_Dev.Data;

namespace Win_Dev.Business
{
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
            get => Person.ProjectsWith;
            set
            {
                Person.ProjectsWith.Clear();
                foreach (Project item in value)
                Person.ProjectsWith.Add(item);
            }
        }
        public ICollection<Goal> Goals
        {
            get => Person.GoalsWith;
            set
            {
                Person.GoalsWith.Clear();
                foreach (Goal item in value)
                    Person.GoalsWith.Add(item);
            }
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
