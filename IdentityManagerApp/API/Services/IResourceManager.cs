using System;
using API.Models;

namespace API.Services
{
    public interface IResourceManager
    {
        Task<List<Resource>> GetResources();
    }
}

