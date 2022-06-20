using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Win_Dev.Data.Dto
{
    class PersonelToProjects
    {
        public Guid PersonID { get; set; }
        public Guid ProjectID { get; set; }

        public virtual Person Person { get; set; }
        public virtual Project Project { get; set; }
    }
}
