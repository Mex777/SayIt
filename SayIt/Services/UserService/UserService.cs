using AutoMapper;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using SayIt.Helpers;
using SayIt.Models.Tables;
using SayIt.Repositories.GenericRepository;
using SayIt.Repositories.UserRepository;

namespace SayIt.Services.UserService;

public class UserService : IUserService
{
    private readonly IUserRepository _userRepo;
    private readonly IMapper _mapper;
    public UserService(IUserRepository repo, IMapper mapper)
    {
        _userRepo = repo;
        _mapper = mapper;
    }
    
    public List<User> GetAllUsers()
    {
        return _userRepo.GetAll();
    }

    public UserDTO GetUserDtoByName(string name)
    {
        return _mapper.Map<UserDTO>(_userRepo.FindUserByName(name));
    }

    public User AddUser(UserDTO user)
    {
        var newUser = new User
            {
                 Id = Guid.NewGuid(),
                 Username = user.Username,
                 DateCreated = new DateTime(),
                 DateModified = new DateTime(),
                 Role = user.Role,
                 Password = PasswordHasher.HashPassword(user.Password)
            };
        
        _userRepo.Create(newUser);
        _userRepo.Save();

        return newUser;
    }

    public bool Login(UserDTO user)
    {
        var dbUser = _userRepo.FindUserByName(user.Username);
        return PasswordHasher.VerifyPassword(user.Password, dbUser.Password);
    }

    public void DeleteUserByName(string name)
    {
        var usr = _userRepo.FindUserByName(name);
        _userRepo.Delete(usr);
        _userRepo.Save();
    }

    public UserDTO UpdateUserByName(string name, UserDTO updated)
    {
        var usr = _userRepo.FindUserByName(name);
        usr.Password = updated.Password;
        usr.Role = updated.Role;
        _userRepo.Update(usr);
        _userRepo.Save();
        return _mapper.Map<UserDTO>(usr);
    }
}