using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FOS.Model.Domain;
using FOS.Repositories.DataModel;
using Newtonsoft.Json;

namespace FOS.Repositories.Mapping
{
    public interface IFeedbackMapper
    {
        Model.Domain.FeedBack MapToDomain(DataModel.FeedBack feedBack);
        DataModel.FeedBack MapToDataModel(Model.Domain.FeedBack feedBack);
    }
    public class FeedbackMapper : IFeedbackMapper
    {
        public DataModel.FeedBack MapToDataModel(Model.Domain.FeedBack feedBack)
        {
            return new DataModel.FeedBack()
            {
                DeliveryId = feedBack.DeliveryId,
                Ratings = JsonConvert.SerializeObject(feedBack.Ratings),
                FoodFeedbacks = JsonConvert.SerializeObject(feedBack.FoodFeedbacks)
            };
        }

        public Model.Domain.FeedBack MapToDomain(DataModel.FeedBack feedBack)
        {
            return new Model.Domain.FeedBack()
            {
                DeliveryId = feedBack.DeliveryId,
                Ratings = JsonConvert.DeserializeObject<Dictionary<string, float>>(feedBack.Ratings),
                FoodFeedbacks = JsonConvert.DeserializeObject<Dictionary<int, Dictionary<string, string>>>(feedBack.FoodFeedbacks),
            };
        }
    }
}
