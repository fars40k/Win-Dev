using System;

namespace Win_Dev.Data.Dto
{
    class PersonelToGoals
    {
        public Guid PersonID { get; set; }        
        public Guid GoalID { get; set; }

        public virtual Person Person { get; set; }
        public virtual Goal Goal { get; set; }
    }
}
