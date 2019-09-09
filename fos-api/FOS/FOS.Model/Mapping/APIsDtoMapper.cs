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
        Model.Apis ToModel(Dto.APIs order);
        Dto.APIs ToDto(Model.Apis order);
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
                TypeService = Config.ToEnum<ServiceKind>(api.TypeService)
            };
        }

        public Apis ToModel(Dto.APIs api)
        {
            return new Model.Apis()
            {
                ID = api.ID,
                JSONData = api.JSONData,
                Name = api.Name,
                TypeService = api.TypeService.GetType().FullName
            };
        }
       

    }

}
