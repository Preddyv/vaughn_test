using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using backend.Models;
using backend.Services;

namespace backend.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UsersController : ControllerBase
    {
        private readonly IUserService _userService;
        private readonly IHotelBookingService _hotelBookingService;

        public UsersController(IUserService userService, IHotelBookingService hotelBookingService)
        {
            _userService = userService;
            _hotelBookingService = hotelBookingService;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<User>>> GetUsers()
        {
            var users = await _userService.GetUsersAsync();
            return Ok(users);
        }

        [HttpPost]
        public async Task<ActionResult<User>> AddUser([FromBody] User user)
        {
            var newUser = await _userService.AddUserAsync(user);
            return CreatedAtAction(nameof(GetUsers), new { id = newUser.Id }, newUser);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateUser(int id, [FromBody] User user)
        {
            if (id != user.Id)
            {
                return BadRequest();
            }

            var updatedUser = await _userService.UpdateUserAsync(user);
            if (updatedUser == null)
            {
                return NotFound();
            }

            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteUser(int id)
        {
            var deleted = await _userService.DeleteUserAsync(id);
            if (!deleted)
            {
                return NotFound();
            }

            return NoContent();
        }

        [HttpGet("nearest")]
        public async Task<ActionResult<IEnumerable<User>>> GetNearestUsers(
            [FromQuery] string[] names,
            [FromQuery] double[] latitudes,
            [FromQuery] double[] longitudes)
        {
            if (names == null || latitudes == null || longitudes == null ||
                names.Length != latitudes.Length || latitudes.Length != longitudes.Length)
            {
                return BadRequest("Hotel parameters must be provided in equal lengths");
            }

            var hotels = names.Select((name, i) => new Hotel 
            { 
                Name = name, 
                Latitude = latitudes[i], 
                Longitude = longitudes[i] 
            }).ToList();

            var nearestUsers = await _userService.GetNearestUsersAsync(hotels);
            return Ok(nearestUsers);
        }

        [HttpPost("book")]
        public async Task<IActionResult> BookHotel([FromBody] Hotel hotel)
        {
            var result = await _hotelBookingService.BookHotelAsync(hotel);
            if (!result)
            {
                return BadRequest("Hotel booking failed.");
            }

            return Ok("Hotel booked successfully.");
        }

        [HttpGet("hotels")]
        public async Task<ActionResult<IEnumerable<Hotel>>> GetHotels()
        {
            var hotels = await _hotelBookingService.GetHotelsAsync();
            return Ok(hotels);
        }
    }
}
