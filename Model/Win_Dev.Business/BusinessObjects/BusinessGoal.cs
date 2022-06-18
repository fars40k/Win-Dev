namespace Win_Dev.Business
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Win_Dev.Data;

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

        public int StatusKey
        {
            get => Goal.StatusKey;
            set => Goal.StatusKey = value;
        }

        public virtual Project InProject
        {
            get => InProject;
            set => InProject = value;
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

        public BusinessGoal() : base()
        {

        }

        public BusinessGoal(Goal newGoal) : base()
        {
            Goal = newGoal;
        }

        public override string ToString()
        {
            return (Goal.Name);
        }
    }
}
