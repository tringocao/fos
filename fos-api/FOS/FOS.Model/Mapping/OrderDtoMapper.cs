using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FOS.Model.Dto;

namespace FOS.Model.Mapping
{
    public interface IOrderDtoMapper
    {
        Model.Domain.Order ToModel(Dto.Order order);
        Dto.Order ToDto(Model.Domain.Order order);
    }

    public class OrderDtoMapper : IOrderDtoMapper
    {
        public Dto.Order ToDto(Model.Domain.Order order)
        {
            throw new NotImplementedException();
        }

        public Model.Domain.Order ToModel(Dto.Order order)
        {
            throw new NotImplementedException();
        }
    }
}
