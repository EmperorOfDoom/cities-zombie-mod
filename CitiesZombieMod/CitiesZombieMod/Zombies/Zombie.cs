namespace CitiesZombieMod
{
    class Zombie
    {
        public uint id;
        public string name;

        public Zombie(uint id)
        {
            this.id = id;
            name = "Zombie " + this.id;
        }
    }
}
