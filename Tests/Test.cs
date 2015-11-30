using NUnit.Framework;
using System;
using GoatTower;

namespace Tests
{
    [TestFixture()]
    public class Test
    {





        
        [Test()]
        public void TestCase()
        {
            var person = new Person
            {
                Name = "Moo"
            };
            Assert.AreEqual("Moo", person.Name);
        }

        [Test()]
        public void GoatAttack()
        {
            var person = new Person{
                HitPoints = 5
            };
            var goat = new Goat();
            Assert.True(goat.Attack(person));
            Assert.AreEqual(4, person.HitPoints);
            /*
             * arena
             * person attack goat
             * goat attack person
             * dodge, strength, hit points
             * level > threshold cant attack
             * 
             */
        }

        [Test()]
        public void PersonAttack()
        {
            var person = new Person();
            var goat = new Goat();
            Assert.True(goat.IsAlive);
            person.Attack(goat);
            Assert.AreEqual(0, goat.HitPoints);
            Assert.False(goat.IsAlive);
        }
    }
}

