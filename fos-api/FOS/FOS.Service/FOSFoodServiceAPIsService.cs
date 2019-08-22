using FOS.Model.Domain;
using FOS.Model.Dto;
using FOS.Repositories.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FOS.Services
{
    public interface IFOSFoodServiceAPIsService
    {
        //string GetByIdAsync(int businessId);
        APIsDTO GetById(int Id);
    }
    public class FOSFoodServiceAPIsService : IFOSFoodServiceAPIsService
    {
        IFOSFoodServiceAPIsRepository _iFOSFood;
        public FOSFoodServiceAPIsService(IFOSFoodServiceAPIsRepository iFOSFood)
        {
            _iFOSFood = iFOSFood;
        }
        public APIsDTO GetById(int Id)
        {
            APIs a = _iFOSFood.GetFOSCrawlLinksById(Id);
            APIsDTO a2 = new APIsDTO() { ID = a.ID, JSONData = a.JSONData, Name = a.Name };
            //if (a.TypeService == ServiceKind.Now.GetType().FullName)
            //{
            //    a2.TypeService = ServiceKind.Now;
            //}
            a2.TypeService = ServiceKind.Now;
            return a2;
        }

    }
}
