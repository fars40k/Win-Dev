using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
