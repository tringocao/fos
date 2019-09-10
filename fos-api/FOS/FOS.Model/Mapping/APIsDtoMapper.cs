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
        Domain.Apis ToModel(Dto.APIs order);
        Dto.APIs ToDto(Domain.Apis order);
    }

    public class APIsDtoMapper : IAPIsDtoMapper
    {
        public Dto.APIs ToDto(Apis api)
        {
            return new Dto.APIs()
            {
                ID = api.ID,
                JSONData = api.JSONData,
                Name = api.Name,
                TypeService = Config.ToEnum<ServiceKind>(api.TypeService.GetType().FullName)
            };
        }

        public Apis ToModel(Dto.APIs api)
        {
            return new Domain.Apis()
            {
                ID = api.ID,
                JSONData = api.JSONData,
                Name = api.Name,
                TypeService = api.TypeService
            };
        }
       

    }

}
