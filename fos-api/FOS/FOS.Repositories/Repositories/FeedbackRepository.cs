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
            return _context.FeedBacks.Where(feedback => feedback.DeliveryId == Id).FirstOrDefault();
        }

        public void Update(FeedBack feedBack)
        {
            throw new NotImplementedException();
        }
    }
}
