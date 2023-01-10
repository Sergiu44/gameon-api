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
        public UserService(ServiceDependencies serviceDependencies) : base(serviceDependencies)
        {
        }

        public async Task<CurrentUserDto> Login(LoginModel model)
        {
            var user = await _unitOfWork.Users.Get().FirstOrDefaultAsync(user => user.Email == model.Email);
            if (user == null)
            {
                return new CurrentUserDto { IsAuthenticated = false };
            }

            if (!user.Password.SequenceEqual(model.Password.HashPassword(user.Salt)))
            {
                return new CurrentUserDto { IsAuthenticated = false };
            }

            return new CurrentUserDto
            {
                Id = user.Id,
                FirstName = user.FirstName,
                LastName = user.LastName,
                IsAuthenticated = true,
                IsAdmin = user.IsAdmin
            };
        }

        public void RegisterNewUser(RegisterModel model)
        {
            var userSalt = Guid.NewGuid();

            var usersCount = _unitOfWork.Users.Get().Select(user => user.Id).OrderBy(user => user).LastOrDefault();

            var user = new User()
            {
                Id = usersCount+1,
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
