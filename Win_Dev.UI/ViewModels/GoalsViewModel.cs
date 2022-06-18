using GalaSoft.MvvmLight.Ioc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Win_Dev.Business;

namespace Win_Dev.UI.ViewModels
{
    class GoalsViewModel
    {
        public BusinessModel Model = SimpleIoc.Default.GetInstance<BusinessModel>();

       
    }
}
