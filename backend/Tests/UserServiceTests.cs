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
                    Username = "user1",
                    Email = "user1@example.com",
                    Phone = "1-123-456-7890",
                    Website = "user1.com",
                    Address = new Address
                    {
                        Street = "123 Test St",
                        Suite = "Apt 1",
                        City = "TestCity",
                        Zipcode = "12345",
                        Geo = new Geo { Lat = -34.4618, Lng = -58.5718 } // Near Hotel A
                    },
                    Company = new Company
                    {
                        Name = "Company 1",
                        CatchPhrase = "Test Phrase 1",
                        Bs = "Test Bs 1"
                    }
                },
                new User 
                {
                    Id = 2,
                    Name = "User 2", 
                    Username = "user2",
                    Email = "user2@example.com",
                    Phone = "1-234-567-8901",
                    Website = "user2.com",
                    Address = new Address
                    {
                        Street = "456 Test St",
                        Suite = "Apt 2",
                        City = "TestCity",
                        Zipcode = "23456",
                        Geo = new Geo { Lat = 40.7548, Lng = -73.9774 } // Near Hotel B
                    },
                    Company = new Company
                    {
                        Name = "Company 2",
                        CatchPhrase = "Test Phrase 2",
                        Bs = "Test Bs 2"
                    }
                },
                new User 
                {
                    Id = 3,
                    Name = "User 3",
                    Username = "user3",
                    Email = "user3@example.com",
                    Phone = "1-345-678-9012",
                    Website = "user3.com",
                    Address = new Address
                    {
                        Street = "789 Test St",
                        Suite = "Apt 3",
                        City = "TestCity",
                        Zipcode = "34567",
                        Geo = new Geo { Lat = 34.0194, Lng = -118.4108 } // Near Hotel C
                    },
                    Company = new Company
                    {
                        Name = "Company 3",
                        CatchPhrase = "Test Phrase 3",
                        Bs = "Test Bs 3"
                    }
                },
                new User 
                {
                    Id = 4,
                    Name = "User 4",
                    Username = "user4",
                    Email = "user4@example.com",
                    Phone = "1-456-789-0123",
                    Website = "user4.com",
                    Address = new Address
                    {
                        Street = "012 Test St",
                        Suite = "Apt 4",
                        City = "TestCity",
                        Zipcode = "45678",
                        Geo = new Geo { Lat = -25.3444, Lng = 131.0369 } // Near Hotel D
                    },
                    Company = new Company
                    {
                        Name = "Company 4",
                        CatchPhrase = "Test Phrase 4",
                        Bs = "Test Bs 4"
                    }
                },
                new User 
                {
                    Id = 5,
                    Name = "User 5",
                    Username = "user5",
                    Email = "user5@example.com",
                    Phone = "1-567-890-1234",
                    Website = "user5.com",
                    Address = new Address
                    {
                        Street = "345 Test St",
                        Suite = "Apt 5",
                        City = "TestCity",
                        Zipcode = "56789",
                        Geo = new Geo { Lat = 51.5074, Lng = -0.1278 } // Far from all hotels
                    },
                    Company = new Company
                    {
                        Name = "Company 5",
                        CatchPhrase = "Test Phrase 5",
                        Bs = "Test Bs 5"
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
                Username = "testuser",
                Email = "test@example.com",
                Phone = "1-234-567-8901",
                Website = "test.com",
                Address = new Address
                {
                    Street = "Test Street",
                    Suite = "Test Suite",
                    City = "Test City",
                    Zipcode = "12345",
                    Geo = new Geo { Lat = 0, Lng = 0 }
                },
                Company = new Company
                {
                    Name = "Test Company",
                    CatchPhrase = "Test Catch Phrase",
                    Bs = "Test Bs"
                }
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
