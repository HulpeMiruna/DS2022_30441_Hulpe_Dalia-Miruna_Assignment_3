using AutoMapper;
using EnergyPlatform.Repository.Entitys;
using EnergyPlatformProgram.BusinessLogic.Constants;
using EnergyPlatformProgram.BusinessLogic.Interfaces;
using EnergyPlatformProgram.Hubs;
using EnergyPlatformProject.Models;
using Grpc.Net.Client;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace EnergyPlatformProject.Controllers
{
    public class AccountController : Controller
    {

        public readonly UserManager<UserEntity> _userManager;
        public readonly IMapper _mapper;
        private readonly IDeviceLogic _deviceLogic;
        private readonly IUserLogic _userLogic;
        private readonly IChatService _chatService;

        public AccountController(UserManager<UserEntity> userManager, IMapper mapper, IDeviceLogic deviceLogic, IUserLogic userLogic, IChatService chatService)
        {
            _userManager = userManager;
            _mapper = mapper;
            _deviceLogic = deviceLogic;
            _userLogic = userLogic;
            _chatService = chatService;
        }

        [Authorize(Policy = RoleConstants.UserRequirement)]
        [HttpGet]
        public async Task<IActionResult> UserPortal()
        {
            var currentUser = await _userManager.FindByNameAsync(User.Identity.Name);
            var devices = await _deviceLogic.GetByOwnerIdAsync(currentUser.Id);

            var model = new UserPortalViewModel()
            {
                Devices = devices.Select(d => _mapper.Map<DeviceViewModel>(d)).ToList(),
                Date = DateTime.UtcNow
            };

            return View(model);
        }

        [Authorize(Policy = RoleConstants.AdminRequirement)]
        [HttpGet]
        public async Task<IActionResult> AdminPortal()
        {
            var users = await _userManager.GetUsersInRoleAsync(RoleConstants.UserRole);

            var devicesModel = await _deviceLogic.GetAllAsync();
            var devices = devicesModel.Select(d => _mapper.Map<DeviceViewModel>(d)).ToList();

            var adminViewModel = new AdminViewModel();
            adminViewModel.Users = users;
            adminViewModel.Devices = devices;

            return View(adminViewModel);
        }


        [Authorize(Policy = RoleConstants.AdminRequirement)]
        [HttpGet]
        public async Task<IActionResult> DeleteUser(string email)
        {
            var user = await _userManager.FindByEmailAsync(email);
            await _userLogic.RemoveWithDevices(user);

            return RedirectToAction(nameof(AdminPortal));
        }

        [Authorize(Policy = RoleConstants.AdminRequirement)]
        [HttpGet]
        public async Task<IActionResult> UpdateUser(string email)
        {
            var user = await _userManager.FindByEmailAsync(email);
            var updateUser = new UserViewModel();
            updateUser.Email = email;
            updateUser.FirstName = user.FirstName;
            updateUser.LastName = user.LastName;

            return View(updateUser);
        }

        [Authorize(Policy = RoleConstants.AdminRequirement)]
        [HttpPost]
        public async Task<IActionResult> UpdateUser(UserViewModel updateUser)
        {
            var user = await _userManager.FindByEmailAsync(updateUser.Email);
            user.FirstName = updateUser.FirstName;
            user.LastName = updateUser.LastName;
            await _userManager.UpdateAsync(user);

            return RedirectToAction(nameof(AdminPortal));
        }

        [Authorize(Policy = RoleConstants.AdminRequirement)]
        [HttpGet]
        public IActionResult AddUser()
        {
            return View();
        }

        [Authorize(Policy = RoleConstants.AdminRequirement)]
        [HttpPost]
        public async Task<IActionResult> AddUser(UserViewModel userData)
        {
            var user = new UserEntity { UserName = userData.Email, Email = userData.Email, FirstName = userData.FirstName, LastName = userData.LastName};
            var result = await _userManager.CreateAsync(user, userData.Password);
            await _userManager.AddToRoleAsync(user, RoleConstants.UserRole);

            return RedirectToAction(nameof(AdminPortal));
        }

        [HttpGet]
        public async Task<IActionResult> Chat(Guid id)
        {
            var channel = GrpcChannel.ForAddress("https://localhost:5002");
            var client = new Greeter.GreeterClient(channel);

            var response = await client.SendMessageAsync(new MessageRequest { Id = "id", Message = "message" });
            var user = await _userManager.FindByIdAsync(id.ToString());

            var currentUser = User.Identity.Name;

            var currentUserComplete = await _userManager.FindByEmailAsync(currentUser);
            
            var model = new ChatViewModel()
            {
                Id = id,
                User = user,
                FromUser = currentUserComplete.Id.ToString(),
                ToUser = user.Id.ToString(),
                CurrentUser = currentUserComplete.Id.ToString()
            };

            return View("_ChatPage", model);
        }

        [HttpPost]
        public async Task<IActionResult> Chat(string message, string toUser, string fromUser)
        {
            if (message == "Enable")
            {
                await _chatService.EnableType(toUser, fromUser);
                return Ok();
            }

            if (message == "Disable")
            {
                await _chatService.DisableType(toUser, fromUser);
                return Ok();
            }

            await _chatService.SendMessage(message, toUser, fromUser);
            return Ok();
        }
    }
}
