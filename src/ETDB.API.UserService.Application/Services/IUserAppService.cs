using ETDB.API.UserService.Presentation.DTO;

namespace ETDB.API.UserService.Application.Services
{
    public interface IUserAppService
    {
        void Register(UserRegisterDTO userRegisterDTO);
    }
}
