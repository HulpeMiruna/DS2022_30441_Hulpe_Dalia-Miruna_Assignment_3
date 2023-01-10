using System;
using System.ComponentModel.DataAnnotations;
using EnergyPlatform.Repository.Entitys;

namespace EnergyPlatformProject.Models
{
    public class ChatViewModel
    {
        public UserEntity User;
        public Guid Id;
        public string Message;
        public string FromUser;
        public string ToUser;
        public string CurrentUser;
    }
}
