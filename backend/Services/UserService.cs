using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using backend.Models;
using Newtonsoft.Json;

namespace backend.Services
{
    public interface IUserService
    {
        Task<IEnumerable<User>> GetUsersAsync();
        Task<User> UpdateUserAsync(User user);
        Task<bool> DeleteUserAsync(int id);
        Task<IEnumerable<User>> GetNearestUsersAsync(List<Hotel> hotels);
    }

    public class UserService : IUserService
    {
        private readonly HttpClient _httpClient;
        private List<User> _users;

        public UserService(HttpClient httpClient)
        {
            _httpClient = httpClient;
            _users = new List<User>();
        }

        public async Task<IEnumerable<User>> GetUsersAsync()
        {
            if (!_users.Any())
            {
                var response = await _httpClient.GetStringAsync("https://jsonplaceholder.typicode.com/users");
                _users = JsonConvert.DeserializeObject<List<User>>(response);
            }
            return _users;
        }

        public async Task<User> UpdateUserAsync(User user)
        {
            var existingUser = _users.FirstOrDefault(u => u.Id == user.Id);
            if (existingUser == null)
            {
                return null;
            }

            existingUser.Name = user.Name;
            existingUser.Username = user.Username;
            existingUser.Email = user.Email;
            existingUser.Address = user.Address;
            existingUser.Phone = user.Phone;
            existingUser.Website = user.Website;
            existingUser.Company = user.Company;

            return await Task.FromResult(existingUser);
        }

        public async Task<bool> DeleteUserAsync(int id)
        {
            var user = _users.FirstOrDefault(u => u.Id == id);
            if (user == null)
            {
                return false;
            }

            _users.Remove(user);
            return await Task.FromResult(true);
        }

        public async Task<IEnumerable<User>> GetNearestUsersAsync(List<Hotel> hotels)
        {
            var nearestUsers = new List<User>();

            foreach (var hotel in hotels)
            {
                User nearestUser = null;
                double nearestDistance = double.MaxValue;

                foreach (var user in _users)
                {
                    var distance = GetDistance(hotel.Latitude, hotel.Longitude, user.Address.Geo.Lat, user.Address.Geo.Lng);
                    if (distance < nearestDistance)
                    {
                        nearestDistance = distance;
                        nearestUser = user;
                    }
                }

                if (nearestUser != null)
                {
                    nearestUsers.Add(nearestUser);
                }
            }

            return await Task.FromResult(nearestUsers);
        }

        private double GetDistance(double lat1, double lon1, double lat2, double lon2)
        {
            var d1 = lat1 * (Math.PI / 180.0);
            var num1 = lon1 * (Math.PI / 180.0);
            var d2 = lat2 * (Math.PI / 180.0);
            var num2 = lon2 * (Math.PI / 180.0) - num1;
            var d3 = Math.Pow(Math.Sin((d2 - d1) / 2.0), 2.0) + Math.Cos(d1) * Math.Cos(d2) * Math.Pow(Math.Sin(num2 / 2.0), 2.0);

            return 6376500.0 * (2.0 * Math.Atan2(Math.Sqrt(d3), Math.Sqrt(1.0 - d3)));
        }
    }
}
