using Moq;
using SecTech.Application.Services;
using SecTech.Domain.Entity;
using SecTech.Domain.Interfaces.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SecTech.Tests.Services
{
    public class UserServiceTests
    {

        private readonly Mock<IBaseRepository<User>> _mockRepository;
        private readonly UserService _userService;
        public UserServiceTests(Mock<IBaseRepository<User>> mockRepository, UserService userService)
        {
            _mockRepository = mockRepository;
            _userService = userService;
        }

        
        public void Login_Success()
        {
            // Arrange


            // Act


            // Assert
        }

    }
}
