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
            Dto.PromotionType PromotionType = Dto.PromotionType.FreeShip;
            switch (promotion.DiscountOnType)
            {
                case "3":
                    {
                        PromotionType = Dto.PromotionType.FreeShip;
                        break;
                    }
                case "2":
                    {
                        PromotionType = Dto.PromotionType.DiscountAll;
                        break;
                    }
                case null:
                    {
                        PromotionType = Dto.PromotionType.DiscountPerItem;
                        break;
                    }
                case "4":
                    {
                        PromotionType = Dto.PromotionType.ShipFee;
                        break;
                    }
            }
            DateTime oDate = DateTime.ParseExact(promotion.Expired, "dd/MM/yyyy HH:mm", null);

            return new Dto.Promotion()
            {
                Expired = oDate,
                IsPercent = promotion.DiscountValueType == "1" ? true : false,
                MaxDiscountAmount = promotion.MaxDiscountAmount !=null ? Int32.Parse(promotion.MaxDiscountAmount) : 0,
                MinOrderAmount = promotion.MinOrderAmount != null ? Int32.Parse(promotion.MinOrderAmount) : 0,
                Value = promotion.DiscountAmount != null ? Int32.Parse(promotion.DiscountAmount) : 0,
                PromotionType = PromotionType
            };
        }
        public Domain.NowModel.Promotion ToModel(Dto.Promotion promotion)
        {
            string discountOnType = null;
            switch (promotion.PromotionType)
            {
                case PromotionType.FreeShip:
                    {
                        discountOnType = "3";
                        break;
                    }
                case PromotionType.DiscountPerItem:
                    {
                        discountOnType = null;
                        break;
                    }
                case PromotionType.DiscountAll:
                    {
                        discountOnType = "2";
                        break;
                    }
                case PromotionType.ShipFee:
                    {
                        discountOnType = "4";
                        break;
                    }
            }
            return new Domain.NowModel.Promotion()
            {
                Expired = promotion.Expired.Value.ToString("dd/MM/yyyy HH:mm"),
                MaxDiscountAmount = promotion.MaxDiscountAmount.ToString(),
                MinOrderAmount = promotion.MinOrderAmount.ToString(),
                DiscountValueType = promotion.IsPercent ? "1" : "3",
                DiscountAmount = promotion.Value.ToString(),
                DiscountOnType = discountOnType
            };
        }
    }

    public interface IPromotionDtoMapper
    {
        Model.Domain.NowModel.Promotion ToModel(Dto.Promotion promotion);
        Dto.Promotion ToDto(Model.Domain.NowModel.Promotion promotion);
    }
}
