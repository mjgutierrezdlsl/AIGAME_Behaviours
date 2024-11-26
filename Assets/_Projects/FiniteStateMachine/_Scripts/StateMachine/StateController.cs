using UnityEngine;
using UnityEngine.Events;

namespace AIGAME.FSM
{
    public abstract class StateController : MonoBehaviour
    {
        protected State CurrentState;
        public delegate void StateChangedEvent(State currentState, State previousState = null);
        public event StateChangedEvent OnStateChanged;
        [SerializeField] private UnityEvent<State> StateChanged;

        protected virtual void Update()
        {
            CurrentState?.UpdateState();
        }

        protected virtual void FixedUpdate()
        {
            CurrentState?.PhysicsUpdateState();
        }

        public void ChangeState(State newState)
        {
            State previousState = null;
            // Exit the current state if it exists
            if (CurrentState != null)
            {
                CurrentState?.ExitState();
                previousState = CurrentState;
            }
            // Set the new state as the current state
            CurrentState = newState;
            // Enter the new state
            CurrentState.EnterState(this);
            // Send event that state has changed
            StateChanged?.Invoke(CurrentState);
            OnStateChanged?.Invoke(CurrentState, previousState);
        }
    }
}
