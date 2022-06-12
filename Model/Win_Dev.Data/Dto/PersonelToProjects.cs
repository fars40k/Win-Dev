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
        public Personel Person { get; set; }

        public Guid ProjectID { get; set; }
        public Project Project { get; set; }
    }
}
