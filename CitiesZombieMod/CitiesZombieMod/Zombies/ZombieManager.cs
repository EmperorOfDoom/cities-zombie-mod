using ColossalFramework;
using ICities;
using System.Collections.Generic;
using UnityEngine;

namespace CitiesZombieMod
{
    /*
        The ZombieManager takes care of keeping track of all active zombies.
    */
    public class ZombieManager : ThreadingExtensionBase
    {
        public static ZombieManager Instance { get; private set; }
        private bool loadingLevel = false;
        private uint _last_assigned_zombie_id = 0;
        private Dictionary<uint, Zombie> _zombies;

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
            _zombies = new Dictionary<uint, Zombie>();
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
            _last_assigned_zombie_id += 1;
            Zombie zombie = new Zombie(_last_assigned_zombie_id, position);
            //    Logger.Log("Added zombie with id " + zombie.id + " to the zombie pool.");
            _zombies[zombie.m_id] = zombie;
        }    
    }
}
