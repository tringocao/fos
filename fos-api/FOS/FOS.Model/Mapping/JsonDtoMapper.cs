using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using FOS.Model.Dto;

namespace FOS.Model.Mapping
{
    public interface IJsonDtoMapper<T> where T : class
    {
        dynamic ToModel(T order);
        T ToDto(dynamic data);
    }

    public class JsonDtoMapper<T> : IJsonDtoMapper<T> where T : class
    {
        public T ToDto(dynamic data)
        {
            return Mapper.Map<dynamic, T>(data);
        }

        public dynamic ToModel(T order)
        {
            throw new NotImplementedException();
        }
    }
}
