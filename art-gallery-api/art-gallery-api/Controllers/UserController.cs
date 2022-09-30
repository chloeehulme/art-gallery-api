using System;
using Microsoft.AspNetCore.Mvc;
using art_gallery_api.Persistence;
using art_gallery_api.Models;
using Microsoft.AspNetCore.Authorization;

namespace art_gallery_api.Controllers
{
    [ApiController]
    [Route("api/art-gallery/users")]
    public class UsersController : ControllerBase
    {
        private readonly IUserDataAccesss _userRepo;
        public UsersController(IUserDataAccesss userRepo)
        {
            _userRepo = userRepo;
        }

        /// <summary>
        /// Gets all users.
        /// </summary>
        /// <returns> An array of all users. </returns>
        /// <response code="200"> Returns an array of all users, or an empty
        /// array if there are currently none stored. </response>
        [ProducesResponseType(StatusCodes.Status200OK)]
        [HttpGet(), Authorize(Policy = "AdminOnly")]
        public IEnumerable<User> GetUsers() =>
        _userRepo.GetUsers().ToArray();

        /// <summary>
        /// Gets all 'admin' users.
        /// </summary>
        /// <returns> An array of 'admin' users. </returns>
        /// <response code="200"> Returns an array of all 'admin' users,
        /// or an empty array if there are currently none stored. </response>
        [ProducesResponseType(StatusCodes.Status200OK)]
        [HttpGet("admin"), Authorize(Policy = "AdminOnly")]
        public IEnumerable<User> GetAdminUsers() =>
        _userRepo.GetAdminUsers().ToArray();

        /// <summary>
        /// Gets a user with the specified id.
        /// </summary>
        /// <param name="id"> The id for the user we are looking to return (from the request URL). </param>
        /// <returns> A user with the specified id. </returns>
        /// <response code="200"> Returns the user with the specified id. </response>
        /// <response code="404"> If the user retrieved at the specified id is null,
        /// ie. the user does not exist. </response>
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [HttpGet("{id}", Name = "GetUser"), Authorize(Policy = "AdminOnly")]
        public IActionResult GetUserById(int id)
        {
            User? user = _userRepo.GetUserById(id);
            if (user is null) return NotFound();
            else return Ok(user);
        }

        /// <summary>
        /// Creates a new user.
        /// </summary>
        /// <param name="newUser"> A new user (from the HTTP request body). </param>
        /// <returns> A newly created user. </returns>
        /// <remarks>
        /// Sample request body:
        ///
        ///     POST /api/users
        ///     {
        ///         "email": "example@gmail.com",
        ///         "firstName": "John,
        ///         "lastName": "Doe",
        ///         "passwordHash": "123456"
        ///     }
        ///
        /// </remarks>
        /// <response code="201"> Returns a newly created user. </response>
        /// <response code="400"> If the new user is null. </response>
        /// <response code="409"> If a user with the same email already exists. </response>
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status409Conflict)]
        [HttpPost()]
        [AllowAnonymous]
        public IActionResult AddUser(User newUser)
        {
            if (newUser is null) return BadRequest();

            if (_userRepo.GetUsers().Exists(x => x.Email == newUser.Email)) return Conflict();

            _userRepo.AddUser(newUser);

            return CreatedAtRoute("GetUser", new { id = newUser.Id }, newUser);
        }

        /// <summary>
        /// Updates an existing user.
        /// </summary>
        /// <param name="id"> The id for the user we are looking to update (from the request URL). </param>
        /// <param name="updatedUser"> Updated details for the user (from the HTTP request body). </param>
        /// <returns> No content. </returns>
        /// <remarks>
        /// Sample request body:
        ///
        ///     PUT /api/users/{id}
        ///     {
        ///         "firstName": "Mark",
        ///         "lastName": "Smith",
        ///         "role": "admin
        ///     }
        ///
        /// </remarks>
        /// <response code="204"> If the user is udpated successfully. </response>
        /// <response code="400"> If the request body is null. </response>
        /// <response code="404"> If the user to update is null. </response>
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [HttpPut("{id}"), Authorize(Policy = "UserOnly")]
        public IActionResult UpdateUser(int id, User updatedUser)
        {
            User? user = _userRepo.GetUserById(id);
            if (user is null) return NotFound();

            try
            {
                _userRepo.UpdateUser(id, updatedUser);
            }
            catch (Exception er) { return BadRequest(er); }

            return NoContent();
        }

        /// <summary>
        /// Deletes a user with the specified id.
        /// </summary>
        /// <param name="id"> The id for the user we are looking to delete (from the request URL). </param>
        /// <returns> No content. </returns>
        /// <response code="204"> Returns the user with the specified id. </response>
        /// <response code="404"> If the user to delete is null. </response>
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [HttpDelete("{id}"), Authorize(Policy = "UserOnly")]
        public IActionResult DeleteUser(int id)
        {
            User? user = _userRepo.GetUserById(id);
            if (user is null) return NotFound();

            _userRepo.DeleteUser(id);
            return NoContent();
        }

        /// <summary>
        /// Patches an existing user.
        /// </summary>
        /// <param name="id"> The id for the user we are looking to update (from the request URL). </param>
        /// <param name="updatedLogin"> Updated login details for the user (from the HTTP request body). </param>
        /// <returns> No content. </returns>
        /// <remarks>
        /// Sample request body:
        ///
        ///     PATCH /api/users/{id}
        ///     {
        ///         "email": "example@yahoo.com",
        ///         "password": "123456"
        ///     }
        ///
        /// </remarks>
        /// <response code="204"> If the user is udpated successfully. </response>
        /// <response code="400"> If the request body is null. </response>
        /// <response code="404"> If the user to update is null. </response>
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [HttpPatch("{id}"), Authorize(Policy = "UserOnly")]
        public IActionResult PatchUser(int id, Login updatedLogin)
        {
            User? user = _userRepo.GetUserById(id);
            if (user is null) return NotFound();

            try
            {
                _userRepo.PatchUser(id, updatedLogin);
            }
            catch (Exception er) { return BadRequest(er); }

            return NoContent();
        }
    }
}

