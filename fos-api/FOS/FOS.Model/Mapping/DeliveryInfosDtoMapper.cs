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
                Categories = deliveryInfos.Categories.Count() > 0 ? deliveryInfos.Categories[0] : "",
                CityId = deliveryInfos.CityId.ToString(),
                DeliveryId = deliveryInfos.DeliveryId.ToString(),
                IsFoodyDelivery = deliveryInfos.IsFoodyDelivery.ToString(),
                IsOpen = deliveryInfos.IsOpen.ToString(),
                Name = deliveryInfos.Name,
                Operating = deliveryInfos.Operating.OpenTime + "-" + deliveryInfos.Operating.CloseTime,
                Photo = deliveryInfos.Photos.FirstOrDefault().Value,
                PromotionGroups = deliveryInfos.PromotionGroups.Count() > 0 ? deliveryInfos.PromotionGroups[0].Text : "",
                RestaurantId = deliveryInfos.RestaurantId.ToString(),
                UrlRewriteName = deliveryInfos.UrlRewriteName
            };
        }
    }

    public interface IDeliveryInfosDtoMapper
    {
        Dto.DeliveryInfos ToDto(Model.Domain.NowModel.DeliveryInfos deliveryInfos);
    }
}
