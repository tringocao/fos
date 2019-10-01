using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FOS.Model.Domain.NowModel;
using FOS.Model.Dto;

namespace FOS.Model.Mapping
{
    public class PromotionDtoMapper : IPromotionDtoMapper
    {

        public Dto.Promotion ToDto(Domain.NowModel.Promotion promotion)
        {
            Dto.PromotionType PromotionType;
            switch (promotion.DiscountOnType)
            {
                case "3":
                    {
                        PromotionType = Dto.PromotionType.ShipFee;
                        break;
                    }
                case "2":
                    {
                        PromotionType = Dto.PromotionType.DiscountAll;
                        break;
                    }
                default:
                    {
                        PromotionType = Dto.PromotionType.DiscountPerItem;
                        break;
                    }
            }
            return new Dto.Promotion()
            {
                Expired = DateTime.Parse(promotion.Expired),
                IsPercent = promotion.DiscountValueType == "1" ? true : false,
                MaxDiscountAmount = promotion.MaxDiscountAmount.Length > 0 ? Int32.Parse(promotion.MaxDiscountAmount) : 0,
                MinOrderAmount = promotion.MinOrderAmount.Length > 0 ? Int32.Parse(promotion.MinOrderAmount) : 0,
                Value = promotion.DiscountAmount.Length > 0 ? Int32.Parse(promotion.DiscountAmount) : 0,
                PromotionType = PromotionType
            };
        }

        public Domain.NowModel.Promotion ToModel(Dto.Promotion promotion)
        {
            throw new NotImplementedException();
        }
    }

    public interface IPromotionDtoMapper
    {
        Model.Domain.NowModel.Promotion ToModel(Dto.Promotion promotion);
        Dto.Promotion ToDto(Model.Domain.NowModel.Promotion promotion);
    }
}
