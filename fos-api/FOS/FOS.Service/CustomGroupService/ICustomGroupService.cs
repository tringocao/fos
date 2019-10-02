using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FOS.Services.CustomGroupService
{
    public interface ICustomGroupService
    {
        void CreateGroup(Model.Domain.CustomGroup customGroup);
        void UpdateGroup(Model.Domain.CustomGroup customGroup);
        void DeleteGroupById(string groupId);
        List<Model.Domain.CustomGroup> GetAll(string ownerId);
    }
}
