using SecTech.Domain.Dto.Group;
using SecTech.Domain.Entity;
using SecTech.Domain.Result;


namespace SecTech.Domain.Interfaces.Services
{
    public interface IGroupService
    {
        Task<BaseResult<UGroup>> CreateGroup(CreateGroupDto group);
        Task<BaseResult<UGroup>> AddUserToGroup(string userEmail, string groupName);
    }
}
