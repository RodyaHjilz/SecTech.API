using SecTech.Domain.Dto.Group;
using SecTech.Domain.Entity;
using SecTech.Domain.Result;


namespace SecTech.Domain.Interfaces.Services
{
    public interface IGroupService
    {
        Task<BaseResult<UGroupDto>> CreateGroup(UGroupDto group);
        Task<BaseResult<UGroupDto>> AddUserToGroup(string userEmail, string groupName);

        Task<BaseResult<IEnumerable<UGroupDto>>> GetGroups();

    }
}
