using AutoMapper;
using agency_portal_api.DTOs;
using agency_portal_api.DTOs.Enums;
using agency_portal_api.DTOs.ServiceDtos;
using agency_portal_api.Entities;
using Microsoft.EntityFrameworkCore;
using agency_portal_api.Options;
using agency_portal_api.Utilities;

namespace agency_portal_api.Services
{
    public interface IUserService
    {
        Task<User> FindByEmail(string email);
        Task<CustomResponse<User>> CreateUser(CreateUserDto model, string roleName, CancellationToken token);
        Task<CustomResponse<User>> GetUserById(string id, CancellationToken token);
        IQueryable<User> ListAll();
        Task<CustomResponse<bool>> CheckPassword(User user, string password);
        Task<bool> Exists();
        Task<CustomResponse<User>> UpdateUser(User user, CancellationToken token);
    }
    public class UserService : IUserService
    {
        private readonly IMapper mapper;
        private readonly PasswordValidator passwordValidator;
        private readonly IDbTransactionService dbTransactionService;
        public readonly IRepository repository;

        public UserService(IConfiguration configuration, IMapper mapper, IRepository repository,
             PasswordValidator passwordValidator, IDbTransactionService dbTransactionService)
        {
            this.mapper = mapper;
            this.repository = repository;
            this.passwordValidator = passwordValidator;
            this.dbTransactionService = dbTransactionService;
        }


        public async Task<CustomResponse<User>> UpdateUser(User user, CancellationToken token)
        {
            if (user == null)
                return new CustomResponse<User>(ServiceResponses.BadRequest, null, "User cannot be null");

            if ((await Update(user, token)).Response == ServiceResponses.Success)
                return await GetUserById(user.Id, token);

            return new CustomResponse<User>(ServiceResponses.Failed, null, "Unable to update user");
        }


        public async Task<CustomResponse<User>> CreateUser(CreateUserDto model, string roleName, CancellationToken token)
        {
            if (model == null)
                return new CustomResponse<User>(ServiceResponses.BadRequest, null, "User cannot be null");

            if ((await FindByEmail(model.Email)) is not null)
                return new CustomResponse<User>(ServiceResponses.BadRequest, null, "User with email already exists");

            if (await ListAll().FirstOrDefaultAsync(c => c.PhoneNumber == model.PhoneNumber) is not null)
                return new CustomResponse<User>(ServiceResponses.BadRequest, null, "User with phone number already exists");

            if (passwordValidator.ValidatePassword(model.Password) == false)
                return new CustomResponse<User>(ServiceResponses.BadRequest, null, $"Password validation error");

            var user = mapper.Map<User>(model);
            user.UserName = user.UserName ?? user.Email;
            user.RoleName = roleName ?? "Admin";

            var createResult = await Create(user, model.Password, token);

            if (createResult.Response == ServiceResponses.Success)
            {
                return await GetUserById(user.Id, token);
            }

            return new CustomResponse<User>(ServiceResponses.Failed, null, $"Unable to create user: {string.Join(" ", createResult.Message)}");

        }

        public async Task<CustomResponse<User>> GetUserById(string id, CancellationToken token)
        {
            var user = await ListAll()
                .FirstOrDefaultAsync(c => c.Id == id, token);

            if (user is null)
                return new CustomResponse<User>(ServiceResponses.NotFound, null, "User Not Found");

            return new CustomResponse<User>(ServiceResponses.Success,user, null);
        }

        public IQueryable<User> ListAll()
        {
            return repository.ListAll<User>();
        }


        public async Task<User> FindByEmail(string email)
        {
            var normalizedEmail = email.ToUpper();
            return await ListAll()
                .FirstOrDefaultAsync(c => c.NormalizedEmail == normalizedEmail);
        }

        public async Task<User> FindByName(string username)
        {
            var normalizedUsername = username.ToUpper();
            return await ListAll().FirstOrDefaultAsync(c => c.NormalizedUserName == normalizedUsername);
        }

        public async Task<User> FindById(string id)
        {
            return await ListAll().FirstOrDefaultAsync(c => c.Id == id);
        }

        private async Task<CustomResponse<bool>> Create(User user, string password, CancellationToken token)
        {
            if (user == null)
                return new CustomResponse<bool>(ServiceResponses.BadRequest, "Value cannot be null");

            var salt = PasswordUtil.GenerateSalt(); 
            var hash = PasswordUtil.CreateHash(password, salt);

            user.PasswordHashed = hash;
            user.PasswordByte = salt;
            user.NormalizedEmail = user.Email.ToUpper();
            user.NormalizedUserName = user.UserName.ToUpper();

            var response = await repository.AddAsync(user, token);
            if (response)
            {
                return new CustomResponse<bool>() { Data = response, Response = ServiceResponses.Success };
            }

            return new CustomResponse<bool>(ServiceResponses.Failed, "Unable to create");
        }

        private async Task<CustomResponse<bool>> Update(User user, CancellationToken token)
        {
            if (user == null)
            {
                return new CustomResponse<bool>(ServiceResponses.BadRequest, "Value cannot be null");
            }

            var response = await repository.ModifyAsync(user, token);
            if (response)
            {
                return new CustomResponse<bool>() { Data = response, Response = ServiceResponses.Success };
            }

            return new CustomResponse<bool>(ServiceResponses.Failed, "Unable to update");
        }

        private async Task<CustomResponse<bool>> Delete(User user, CancellationToken token)
        {
            if (user == null)
            {
                return new CustomResponse<bool>(ServiceResponses.BadRequest, "Value cannot be null");
            }

            var response = await repository.DeleteAsync(user, token);
            if (response)
            {
                return new CustomResponse<bool>() { Data = response, Response = ServiceResponses.Success };
            }

            return new CustomResponse<bool>(ServiceResponses.Failed, "Unable to delete"); ;
        }

        public async Task<CustomResponse<bool>> CheckPassword(User user, string password)
        {
            if (user == null)
            {
                return new CustomResponse<bool>(ServiceResponses.BadRequest, "Value cannot be null");
            }

            if (PasswordUtil.VerifyHash(password, user.PasswordByte, user.PasswordHashed))
            {
                return new CustomResponse<bool>()
                {
                    Response = ServiceResponses.Success,
                    Data = true
                };
            }

            return new CustomResponse<bool>()
            {
                Response = ServiceResponses.Failed,
                Message = "Login failed for user"
            };
        }

        public async Task<bool> Exists()
        {
            return await ListAll().AnyAsync();
        }
    }
}
