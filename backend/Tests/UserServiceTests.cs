using System.Collections.Generic;
using System.Threading.Tasks;
using System.Net.Http;
using System.Linq;
using backend.Models;
using backend.Services;
using Xunit;
using Moq;
using Moq.Protected;
using System.Threading;
using System.Net;
using System.Text;

namespace backend.Tests
{
    public class UserServiceTests
    {
        private readonly Mock<HttpMessageHandler> _mockHttpMessageHandler;
        private readonly HttpClient _httpClient;
        private readonly IUserService _userService;
        private readonly List<User> _testUsers;

        public UserServiceTests()
        {
            // Test data setup
            _testUsers = new List<User>
            {
                new User 
                {
                    Id = 1,
                    Name = "User 1",
                    Email = "user1@example.com",
                    Address = new Address
                    {
                        Geo = new Geo { Lat = -34.4618, Lng = -58.5718 } // Near Hotel A
                    }
                },
                new User 
                {
                    Id = 2,
                    Name = "User 2", 
                    Email = "user2@example.com",
                    Address = new Address
                    {
                        Geo = new Geo { Lat = 40.7548, Lng = -73.9774 } // Near Hotel B
                    }
                },
                new User 
                {
                    Id = 3,
                    Name = "User 3",
                    Email = "user3@example.com",
                    Address = new Address
                    {
                        Geo = new Geo { Lat = 34.0194, Lng = -118.4108 } // Near Hotel C
                    }
                },
                new User 
                {
                    Id = 4,
                    Name = "User 4",
                    Email = "user4@example.com",
                    Address = new Address
                    {
                        Geo = new Geo { Lat = -25.3444, Lng = 131.0369 } // Near Hotel D
                    }
                },
                new User 
                {
                    Id = 5,
                    Name = "User 5",
                    Email = "user5@example.com",
                    Address = new Address
                    {
                        Geo = new Geo { Lat = 51.5074, Lng = -0.1278 } // Far from all hotels
                    }
                }
            };

            // Mock HTTP handler setup
            _mockHttpMessageHandler = new Mock<HttpMessageHandler>();
            _mockHttpMessageHandler
                .Protected()
                .Setup<Task<HttpResponseMessage>>(
                    "SendAsync",
                    ItExpr.IsAny<HttpRequestMessage>(),
                    ItExpr.IsAny<CancellationToken>())
                .ReturnsAsync(new HttpResponseMessage
                {
                    StatusCode = HttpStatusCode.OK,
                    Content = new StringContent(Newtonsoft.Json.JsonConvert.SerializeObject(_testUsers), Encoding.UTF8, "application/json")
                });

            _httpClient = new HttpClient(_mockHttpMessageHandler.Object);
            _userService = new UserService(_httpClient);
        }

        [Fact]
        public async Task GetNearestUsersAsync_ShouldReturnCorrectNearestUsers()
        {
            // Arrange
            var hotels = new List<Hotel>
            {
                new Hotel { Name = "Hotel A", Latitude = -43.9509, Longitude = -34.4618 },
                new Hotel { Name = "Hotel B", Latitude = 40.7128, Longitude = -74.0060 },
                new Hotel { Name = "Hotel C", Latitude = 34.0522, Longitude = -118.2437 },
                new Hotel { Name = "Hotel D", Latitude = -25.2744, Longitude = 133.7751 }
            };

            // Initialize users
            await _userService.GetUsersAsync();

            // Act
            var nearestUsers = (await _userService.GetNearestUsersAsync(hotels)).ToList();

            // Assert
            Assert.NotNull(nearestUsers);
            Assert.Equal(hotels.Count, nearestUsers.Count);
            
            // Verify each nearest user is as expected
            Assert.Equal(1, nearestUsers[0].Id); // User 1 should be closest to Hotel A
            Assert.Equal(2, nearestUsers[1].Id); // User 2 should be closest to Hotel B
            Assert.Equal(3, nearestUsers[2].Id); // User 3 should be closest to Hotel C
            Assert.Equal(4, nearestUsers[3].Id); // User 4 should be closest to Hotel D
        }

        [Fact]
        public async Task AddUserAsync_ShouldAddUserAndAssignId()
        {
            // Arrange
            var newUser = new User
            {
                Name = "Test User",
                Email = "test@example.com"
            };

            // Initialize users
            await _userService.GetUsersAsync();

            // Act
            var addedUser = await _userService.AddUserAsync(newUser);
            var allUsers = await _userService.GetUsersAsync();

            // Assert
            Assert.NotNull(addedUser);
            Assert.True(addedUser.Id > 0);
            Assert.Equal(newUser.Name, addedUser.Name);
            Assert.Equal(newUser.Email, addedUser.Email);
            Assert.Contains(addedUser, allUsers);
        }

        [Fact]
        public async Task DeleteUserAsync_ShouldRemoveExistingUser()
        {
            // Arrange - Initialize users
            await _userService.GetUsersAsync();
            var userIdToDelete = 1;

            // Act
            var result = await _userService.DeleteUserAsync(userIdToDelete);
            var allUsers = await _userService.GetUsersAsync();

            // Assert
            Assert.True(result);
            Assert.DoesNotContain(allUsers, u => u.Id == userIdToDelete);
        }

        [Fact]
        public async Task DeleteUserAsync_ShouldReturnFalseForNonexistentUser()
        {
            // Arrange - Initialize users
            await _userService.GetUsersAsync();
            var nonExistentUserId = 999;

            // Act
            var result = await _userService.DeleteUserAsync(nonExistentUserId);

            // Assert
            Assert.False(result);
        }
    }
}
