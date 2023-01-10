using System.Threading.Tasks;
using AutoMapper;
using EnergyPlatformProgram.BusinessLogic.Models;
using EnergyPlatformProgram.Mappers;
using EnergyPlatformProgram.Repository.Implementations;
using EnergyPlatformProgram.Repository.Models;
using EnergyPlatformProject.Models;
using Microsoft.AspNetCore.SignalR;

namespace EnergyPlatformProgram.Hubs
{
    public interface IChatService
    {
        Task SendMessage(string message, string toUser, string fromUser);
        Task DisableType(string toUser, string fromUser);
        Task EnableType(string toUser, string fromUser);
    }
}
