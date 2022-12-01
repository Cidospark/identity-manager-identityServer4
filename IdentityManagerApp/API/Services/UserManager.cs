using System;
using API.Models;
using DataAccess.Data;
using Microsoft.EntityFrameworkCore;

namespace API.Services
{
    public class UserManager : IUserManager
    {
        private readonly ApplicationDBContext db;

        public UserManager(ApplicationDBContext db)
        {
            this.db = db;
        }

        public async Task<List<User>> GetUsers()
        {
            return await db.Users.Select(x => new User
            {
                Id = x.Id,
                LastName = x.LastName,
                FirstName = x.FirstName,
                Email = x.Email,
                PhoneNumber = x.PhoneNumber,
                Address = x.Address

            }).ToListAsync();
        }
    }
}

