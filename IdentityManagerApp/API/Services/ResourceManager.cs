using System;
using API.Models;
using DataAccess.Data;
using Microsoft.EntityFrameworkCore;

namespace API.Services
{
    public class ResourceManager : IResourceManager
    {
        private readonly ApplicationDBContext db;

        public ResourceManager(ApplicationDBContext db)
        {
            this.db = db;
        }

        public async Task<List<Resource>> GetResources()
        {
            return await db.Resources.Select(x => new Resource
            {
                Id = x.Id,
                State = x.State,
                Format = x.Format,
                Description = x.Description

            }).ToListAsync();
        }
    }
}

