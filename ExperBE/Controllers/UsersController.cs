using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Threading.Tasks;
using ExperBE.Dtos.Users;
using ExperBE.Exceptions;
using ExperBE.Extensions.ClaimsPrincipal;
using ExperBE.Extensions.FluentValidation;
using ExperBE.Extensions.IQueryable;
using ExperBE.Repositories.Wrapper;
using FluentValidation;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ExperBE.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Produces("application/json")]
    public class UsersController : ControllerBase
    {
        private readonly IRepositoryWrapper _repository;
        public UsersController(IRepositoryWrapper repository)
        {
            _repository = repository;
        }

        [AllowAnonymous]
        [HttpPost("login")]
        [ProducesResponseType(typeof(UserTokenDto), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(BadRequestExceptionError), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<IActionResult> Login(UserLoginDto userLoginDto)
        {
            await userLoginDto.Validate().ThrowIfInvalid();
            var user = await _repository.User.GetAll()
                .ForEmail(userLoginDto.Email)
                .FirstOrDefaultAsync();

            if (user is null || !user.ValidatePassword(userLoginDto.Password))
            {
                return Unauthorized();
            }

            var tokenDto = user.GenerateJwtToken();
            return Ok(tokenDto);
        }

        [AllowAnonymous]
        [HttpPost("register")]
        [ProducesResponseType(typeof(UserTokenDto), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(BadRequestExceptionError), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Register(UserRegisterDto userRegisterDto)
        {
            await userRegisterDto.Validate().ThrowIfInvalid();
            var userExists = await _repository.User.GetAll().AsNoTracking()
                .ForEmail(userRegisterDto.Email)
                .AnyAsync();

            if (userExists)
            {
                throw new BadRequestException(nameof(userRegisterDto.Email), "Email is already in use");
            }

            var user = Models.Entities.User.CreateNew(userRegisterDto.Email, userRegisterDto.Password);
            _repository.User.Add(user);
            await _repository.SaveAsync();

            var tokenDto = user.GenerateJwtToken();
            return Ok(tokenDto);
        }

        [HttpGet("findByEmailStartsWith")]
        [ProducesResponseType(typeof(IEnumerable<UserDto>), StatusCodes.Status200OK)]
        public async Task<IActionResult> FindByEmailStartsWith(string email)
        {
            var currentUserEmail = User.GetEmail();
            var users = await _repository.User.GetAll().AsNoTracking()
                .Where(u => u.Email.StartsWith(email) && u.Email != currentUserEmail)
                .OrderBy(u => u.Email)
                .Skip(0)
                .Take(20)
                .ToListAsync();

            var dtos = users.Select(u => new UserDto(u));
            return Ok(dtos);
        }
    }
}
