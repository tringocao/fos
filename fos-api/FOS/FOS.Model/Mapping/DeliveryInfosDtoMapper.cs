using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FOS.Model.Domain.NowModel;
using FOS.Model.Dto;

namespace FOS.Model.Mapping
{
    public class DeliveryInfosDtoMapper : IDeliveryInfosDtoMapper
    {
        public Dto.DeliveryInfos ToDto(Domain.NowModel.DeliveryInfos deliveryInfos)
        {
            return new Dto.DeliveryInfos()
            {
                Address = deliveryInfos.Address,
                Campaigns = deliveryInfos.Campaigns,
                Categories = deliveryInfos.Categories,
                CityId = deliveryInfos.CityId,
                DeliveryId = deliveryInfos.DeliveryId,
                IsFoodyDelivery = deliveryInfos.IsFoodyDelivery,
                IsOpen = deliveryInfos.IsOpen,
                Name = deliveryInfos.Name,
                Operating = deliveryInfos.Operating,
                Photo = deliveryInfos.Photos.FirstOrDefault().Value,
                PromotionGroups = deliveryInfos.PromotionGroups,
                RestaurantId = deliveryInfos.RestaurantId,
                UrlRewriteName = deliveryInfos.UrlRewriteName
            };
        }
    }

    public interface IDeliveryInfosDtoMapper
    {
        Dto.DeliveryInfos ToDto(Model.Domain.NowModel.DeliveryInfos deliveryInfos);
    }
}
