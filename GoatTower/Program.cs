using System;
using MongoDB.Driver;
using MongoDB.Bson;

// https://github.com/mongodb/mongo-csharp-driver/tree/master

namespace GoatTower
{
    public class Program
    {
        public static StateMachine gGameMode;
        protected static int frame = 1;

        public static void Main(string[] args)
        {
            Console.WriteLine("Hello World!");

            Run();

            /*
            //Baa();
            var person = new Person
            {
                    HitPoints = 5
            };
            var goat = new Goat();
            while(person.HitPoints > 0) 
            {
                Console.WriteLine("Player encounters a goat. Press any key to attack.");
                Console.ReadKey();
                goat.Attack(person);
                Console.WriteLine("Goat counters. Players hp is now "+ person.HitPoints.ToString());
            }
            Console.WriteLine("Player hp "+ person.HitPoints.ToString());
            Console.ReadKey();
            */
        }

        protected static void Run() {
            gGameMode = new StateMachine();

            // A state for each game mode
            gGameMode.Add(States.MainMenu, new MainMenuState(gGameMode))
                .Add(States.Battle, new BattleState(gGameMode));
            //.Add("localmap",   new LocalMapState(gGameMode))
            //.Add("worldmap",   new WorldMapState(gGameMode))
            //.Add("ingamemenu", new InGameMenuState(gGameMode))

            gGameMode.Change(States.MainMenu);
            while (true)
            {
                Update();
            }
        }

        // Main Game Update Loop
        public static void Update()
        {
            float elapsedTime = frame++; //GetElapsedFrameTime();
            gGameMode.Update(elapsedTime);
            gGameMode.Render();
        }

        /*
        protected static async void Baa() {
            var connectionString = "mongodb://localhost:27017";
            var dbName = "GoatTower";
            var client = new MongoClient(connectionString);
            var database = client.GetDatabase(dbName);

            var players = database.GetCollection<Person>("Players");

            await players.InsertOneAsync(new Person { Name = "Jack" });

            var list = await players.Find(x => x.Name == "Jack")
                .ToListAsync();

            foreach(var person in list)
            {
                Console.WriteLine(person.Name);
            }
        }
        */
    }

}
