using UnityEngine;

namespace CitiesZombieMod
{
    class Zombie
    {
        public uint m_id;
        public string m_name;
        public Vector3 m_position;
        public Zombie(uint id, Vector3 position)
        {
            m_id = id;
            m_position = position;
            m_name = "Zombie " + id;          
        }
    }
}
