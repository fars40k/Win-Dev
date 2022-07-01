using System;

namespace Win_Dev.Data.Dto
{
    class GoalsToProjects
    {
        public Guid GoalID { get; set; }
        public Guid ProjectID { get; set; }

        public virtual Goal Goal { get; set; }
        public virtual Project Project { get; set; }
    }
}
