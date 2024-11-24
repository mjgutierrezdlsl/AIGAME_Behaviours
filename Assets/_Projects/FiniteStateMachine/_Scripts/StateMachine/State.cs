using System;

namespace AIGAME.FSM
{
    [Serializable]
    public abstract class State
    {
        protected StateController StateController;
        public void EnterState(StateController controller)
        {
            StateController = controller;
            OnStateEnter();
        }
        public void UpdateState()
        {
            OnStateUpdate();
        }
        public void PhysicsUpdateState()
        {
            OnPhysicsUpdate();
        }
        public void ExitState()
        {
            OnStateExit();
        }

        protected virtual void OnStateEnter() { }
        protected virtual void OnStateUpdate() { }
        protected virtual void OnPhysicsUpdate() { }
        protected virtual void OnStateExit() { }
    }
}
