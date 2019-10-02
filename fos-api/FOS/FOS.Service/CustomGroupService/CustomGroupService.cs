using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FOS.Model.Domain;
using FOS.Repositories.Mapping;
using FOS.Repositories.Repositories;

namespace FOS.Services.CustomGroupService
{
    public class CustomGroupService : ICustomGroupService
    {
        ICustomGroupRepository _customGroupRepository;
        ICustomGroupMapper _customGroupMapper;
        public CustomGroupService(ICustomGroupRepository customGroupRepository, ICustomGroupMapper customGroupMapper)
        {
            _customGroupRepository = customGroupRepository;
            _customGroupMapper = customGroupMapper;
        }
        public void CreateGroup(CustomGroup customGroup)
        {
            var dataModelGroup = new Repositories.DataModel.CustomGroup();
            dataModelGroup = _customGroupMapper.DomainToDataModel(customGroup);
            dataModelGroup.ID = Guid.NewGuid();
            foreach(var user in dataModelGroup.Users)
            {
                user.Id = Guid.NewGuid();
            }
            _customGroupRepository.CreateGroup(dataModelGroup);
        }

        public void DeleteGroupById(string groupId)
        {
            _customGroupRepository.DeleteGroupById(groupId);
        }

        public List<CustomGroup> GetAll(string ownerId)
        {
            var allGroup = _customGroupRepository.GetAll(ownerId);
            var customGroups = new List<CustomGroup>();
            customGroups = allGroup.Select(g => _customGroupMapper.DataModelToDomain(g)).ToList();
            return customGroups;
        }

        public void UpdateGroup(CustomGroup customGroup)
        {
            var dataModelGroup = new Repositories.DataModel.CustomGroup();
            dataModelGroup = _customGroupMapper.DomainToDataModel(customGroup);
            _customGroupRepository.UpdateGroup(dataModelGroup);
        }
    }
}
