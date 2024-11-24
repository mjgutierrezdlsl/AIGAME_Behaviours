using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AIGAME.FSM
{
    [System.Serializable]
    public class SeekState : State
    {
        [field: SerializeField] public float Range { get; private set; } = 1f;
        [field: SerializeField] public LayerMask VisibleLayer { get; private set; }
        [SerializeField] private float _moveSpeed = 2f;
        private Collider2D _currentTarget;
        private CollectorStateController _controller;
        private Vector2 _moveDirection;
        protected override void OnStateEnter()
        {
            base.OnStateEnter();
            _controller = StateController as CollectorStateController;
            _currentTarget = Physics2D.OverlapCircle(_controller.Collector.Position, Range, VisibleLayer);
        }
        protected override void OnStateExit()
        {
            base.OnStateExit();
            _currentTarget = null;
        }
        protected override void OnStateUpdate()
        {
            base.OnStateUpdate();
            _moveDirection = (_currentTarget.transform.position - _controller.Collector.Position).normalized;
        }
        protected override void OnPhysicsUpdate()
        {
            base.OnPhysicsUpdate();
            _controller.Collector.Rigidbody.MovePosition((Vector2)_controller.Collector.Position + _moveDirection * _moveSpeed * Time.fixedDeltaTime);
        }
        public void OnTriggerEnter2D(Collider2D other)
        {
            if (_currentTarget == other)
            {
                // Pass the gameObject to the collector in order to be destroyed
                _controller.Collector.AddResource(other.gameObject);
                // Return to Wander state
                _controller.ChangeState(_controller.Wander);
            }
        }
    }
}
