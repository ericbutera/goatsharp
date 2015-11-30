using System;
using MongoDB.Bson;

namespace GoatTower
{
    public class Person
    {
        public ObjectId Id { get; set; }
        public string Name { get; set; } = "Meat";
        public int HitPoints { get; set;} = 5;
        public int Level { get; set; } = 1;

        public bool Attack(Goat goat) 
        {
            goat.HitPoints -= 1;
            return true;
        }
    }
}

