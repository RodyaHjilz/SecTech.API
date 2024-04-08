using SecTech.Domain.Dto.User;
using SecTech.Domain.Entity;
using SecTech.Domain.Result;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SecTech.Domain.Interfaces.Services
{
    public interface IUserService
    {
        Task<BaseResult<User>> Register(string email, string password);
        Task<BaseResult<Token>> Login(string email, string password);

        
    }
}
