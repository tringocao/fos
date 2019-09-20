using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FOS.Model.Mapping
{
    public interface IRestaurantSummaryDtoMapper
    {
        Model.Dto.RestaurantSummary ElementToDto(Model.Domain.RestaurantSummary restaurantSummary);
        IEnumerable<Model.Dto.RestaurantSummary> ListToDto(IEnumerable<Model.Domain.RestaurantSummary> restaurantSummary);
    }
    public class RestaurantSummaryDtoMapper : IRestaurantSummaryDtoMapper
    {
        public Model.Dto.RestaurantSummary ElementToDto(Model.Domain.RestaurantSummary restaurantSummary)
        {
            return new Model.Dto.RestaurantSummary
            {
                Rank = restaurantSummary.Rank,
                Restaurant = restaurantSummary.Restaurant,
                RelativePercent = restaurantSummary.RelativePercent,
                RestaurantId = restaurantSummary.RestaurantId,
                Percent = restaurantSummary.Percent,
                DeliveryId = restaurantSummary.DeliveryId,
                ServiceId = restaurantSummary.ServiceId,
                AppearTime = restaurantSummary.AppearTimes
            };
        }
        public IEnumerable<Model.Dto.RestaurantSummary> ListToDto(IEnumerable<Model.Domain.RestaurantSummary> restaurantSummary)
        {
            return restaurantSummary.Select(c => ElementToDto(c)).ToList();
        }
    }
}
