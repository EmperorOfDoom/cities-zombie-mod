using ColossalFramework;
using ICities;
using System.Collections.Generic;
using UnityEngine;

namespace CitiesZombieMod
{
     public class ZombieManagerThread : ThreadingExtensionBase
     {
         public static ZombieManagerThread Instance { get; private set; }
         private bool loadingLevel = false;
       //  private uint _last_assigned_zombie_id = 0;
         private Dictionary<uint, ZombieInstance> _zombies;
    
         public void OnLevelUnloading()
         {
             loadingLevel = true;
         }
    
         public void OnLevelLoaded(LoadMode mode)
         {
             loadingLevel = false;
         }
    
         public override void OnCreated(IThreading threading)
         {
             Instance = this;
             Logger.Log("Creating zombie pool.");
             _zombies = new Dictionary<uint, ZombieInstance>();
         }
    
         public override void OnReleased()
         {
         }
    
         public override void OnUpdate(float realTimeDelta, float simulationTimeDelta)
         {
             if (loadingLevel) return;
             Logger.Log("Current amount of zombies: " + _zombies.Values.Count); 
         }
    
    
         public void SpawnZombie(Vector3 position)
         {
           //  _last_assigned_zombie_id += 1;
           //  ZombieInstance zombie = new ZombieInstance(_last_assigned_zombie_id, position);
           //  //    Logger.Log("Added zombie with id " + zombie.id + " to the zombie pool.");
           //  _zombies[zombie.m_id] = zombie;
         }    
     }
}
