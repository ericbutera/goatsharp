using System;
using System.Collections.Generic;
using System.Linq;

// goofing around trying to make something like this:
// http://gamedevelopment.tutsplus.com/articles/how-to-build-a-jrpg-a-primer-for-game-developers--gamedev-6676#combat

namespace GoatTower
{
    public class Game
    {
        public Game() {}
    }

    #region [ entities ]
    public abstract class Entity {
        public bool IsPlayerControlled { get; set;} = false;
    }

    public class GoatEntity : Entity {
    }
    #endregion

    #region [ states ]
    /// <summary>
    /// Various available game states
    /// </summary>
    public enum States {
        MainMenu = 0,
        Battle = 1,
        Tick = 2,
        Execute = 3
    }
    public interface IState
    {
        //StateMachine gGameMode { get; set; }
        void Update(float elapsedTime);
        void Render();
        void OnEnter(params object[] optional);
        void OnExit();
    }

    public class EmptyState : IState
    {
        public void Update(float elapsedTime) {}// Nothing to update in the empty state.
        public void Render() {} // Nothing to render in the empty state
        public void OnEnter(params object[] optiona){}// No action to take when the state is entered
        public void OnExit() {}// No action to take when the state is exited
    }

    public class MainMenuState : IState {
        protected StateMachine gGameMode;
        public MainMenuState(StateMachine mode) {
            gGameMode = mode;
        }
        public void Update(float elapsedTime) {}

        public void Render() {
            Console.WriteLine("Please enter your name\n");
            // okay so where do we ask for input?
            var name = Console.ReadLine();
            Console.WriteLine("You'll be known as " + name);
            gGameMode.Change(States.Battle);
        }

        public void OnEnter(params object[] optional) {}
        public void OnExit() {}    
    }

    class BattleState : IState
    {
        List<IAction> mActions = new List<IAction>();
        List<Entity> mEntities = new List<Entity>();
        StateMachine mBattleStates = new StateMachine();

        public BattleState(StateMachine gGameMode)
        {
            mBattleStates.Add(States.Tick, new BattleTick(mBattleStates, mActions));
            // TODO mBattleStates.Add(States.Execute, new BattleExecute(mBattleStates, mActions));
        }

        public void OnEnter(params object[] optional)
        {
            mBattleStates.Change(States.Tick);

            //
            // Get a decision action for every entity in the action queue
            // The sort it so the quickest actions are the top
            //

            /*List<Entity> entities=null, */
            // FIGURE OUT A WAY TO GET THE BADDIES IN HERE!!!!
            //if (optional.
            //if (entities.Count > 0)
            //mEntities.AddRange(entities);
            //mEntities = params.entities;
            mEntities.Add(new GoatEntity());

            foreach(Entity e in mEntities)
            {
                if(e.IsPlayerControlled)
                {
                    var action = new AttackAction();
                    // TODO PlayerDecide action = new PlayerDecide(e, e.Speed());
                    //mActions.Push(action);
                    mActions.Add(action);
                }
                else
                {
                    var action = new BaaAction();
                    //TODO AIDecide action = new AIDecide(e, e.Speed());
                    //mActions.Push(action);
                    mActions.Add(action);
                }
            }

            mActions = mActions.OrderBy(t => t.TimeRemaining).ToList();
            //Sort(mActions, BattleState.SortByTime);
            // public static bool SortByTime(Action a, Action b) { return a.TimeRemaining > b.TimeRemaining; }
        }

        public void Update(float elapsedTime)
        {
            mBattleStates.Update(elapsedTime);
        }

        public void Render()
        {
            // Draw the scene, gui, characters, animations etc
            mBattleStates.Render();
        }

        public void OnExit() {}
    }

    class BattleTick : IState
    {
        StateMachine mStateMachine;
        List<IAction> mActions;

        public BattleTick(StateMachine stateMachine, List<IAction> actions) //: mStateMachine(stateMachine), mActions(action)
        {
            mStateMachine = stateMachine;
            mActions = actions;
        }

        // Things may happen in these functions but nothing we're interested in.
        public void OnEnter(params object[] optional) {}
        public void OnExit() {}
        public void Render() {}

        public void Update(float elapsedTime)
        {
            foreach(Action a in mActions)
            {
                a.Update(elapsedTime);
            }

            if (mActions.First().IsReady()) //if(mActions.Top().IsReady())
            {
                //var top = mActions.Pop();
                //var top = mActions.RemoveAt(0);
                var top = mActions.First();
                mActions.Remove(top);
                mStateMachine.Change(States.Execute, top);
            }
        }
    }
    #endregion

    #region[ actions ]
    public interface IAction
    {
        float TimeRemaining { get; set; }
        bool IsReady();
        void Update(float elapsedTime);
    }

    public abstract class Action : IAction {
        public float TimeRemaining { get ; set ;}

        public void Update(float elapsedTime) {
            TimeRemaining -= elapsedTime;
        }

        public bool IsReady() {
            return TimeRemaining <= 0;
        }
    }

    public class BaaAction : Action {}
    public class AttackAction : Action {}
    #endregion


    public class StateStack
    {
        Dictionary<string, IState> mStates = new Dictionary<string, IState>();
        Stack<IState> mStack = new Stack<IState>();

        public void Update(float elapsedTime)
        {
            IState top = mStack.Peek();
            top.Update(elapsedTime);
        }

        public void Render()
        {
            IState top = mStack.Peek();
            top.Render();
        }

        public void Push(String name)
        {
            IState state = mStates[name];
            mStack.Push(state);
        }

        public IState Pop()
        {
            return mStack.Pop();
        }
    }

    public class StateMachine
    {
        Dictionary<States, IState> mStates = new Dictionary<States, IState>();
        IState mCurrentState = new EmptyState();

        public void Update(float elapsedTime)
        {
            mCurrentState.Update(elapsedTime);
        }

        public void Render()
        {
            mCurrentState.Render();
        }

        public void Change(States stateName, params object[] optional)
        {
            mCurrentState.OnExit();
            mCurrentState = mStates[stateName];
            mCurrentState.OnEnter(optional);
        }

        public StateMachine Add(States name, IState state)
        {
            mStates[name] = state;
            return this;
        }
    }
}

