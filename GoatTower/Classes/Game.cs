using System;
using System.Collections.Generic;
using System.Linq;

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
    public interface IState
    {
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
        Dictionary<string, IState> mStates = new Dictionary<string, IState>();
        IState mCurrentState = new EmptyState();

        public void Update(float elapsedTime)
        {
            mCurrentState.Update(elapsedTime);
        }

        public void Render()
        {
            mCurrentState.Render();
        }

        public void Change(String stateName, params object[] optional)
        {
            mCurrentState.OnExit();
            mCurrentState = mStates[stateName];
            mCurrentState.OnEnter(optional);
        }

        public void Add(String name, IState state)
        {
            mStates[name] = state;
        }
    }

    //public class MenuState : IState {}

    public class RandomizeGameState : IState {
        public RandomizeGameState(StateMachine gGameMode) {
            
        }
        public void Update(float elapsedTime) {}
        public void Render() {}
        public void OnEnter(params object[] optional) {}
        public void OnExit() {}
    }

    class BattleState : IState
    {
        Stack<IAction> mActions = new Stack<IAction>();
        List<Entity> mEntities = new List<Entity>();

        StateMachine mBattleStates = new StateMachine();

        public BattleState()
        {
            mBattleStates.Add("tick", new BattleTick(mBattleStates, mActions));
            // TODO mBattleStates.Add("execute", new BattleExecute(mBattleStates, mActions));
        }

        public void OnEnter(params object[] optional)
        {
            mBattleStates.Change("tick");

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
                    mActions.Push(action);
                }
                else
                {
                    var action = new BaaAction();
                    //TODO AIDecide action = new AIDecide(e, e.Speed());
                    mActions.Push(action);
                }
            }
                
            mActions = mActions.OrderBy(t => t.TimeRemaining);
            //Sort(mActions, BattleState.SortByTime);
            /*
             * public static bool SortByTime(Action a, Action b)
                    {
                        return a.TimeRemaining > b.TimeRemaining;
                    }
             */
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
        Stack<IAction> mActions;

        public BattleTick(StateMachine stateMachine, Stack<IAction> actions) //: mStateMachine(stateMachine), mActions(action)
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
                var top = mActions.Pop();
                mStateMachine.Change("execute", top);
            }
        }
    }
}

