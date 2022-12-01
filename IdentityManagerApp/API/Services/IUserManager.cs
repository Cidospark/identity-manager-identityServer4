using System;
using API.Models;

namespace API.Services
{
    public interface IUserManager
    {
        Task<List<User>> GetUsers();
    }
}

