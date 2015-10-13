using System;

namespace CitiesZombieMod
{
    public class ZombieAI : PrefabAI
    {
        [NonSerialized]
        public ZombieInfo m_info;

        public virtual void InitializeAI()
        {
        }

        public virtual void ReleaseAI()
        {
        }
    }
}
