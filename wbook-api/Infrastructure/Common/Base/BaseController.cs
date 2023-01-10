using Infrastructure.Common.DTOs;
using System.Web.Mvc;

namespace Infrastructure.Common.Base
{
    public class BaseController: Controller
    {
        protected readonly CurrentUserDto CurrentUser;

        public BaseController(ControllerDependencies controllerDependencies) : base()
        {
            CurrentUser = controllerDependencies.CurrentUser;
        }
    }
}
