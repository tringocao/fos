using FOS.Model.Domain;
using FOS.Model.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FOS.Model.Mapping
{
    public interface IAPIsDtoMapper
    {
        Dto.ExternalService ToDto(Domain.Apis order);
    }

    public class APIsDtoMapper : IAPIsDtoMapper
    {
        public Dto.ExternalService ToDto(Apis api)
        {
            return new Dto.ExternalService()
            {
                Id = api.ID,
                Name = api.Name,
            };
        }

    
       

    }

}
