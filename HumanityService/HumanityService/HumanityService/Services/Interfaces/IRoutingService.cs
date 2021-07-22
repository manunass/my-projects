using HumanityService.DataContracts.CompositeDesignPattern;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HumanityService.Services.Interfaces
{
    public interface IRoutingService
    {
        Task<double> GetETA(Location delivererCoordinates, Location donorCoordinates, Location ngoCoordinates, string transportationType);
        Task<double> GetETA(Location volunteerCoordinates, Location ngoCoordinates, string transportationType);
    }
}
