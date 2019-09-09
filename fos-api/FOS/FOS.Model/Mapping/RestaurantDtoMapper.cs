using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FOS.Model.Domain.NowModel;
using FOS.Model.Dto;

namespace FOS.Model.Mapping
{
    public class RestaurantDtoMapper : IRestaurantDtoMapper
    {
        public Dto.Restaurant ToDto(Domain.NowModel.DeliveryInfos deliveryInfos)
        {
            return new Dto.Restaurant()
            {
                Address = deliveryInfos.Address,
                Category = deliveryInfos.Categories,
                Delivery_id = deliveryInfos.DeliveryId,
                Id = Int32.Parse(deliveryInfos.RestaurantId),
                Name = deliveryInfos.Name,
                Open = deliveryInfos.IsOpen,
                Picture = deliveryInfos.Photos.FirstOrDefault().Value,
                Promotion = deliveryInfos.PromotionGroups,
                Stare = .,
                UrlRewriteName = deliveryInfos.UrlRewriteName
            };
        }
    }

    public interface IRestaurantDtoMapper
    {
        Dto.Restaurant ToDto(Model.Domain.NowModel.DeliveryInfos deliveryInfos);

    }
}
