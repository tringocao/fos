using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FOS.Model.Domain.NowModel;
using FOS.Model.Dto;

namespace FOS.Model.Mapping
{
    class ProvinceDtoMapper : IProvinceDtoMapper
    {
        public Dto.Province ToDto(Domain.NowModel.Province province)
        {
            return new Dto.Province()
            {
                CountryId = province.CountryId,
                District = province.District,
                Id = province.Id,
                Name = province.Name,
                NameUrl = province.NameUrl,
                Service = province.Service
            };
        }
    }

    public interface IProvinceDtoMapper
    {
        Dto.Province ToDto(Model.Domain.NowModel.Province province);

    }
}
