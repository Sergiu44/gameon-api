using Infrastructure;
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

        public bool Test()
        {
            return _unitOfWork.Users.Get().Any();
        }
    }
}
