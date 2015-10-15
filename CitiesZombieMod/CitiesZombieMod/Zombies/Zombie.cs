using ColossalFramework;
using ColossalFramework.Math;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CitiesZombieMod
{
    public struct Zombie
    {
        public enum Flags
        {
            None = 0,
            Created = 1,
            Dead = 4,
            All = -1
        }
        public Zombie.Flags m_flags;

        public ushort m_instance;
        public byte m_health;
    

        public ZombieInfo getZombieInfo(uint zombieId)
        {
            if (this.m_instance != 0)
            {
                return Singleton<ZombieManager>.instance.m_instances.m_buffer[(int)this.m_instance].Info;
            }
            ItemClass.Service service = ItemClass.Service.Residential; // @todo figure out a way to set a logical ItemClass for a Zombie.
            Citizen.Gender gender = Citizen.GetGender(zombieId);
            Randomizer randomizer = new Randomizer(zombieId);
            return Singleton<ZombieManager>.instance.GetGroupZombieInfo(ref randomizer, service, gender, Citizen.SubCulture.Generic);
        }
    }
}
