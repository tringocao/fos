using AutoMapper;
using FOS.Model;
using FOS.Repositories.DataModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

[assembly: WebActivatorEx.PreApplicationStartMethod(typeof(FOS.Repositories.Repositories.AutoMapperConfig), "RegisterMapping")]
namespace FOS.Repositories.Repositories
{
   public class AutoMapperConfig
    {
        public static void RegisterMapping()
        {
            Mapper.Initialize(config =>
            {
                config.CreateMap<ExternalServiceAPI, APIs>()
                .ForMember(model => model.TypeService, cfig => cfig.MapFrom(e => e.TypeService))
                //.ForMember(model => model.header, cfig => cfig.MapFrom(c => new Dictionary<string,
                //    string>(c.FOSHeaderLinks.ToDictionary(dic => dic.Name, dic => dic.DefaultValue))))
                //.ForMember(model => model.body, cfig => cfig.MapFrom(c => new Dictionary<string,
                //    string>(c.FOSBodyFieldLinks.ToDictionary(dic => dic.Name, dic => dic.DefaultValue))))
                .ReverseMap();


            });
        }

    }
}
