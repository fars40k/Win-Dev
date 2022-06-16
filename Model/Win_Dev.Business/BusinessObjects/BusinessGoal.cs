using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Win_Dev.Data;

namespace Win_Dev.Business.BusinessObjects
{
    class BusinessGoal
    {
        public Goal Goal;

        public Guid GoalID
        {
            get => Goal.GoalID;
            set => Goal.GoalID = value;
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
            get => Goal.Personel.ToList<Person>();
            set
            {
                Goal.Personel.Clear();
                foreach (Person item in value)
                    Goal.Personel.Add(item);
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
