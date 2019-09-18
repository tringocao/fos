using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FOS.Model.Mapping
{
    public interface IDishesSummaryDtoMapper
    {
        Model.Dto.DishesSummary ElementToDto(Model.Domain.DishesSummary dishesSummary);
        IEnumerable<Model.Dto.DishesSummary> ListToDto(IEnumerable<Model.Domain.DishesSummary> dishesSummary);
    }
    public class DishesSummaryDtoMapper : IDishesSummaryDtoMapper
    {
        public Model.Dto.DishesSummary ElementToDto(Model.Domain.DishesSummary dishesSummary)
        {
            return new Model.Dto.DishesSummary
            {
                Rank = dishesSummary.Rank,
                Food = dishesSummary.Food,
                RelativePercent = dishesSummary.RelativePercent,
                Percent = dishesSummary.Percent,
            };
        }
        public IEnumerable<Model.Dto.DishesSummary> ListToDto(IEnumerable<Model.Domain.DishesSummary> dishesSummary)
        {
            return dishesSummary.Select(c => ElementToDto(c)).ToList();
        }
    }
}
