using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FOS.Model.Mapping
{
    public interface ICommentsDtoMapper
    {
        Model.Domain.Comment ToDomain(Model.Dto.Comment dto);
        Model.Dto.Comment ToDto(Model.Domain.Comment domainComment);
    }
    public class CommentsDtoMapper : ICommentsDtoMapper
    {
        public Model.Domain.Comment ToDomain(Model.Dto.Comment dtoComments)
        {
            return new Domain.Comment
            {
                Amount = dtoComments.Amount,
                Value = dtoComments.Value
            };
        }
        public Model.Dto.Comment ToDto(Model.Domain.Comment domainComments)
        {
            return new Dto.Comment
            {
                Amount = domainComments.Amount,
                Value = domainComments.Value
            };
        }
    }
}
