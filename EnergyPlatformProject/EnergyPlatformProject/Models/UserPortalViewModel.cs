using EnergyPlatform.Repository.Entitys;
using System;
using System.Collections.Generic;

namespace EnergyPlatformProject.Models
{
    public class UserPortalViewModel
    {
        public DateTime Date { get; set; }

        public List<DeviceViewModel> Devices { get; set; }

        public Guid AdminId { get; set; } = new Guid("E3B3943D-02C8-4F50-E9C2-08DABA791F1C");
    }
}
