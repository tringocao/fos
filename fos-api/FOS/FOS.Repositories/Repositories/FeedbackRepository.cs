using FOS.Repositories.DataModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FOS.Repositories.Repositories
{
    public interface IFeedbackRepository
    {
        FeedBack GetById(string Id);
        void Update(FeedBack feedBack);

    }
    public class FeedbackRepository : IFeedbackRepository
    {
        private readonly FosContext _context;
        public FeedbackRepository(FosContext context)
        {
            _context = context;
        }

        public FeedBack GetById(string Id)
        {
            try
            {
                var _feedback = _context.FeedBacks.Where(feedback => feedback.DeliveryId == Id).FirstOrDefault();
                return _feedback;
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public void Update(FeedBack feedBack)
        {
           var _feedback = _context.FeedBacks.FirstOrDefault(fb => fb.DeliveryId == feedBack.DeliveryId);
           if (_feedback == null)
           {
                _context.FeedBacks.Add(feedBack);
                _context.SaveChanges();
           }
           else
            {
                _context.FeedBacks.Where(fb => fb.DeliveryId == feedBack.DeliveryId).FirstOrDefault().Ratings = feedBack.Ratings;
                _context.FeedBacks.Where(fb => fb.DeliveryId == feedBack.DeliveryId).FirstOrDefault().FoodFeedbacks = feedBack.FoodFeedbacks;
                _context.SaveChanges();
            }
        }
    }
}
