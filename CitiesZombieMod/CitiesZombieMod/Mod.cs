﻿using ICities;

namespace CitiesZombieMod
{
    public class Mod : IUserMod
    {
        public string Name
        {
            get
            {
                return "Zombie Mod v2";
            }
        }
        public string Description
        {
            get
            {
                return "Introduces the walking dead to your game.";
            }
        }
    }
}
