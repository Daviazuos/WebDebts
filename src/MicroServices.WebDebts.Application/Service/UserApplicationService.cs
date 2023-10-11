using MicroServices.WebDebts.Application.Models;
using MicroServices.WebDebts.Domain.Interfaces.Repository;
using System;
using MicroServices.WebDebts.Application.Models.Mappers;
using System.Threading.Tasks;
using MicroServices.WebDebts.Application.Models.AuthModels;
using MicroServices.WebDebts.Domain.Models;

namespace MicroServices.WebDebts.Application.Service
{
    public interface IUserApplicationService
    {
        Task CreateUser(UserAppModel userAppModel);
        Task<LoginAppModelReponse> Login(LoginAppModelRequest loginAppModel);
    }

    public class UserApplicationService : IUserApplicationService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IUserRepository _userRepository;
        private readonly ITokenApplicationService _tokenApplicationService;

        public UserApplicationService(IUserRepository userRepository, ITokenApplicationService tokenApplicationService, IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
            _userRepository = userRepository;
            _tokenApplicationService = tokenApplicationService;
        }

        public async Task CreateUser(UserAppModel userAppModel)
        {
            //var userEntity = userAppModel.ToModel();

            var userEntity = new User
            {
                Id = Guid.NewGuid(),
                CreatedAt = DateTime.Now,
                Document = userAppModel.Document,
                ImageUrl = userAppModel.ImageUrl,
                Name = userAppModel.Name,
                Password = userAppModel.Password,
                Username = userAppModel.Username
            };

            await _userRepository.AddAsync(userEntity);
            await _unitOfWork.CommitAsync();
        }

        public async Task<LoginAppModelReponse> Login(LoginAppModelRequest loginAppModel)
        {
            var user = await _userRepository.FindUserByUserPasswordAsync(loginAppModel.Username, loginAppModel.Password);

            var token = _tokenApplicationService.GenerateToken(user);

            return new LoginAppModelReponse
            {
                Document = user.Document,
                Name = user.Name,
                Token = token,
                Username = user.Username,
                ImageUrl = ""
            };
        }
    }
}
