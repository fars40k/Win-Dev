using System;
using System.Collections.Generic;
using System.Linq;
using Win_Dev.Data;

namespace Win_Dev.Business
{
    /// <summary>
    ///  Wrap for data access class
    /// </summary>
    public class BusinessGoal
    {
        public Goal Goal;
        public Guid GoalID
        {
            get => Goal.GoalID;
            set => Goal.GoalID = value;
        }

        public string Name
        {
            get => Goal.Name;
            set => Goal.Name = value;
        }

        public string Description
        {
            get => Goal.Description;
            set => Goal.Description = value;
        }

        public DateTime CreationDate
        {
            get => Goal.CreationDate;
            set => Goal.CreationDate = value;
        }

        public DateTime ExpireDate
        {
            get => Goal.ExpireDate;
            set => Goal.ExpireDate = value;
        }

        public byte Percentage
        {
            get => Goal.Percentage;
            set => Goal.Percentage = value;
        }

        public int Priority
        {
            get => Goal.Priority;
            set => Goal.Priority = value;

        }

        public int StatusKey
        {
            get => Goal.StatusKey;
            set => Goal.StatusKey = value;
        }

        public ICollection<Project> Project
        {
            get => Goal.ProjectsWith.ToList<Project>();
            set
            {
                Goal.ProjectsWith.Clear();
                foreach (Project item in value)
                    Goal.ProjectsWith.Add(item);
            }
        }
        public ICollection<Person> Personel
        {
            get => Goal.PersonelWith.ToList<Person>();
            set
            {
                Goal.PersonelWith.Clear();
                foreach (Person item in value)
                    Goal.PersonelWith.Add(item);
            }
        }

        public int PersonelOnGoal
        {
            get => Goal.PersonelWith.Count;
            set
            {

            }
        }

        public string CreationDateShort
        {
            get => Goal.CreationDate.ToShortDateString();
            set
            {

            }
        }

        public string ExpireDateShort
        {
            get => Goal.ExpireDate.ToShortDateString();
            set
            {

            }
        }

        public BusinessGoal() : base()
        {

        }

        public BusinessGoal(Goal newGoal) : base()
        {
            Goal = newGoal;
        }

        public override string ToString()
        {
            return Goal.Name;
        }
    }
}
