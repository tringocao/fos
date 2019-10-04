using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FOS.Model.Domain;
using FOS.Model.Dto;

namespace FOS.Model.Mapping
{
    public interface IRecurrenceEventDtoMapper
    {
        Model.Domain.RecurrenceEvent ToModel(Dto.RecurrenceEvent recurrenceEvent);
        Dto.RecurrenceEvent ToDto(Model.Domain.RecurrenceEvent recurrenceEvent );
    }
    public class RecurrenceEventDtoMapper : IRecurrenceEventDtoMapper
    {
        public Dto.RecurrenceEvent ToDto(Domain.RecurrenceEvent recurrenceEvent)
        {
            return new Dto.RecurrenceEvent() {
                Id = recurrenceEvent.Id,
                Title = recurrenceEvent.Title,
                TypeRepeat = (Dto.RepeateType)Enum.Parse(typeof(Dto.RepeateType), recurrenceEvent.TypeRepeat.ToString()),
                EndDate = recurrenceEvent.EndDate,
                StartDate = recurrenceEvent.StartDate,
                UserId = recurrenceEvent.UserId != null ? recurrenceEvent.UserId : null,
                UserMail = recurrenceEvent.UserMail,
                StartTempDate = recurrenceEvent.StartTempDate,
                Version = recurrenceEvent.Version,
                UserName = recurrenceEvent.UserName
            };
        }

        public Domain.RecurrenceEvent ToModel(Dto.RecurrenceEvent recurrenceEvent)
        {
            return new Domain.RecurrenceEvent()
            {
                Id = recurrenceEvent.Id,
                Title = recurrenceEvent.Title,
                TypeRepeat = (Domain.RepeateType)Enum.Parse(typeof(Domain.RepeateType), recurrenceEvent.TypeRepeat.ToString()),
                EndDate = recurrenceEvent.EndDate.ToLocalTime(),
                StartDate = recurrenceEvent.StartDate.ToLocalTime(),
                UserId = recurrenceEvent.UserId != null ? recurrenceEvent.UserId : null,
                StartTempDate = recurrenceEvent.StartTempDate,
                Version = recurrenceEvent.Version,
                UserMail = recurrenceEvent.UserMail,
                UserName = recurrenceEvent.UserName


            };
        }
    }
}
