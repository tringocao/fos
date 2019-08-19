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
        Model.Order ToModel(Dto.Order order);
        Dto.Order ToDto(Model.Order order);
    }

    public class OrderDtoMapper : IOrderDtoMapper
    {
        public Dto.Order ToDto(Order order)
        {
            throw new NotImplementedException();
        }

        public Order ToModel(Dto.Order order)
        {
            throw new NotImplementedException();
        }
    }
}
