using System.Collections.Generic;
using System.Threading.Tasks;
using backend.Models;
using backend.Services;
using Xunit;

namespace backend.Tests
{
    public class UserServiceTests
    {
        private readonly IUserService _userService;

        public UserServiceTests()
        {
            _userService = new UserService(new System.Net.Http.HttpClient());
        }

        [Fact]
        public async Task GetNearestUsersAsync_ShouldReturnNearestUsers()
        {
            // Arrange
            var hotels = new List<Hotel>
            {
                new Hotel { Name = "Hotel A", Latitude = -43.9509, Longitude = -34.4618 },
                new Hotel { Name = "Hotel B", Latitude = 40.7128, Longitude = -74.0060 },
                new Hotel { Name = "Hotel C", Latitude = 34.0522, Longitude = -118.2437 },
                new Hotel { Name = "Hotel D", Latitude = -25.2744, Longitude = 133.7751 }
            };

            // Act
            var nearestUsers = await _userService.GetNearestUsersAsync(hotels);

            // Assert
            Assert.NotNull(nearestUsers);
            Assert.Equal(4, nearestUsers.Count);
        }
    }
}
