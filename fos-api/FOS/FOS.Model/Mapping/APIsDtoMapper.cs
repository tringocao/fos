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
        Model.APIs ToModel(Dto.APIs order);
        Dto.APIs ToDto(Model.APIs order);
    }

    public class APIsDtoMapper : IAPIsDtoMapper
    {
        public Dto.APIs ToDto(APIs api)
        {
            return new Dto.APIs()
            {
                ID = api.ID,
                JSONData = api.JSONData,
                Name = api.Name,
                TypeService = Config.ToEnum<ServiceKind>(api.TypeService)
            };
        }

        public APIs ToModel(Dto.APIs api)
        {
            return new Model.APIs()
            {
                ID = api.ID,
                JSONData = api.JSONData,
                Name = api.Name,
                TypeService = api.TypeService.GetType().FullName
            };
        }
       

    }

}
