using Microsoft.EntityFrameworkCore;
using SecTech.DAL;
using SecTech.DAL.Migrations;
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
        public GroupService(IBaseRepository<UGroup> groupRepository,
                            IBaseRepository<User> userRepository)
        {
            _groupRepository = groupRepository;
            _userRepository = userRepository;
        }

        public async Task<BaseResult<UGroupDto>> AddUserToGroup(string userEmail, string groupName)
        {
            var user = _userRepository.GetAll()
                .Include(x=>x.Groups)
                .FirstOrDefault(x => x.Email == userEmail);
            var group = _groupRepository
                .GetAll()
                .Include(x=>x.Users)
                .FirstOrDefault(x => x.Name == groupName);
            if (group == null)
                return new BaseResult<UGroupDto>() { ErrorMessage = "Group not found" };
            if(user == null)
                return new BaseResult<UGroupDto>() { ErrorMessage = "User not found" };
            if (group.Users == null)
                group.Users = new List<User>();
            if (user.Groups == null)
                user.Groups = new List<UGroup>();
            if (group.Users.Any(u => u.Id == user.Id))
                return new BaseResult<UGroupDto>() { ErrorMessage = "User already exists in the group" };

            group.Users.Add(user);
            user.Groups.Add(group);    
            await _groupRepository.UpdateAsync(group);
            await _userRepository.UpdateAsync(user);
            var newGroup = new UGroupDto()
            {
                Name = group.Name,
                Description = group.Description,
                UserEmails = group.Users.Select(x => x.Email).ToList()
            };
            return new BaseResult<UGroupDto>() { Data = newGroup };
        }

        public async Task<BaseResult<UGroupDto>> CreateGroup(UGroupDto group)
        {
            try
            {
                var existingGroup = _groupRepository.GetAll()
                                                    .FirstOrDefault(x => x.Name == group.Name);
                if(existingGroup != null)
                    return new BaseResult<UGroupDto>() { ErrorMessage = "Group already exists" };


                List<User> userList = null;
                if (group.UserEmails != null)
                {
                    userList = _userRepository.GetAll()
                    .Where(x => group.UserEmails.Contains(x.Email))
                    .ToList();
                    if (userList != null)
                        group.UserEmails = userList.Select(x => x.Email).ToList();                }
                var newGroup = await _groupRepository.CreateAsync(new UGroup()
                {
                    Name = group.Name,
                    Description = group.Description,
                    Users = userList
                });

                

                return new BaseResult<UGroupDto> { Data = group };
            }
            catch (Exception ex)
            {
                return new BaseResult<UGroupDto> { ErrorMessage = ex.Message };
            }
        }
    }
}
