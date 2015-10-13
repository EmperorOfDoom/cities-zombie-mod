using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CitiesZombieMod.Zombies
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
        public ushort m_instance;
        public byte m_health;
    }
}
