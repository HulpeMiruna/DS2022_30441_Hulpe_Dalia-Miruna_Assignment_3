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
    public class ChatService  : Hub, IChatService
    {
        private readonly IHubContext<ChatHub> _chatContext;
        public ChatService(IHubContext<ChatHub> chatContext)
        {
            _chatContext = chatContext;
        }

        public async Task SendMessage(string message, string toUser, string fromUser)
        {
            await _chatContext.Clients.All.SendAsync("ReceiveMessage", message, toUser, fromUser);
        }

        public async Task EnableType( string toUser, string fromUser)
        {
            await _chatContext.Clients.All.SendAsync("EnableType",  toUser, fromUser);
        }

        public async Task DisableType(string toUser, string fromUser)
        {
            await _chatContext.Clients.All.SendAsync("DisableType",  toUser, fromUser);
        }

    }
}
