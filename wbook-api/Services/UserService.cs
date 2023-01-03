using Infrastructure;
using Infrastructure.Common.DTOs;
using Infrastructure.Common.Utils;
using Infrastructure.Entities;
using Infrastructure.Models.User;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services
{
    public class UserService : BaseService
    {
        public UserService(UnitOfWork unitOfWork) : base(unitOfWork)
        {
        }

        public async Task<CurrentUserDto> Login(LoginModel model)
        {
            var user = await _unitOfWork.Users.Get().FirstOrDefaultAsync(user => user.Email == model.Email);
            if (user == null)
            {
                return new CurrentUserDto { Authenticated = false };
            }

            if(user.Deleted == true)
            {
                return new CurrentUserDto { Disabled = true };
            }

            if (!user.Password.SequenceEqual(model.Password.HashPassword(user.Salt)))
            {
                return new CurrentUserDto { Authenticated = false };
            }

            return new CurrentUserDto
            {
                Id = user.Id,
                Email = user.Email,
                FirstName = user.FirstName,
                LastName = user.LastName,
                Authenticated = true,
                Disabled = user.Deleted || false,
                Admin = user.IsAdmin
            };
        }

        public void RegisterNewUser(RegisterModel model)
        {
            var userSalt = Guid.NewGuid();

            var user = new User()
            {
                Id = Guid.NewGuid(),
                Salt = Guid.NewGuid(),
                Email = model.Email,
                FirstName = model.FirstName,
                LastName = model.LastName,
                DateOfBirth = (DateTime)model.DateOfBirth,
                Phone = model.PhoneNumber ?? "",
                Type = model.Type ?? "",
            };

            user.Password = model.Password.HashPassword(user.Salt);
            _unitOfWork.Users.Insert(user);
            _unitOfWork.SaveChanges();
        }
    }
}
