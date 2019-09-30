using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FOS.Model.Mapping
{
    public interface IUserReorderDtoMapper
    {
        Model.Domain.UserReorder ToModel(Dto.UserReorder userReorder);
        Dto.UserReorder ToDto(Model.Domain.UserReorder userReorder);
    }

    public class UserReorderDtoMapper : IUserReorderDtoMapper
    {
        public Dto.UserReorder ToDto(Model.Domain.UserReorder userReorder)
        {
            return new Dto.UserReorder()
            {
                UserMail = userReorder.UserMail,
                OrderId = userReorder.OrderId,
                EventRestaurant = userReorder.EventRestaurant,
                EventTitle = userReorder.EventTitle,
                FoodName = userReorder.FoodName,
                UserName = userReorder.UserName
            };
        }

        public Model.Domain.UserReorder ToModel(Dto.UserReorder userReorder)
        {
            return new Domain.UserReorder()
            {
                UserMail = userReorder.UserMail,
                OrderId = userReorder.OrderId,
                EventRestaurant = userReorder.EventRestaurant,
                EventTitle = userReorder.EventTitle,
                FoodName = userReorder.FoodName,
                UserName = userReorder.UserName

            };
        }
    }
}
