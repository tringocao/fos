using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FOS.Repositories.DataModel;

namespace FOS.Repositories.Repositories
{
    public interface ICustomGroupRepository {
        void CreateGroup(DataModel.CustomGroup customGroup);
        void UpdateGroup(DataModel.CustomGroup customGroup);
        void DeleteGroupById(string groupId);
        List<DataModel.CustomGroup> GetAll(string ownerId);
    }
    public class CustomGroupRepository : ICustomGroupRepository
    {
        private readonly FosContext _context;
        public CustomGroupRepository(FosContext context)
        {
            _context = context;
        }
        public void CreateGroup(CustomGroup customGroup)
        {
            _context.CustomGroups.Add(customGroup);
            _context.SaveChanges();
        }

        public void DeleteGroupById(string groupId)
        {
            var groupGuid = new Guid(groupId);
            var group = _context.CustomGroups.FirstOrDefault(g => g.ID == groupGuid);
            foreach(var user in group.Users.ToList())
            {
                _context.GraphUserInGroup.Remove(user);
            }
            _context.CustomGroups.Remove(group);
            _context.SaveChanges();
        }

        public List<CustomGroup> GetAll(string ownerId)
        {
            return _context.CustomGroups.Where(g => g.Owner == ownerId).ToList();
        }

        public void UpdateGroup(CustomGroup customGroup)
        {
            var entity = _context.CustomGroups.First(g => g.ID == customGroup.ID);
            entity.Name = customGroup.Name;
            foreach(var user in entity.Users.ToList())
            {
                _context.GraphUserInGroup.Remove(user);
            }
            foreach(var user in customGroup.Users.ToList())
            {
                user.Id = Guid.NewGuid();
                entity.Users.Add(user);
            }
            _context.SaveChanges();
        }
    }
}
