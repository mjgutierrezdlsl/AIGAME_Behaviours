using UnityEngine;

namespace AIGAME.FSM
{
    public abstract class PatrolState : State
    {
        protected CollectorStateController Controller { get; private set; }

        protected override void OnStateEnter()
        {
            base.OnStateEnter();
            Controller = StateController as CollectorStateController;
        }

        protected override void OnStateUpdate()
        {
            base.OnStateUpdate();
            var dangerCollider = Physics2D.OverlapCircle(Controller.Collector.Position, Controller.Alert.Range,
                Controller.Alert.DangerLayer);
            if (dangerCollider != null) Controller.ChangeState(Controller.Alert);
        }
    }
}