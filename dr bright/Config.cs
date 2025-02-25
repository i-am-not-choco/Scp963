using System.Collections.Generic;
using Exiled.API.Enums;
using Exiled.API.Features.Spawn;
using Exiled.API.Interfaces;
using UnityEngine;

namespace dr_bright
{
    public class Config : IConfig
    {
        public bool IsEnabled { get; set; } = true;
        
        public bool Debug { get; set; }
        public  int OldRole_Setstheroletotheoldplayerrole { get; set; } = 0;
        public ushort ID { get; set; } = 1;
        public bool showasoldrole { get; set; } = true;
        public bool spawninwave { get; set; } = true;
        
        public uint limit { get; set; } = 1;

        public List<RoomSpawnPoint> SpawnPoints { get; set; } = new
        ([
            new RoomSpawnPoint
            {
                Chance = 100,
                Offset = Vector3.zero,
                Room = RoomType.HczArmory
            }

        
        ]);
        
          
        
        
        
        
    }
    
    
}