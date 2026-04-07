using System.Collections.Generic;
using Application.Utils.Patterns;
namespace Application.Core.FSM
{
    public abstract class BaseState : IState<GameState>
    {

        public abstract GameState StateType
        {
            get;
        }

        public abstract HashSet<GameState> NextStates
        {
            get;
        }
        
        public virtual void EnterState()
        {
        }
        public virtual void UpdateState()
        {
        }
        public virtual void ExitState()
        {
        }

    }
}