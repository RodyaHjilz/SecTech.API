using Microsoft.EntityFrameworkCore;
using SecTech.DAL;
using SecTech.DAL.Repositories;
using SecTech.Domain.Dto.Group;
using SecTech.Domain.Entity;
using SecTech.Domain.Interfaces.Repositories;
using SecTech.Domain.Interfaces.Services;
using SecTech.Domain.Result;
using System.Text.RegularExpressions;

namespace SecTech.Application.Services
{
    
    public class GroupService : IGroupService
    {
        private readonly IBaseRepository<UGroup> _groupRepository;
        private readonly IBaseRepository<User> _userRepository;
        public GroupService(IBaseRepository<UGroup> groupRepository)
        {
            _groupRepository = groupRepository;
        }

        public async Task<BaseResult<UGroup>> AddUserToGroup(string userEmail, string groupName)
        {
            var user = _userRepository.GetAll()
                .Include(x=>x.Groups)
                .FirstOrDefault(x => x.Email == userEmail);
            var group = _groupRepository
                .GetAll()
                .Include(x=>x.Users)
                .FirstOrDefault(x => x.Name == groupName);
            if (group == null)
                return new BaseResult<UGroup>() { ErrorMessage = "Group not found" };
            if(user == null)
                return new BaseResult<UGroup>() { ErrorMessage = "User not found" };
            if (group.Users == null)
                group.Users = new List<User>();
            if (user.Groups == null)
                user.Groups = new List<UGroup>();
            if (group.Users.Any(u => u.Id == user.Id))
                return new BaseResult<UGroup>() { ErrorMessage = "User already exists in the group" };

            group.Users.Add(user);
            user.Groups.Add(group);    
            var newGroup = await _groupRepository.UpdateAsync(group);
            await _userRepository.UpdateAsync(user);
            return new BaseResult<UGroup>() { Data = newGroup };
        }

        public async Task<BaseResult<UGroup>> CreateGroup(CreateGroupDto group)
        {
            try
            {
                List<User> userList = null;
                if (group.UserEmails != null)
                {
                    userList = _userRepository.GetAll()
                    .Where(x => group.UserEmails.Contains(x.Email))
                    .ToList();
                }
                var newGroup = await _groupRepository.CreateAsync(new UGroup()
                {
                    Name = group.Name,
                    Description = group.Description,
                    Users = userList
                });

                return new BaseResult<UGroup> { Data = newGroup };
            }
            catch (Exception ex)
            {
                return new BaseResult<UGroup> { ErrorMessage = ex.Message };
            }
        }
    }
}
