using HumanityService.DataContracts.CompositeDesignPattern;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HumanityService.Stores.Interfaces
{
    public interface ILocationStore
    {
        Task AddLocation(string username, Location location);
        Task<Location> GetLocation(string username);
        Task UpdateLocation(string username, Location location);
        Task DeleteLocation(string username);
    }
}
