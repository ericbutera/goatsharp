using System;

namespace GoatTower
{
    public class Goat
    {
        public string Name { get; set; } = "Baa";
        public int HitPoints { get; set; } = 1;
        public int Level { get; set; } = 1;

        public bool IsAlive
        { 
            get
            { 
                return HitPoints > 0;
            }
        }

        public Goat()
        {
            // calculate base hp based on level ?
        }

        public bool Attack(Person person)
        {
            person.HitPoints -= 1;
            return true;
        }
    }
}

