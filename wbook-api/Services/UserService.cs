using Infrastructure;
using Infrastructure.Common.Exceptions;
using Infrastructure.Common.Utils;
using Infrastructure.Entities;
using Infrastructure.Models.User;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using System.Text;

namespace Services
{
    public class UserService : BaseService
    {
        public UserService(ServiceDependencies serviceDependencies) : base(serviceDependencies)
        {
        }

        public async Task<List<UserItemModel>> GetUsers(int id)
        {
            var usersList = new List<UserItemModel>();
            var listOfUsers = await _unitOfWork.Users.Get().OrderBy(x => x.Email).ToListAsync();

            if(listOfUsers == null)
            {
                return usersList;
            }

            foreach(var user in listOfUsers)
            {
                var userModel = new UserItemModel()
                {
                    Id = user.Id,
                    Email = user.Email,
                    FirstName = user.FirstName,
                    LastName = user.LastName,
                    IsAdmin = user.IsAdmin,
                };
                usersList.Add(userModel);
            }
            return usersList;
        }

        public async Task<User> GetCurrentUser(int id)
        {
            var user = await _unitOfWork.Users.Get().FirstOrDefaultAsync(u => u.Id == id);
            if(user == null)
            {
                throw new NotFound("User not found");
            }
            return user;
        }

        public void PutChangePasswordUser(int id, UserPasswordModel user)
        {
            var userToChange = _unitOfWork.Users.Get().FirstOrDefault(u => u.Id == id);
            byte[] password = Encoding.ASCII.GetBytes(user.CurrentPassword);
            byte[] newPassword = Encoding.ASCII.GetBytes(user.NewPassword);

            if (userToChange.Password.Take(user.CurrentPassword.Length) != password)

            {
                throw new Exception("The current password is not good");
            }
            if (user.NewPassword != null)
            {
                userToChange.Password = newPassword;
            }
            _unitOfWork.Users.Update(userToChange);
            _unitOfWork.SaveChanges();
        }
        public void PutCurrentUser(int id, UserPutModel user)
        {
            var userToChange = _unitOfWork.Users.Get().FirstOrDefault(u => u.Id == id);
            if(userToChange == null)
            {
                throw new NotFound("User not found!");
            }

            if(user.FirstName != null)
            {
                userToChange.FirstName = user.FirstName;
            }
            if(user.LastName != null)
            {
                userToChange.LastName = user.LastName;
            }
            if(user.Phone!= null)
            {
                userToChange.Phone = user.Phone;
            }
            if(user.Email != null)
            {
                userToChange.Email = user.Email;
            }
            if (user.Bio != null)
            {
                userToChange.Bio= user.Bio;
            }
            if (user.Type != null)
            {
                userToChange.Type = user.Type;
            }
            if (user.AddressLine1 != null)
            {
                userToChange.AddressLine1 = user.AddressLine1;
            }
            if (user.AddressLine2 != null)
            {
                userToChange.AddressLine2 = user.AddressLine1;
            }
            if (user.PostalCode != null)
            {
                userToChange.PostalCode = user.PostalCode;
            }
            if (user.County != null)
            {
                userToChange.County = user.County;
            }
            if (user.City != null)
            {
                userToChange.City = user.City;
            }
            if (user.DateOfBirth != null)
            {
                userToChange.DateOfBirth = (DateTime)user.DateOfBirth;
            }
            _unitOfWork.Users.Update(userToChange);
            _unitOfWork.SaveChanges();
        }

        public void DeleteUser(int id)
        {
            var user = _unitOfWork.Users.Get().FirstOrDefault(g => g.Id == id);
            if (user == null)
            {
                throw new NotFound("User not found");
            }
            var basketItems = _unitOfWork.BasketItems.Get().Where(bi => bi.IdUser == user.Id);
            foreach(var basketItem in basketItems)
            {
                _unitOfWork.BasketItems.Delete(basketItem);
            }
            var wishlistItems = _unitOfWork.WishlistItems.Get().Where(w => w.IdUser == user.Id);
            foreach(var wishlistItem in wishlistItems)
            {
                _unitOfWork.WishlistItems.Delete(wishlistItem);
            }

            _unitOfWork.Users.Delete(user);
            _unitOfWork.SaveChanges();
        }

        public async Task<UserModel> Login(LoginModel model)
        {
            var user = await _unitOfWork.Users.Get().FirstOrDefaultAsync(user => user.Email == model.Email);
            if (user == null)
            {
                return new UserModel { IsAuthenticated = false };
            }

            if (!user.Password.SequenceEqual(model.Password.HashPassword(user.Salt)))
            {
                return new UserModel { IsAuthenticated = false };
            }

            return new UserModel
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
