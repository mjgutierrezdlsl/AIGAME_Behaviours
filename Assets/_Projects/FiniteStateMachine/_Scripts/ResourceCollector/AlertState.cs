using System;
using UnityEngine;

namespace AIGAME.FSM
{
    [Serializable]
    public class AlertState : State
    {
        [field: SerializeField] public float Range { get; private set; } = 4f;
        [field: SerializeField] public LayerMask DangerLayer { get; private set; }
        [SerializeField] private float _coolDownTime = 1f;

        private CollectorStateController _controller;
        private float _elapsedTime;

        private Collider2D dangerCollider;

        protected override void OnStateEnter()
        {
            base.OnStateEnter();
            _controller = StateController as CollectorStateController;
            _controller.Collector.Animator.SetBool("isMoving", false);
            _controller.Collector.Animator.SetTrigger("attack");
        }

        protected override void OnStateUpdate()
        {
            base.OnStateUpdate();
            dangerCollider = Physics2D.OverlapCircle(_controller.Collector.Position, Range, DangerLayer);
            if (dangerCollider == null)
            {
                if (_elapsedTime < _coolDownTime)
                {
                    _elapsedTime += Time.deltaTime;
                }
                else
                {
                    _elapsedTime = 0f;
                    _controller.ChangeState(_controller.Wander);
                }
            }
        }
    }
}