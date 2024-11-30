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
        [SerializeField] private float _moveSpeed = 1f;

        private CollectorStateController _controller;
        private float _elapsedTime;

        private Collider2D dangerCollider;

        private Vector2 _moveDirection;
        private bool _isFacingLeft;
        
        protected override void OnStateEnter()
        {
            base.OnStateEnter();
            _controller = StateController as CollectorStateController;
            _controller.Collector.Animator.SetBool("isMoving", false);
        }

        protected override void OnStateUpdate()
        {
            base.OnStateUpdate();
            dangerCollider = Physics2D.OverlapCircle(_controller.Collector.Position, Range, DangerLayer);
            _controller.Collector.Animator.SetBool("isMoving", _moveDirection != Vector2.zero);
            if (_moveDirection.x < 0)
                _isFacingLeft = true;
            else if (_moveDirection.x > 0) _isFacingLeft = false;

            _controller.Collector.SpriteRenderer.flipX = _isFacingLeft;
            if (dangerCollider != null)
            {
                if (_elapsedTime < _coolDownTime)
                {
                    _elapsedTime += Time.deltaTime;
                    _moveDirection = _controller.Collector.Position - dangerCollider.transform.position;
                    _moveDirection.Normalize();
                }
                else
                {
                    _elapsedTime = 0f;
                    _moveDirection = Vector2.zero;
                }
            }
            else
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

        protected override void OnPhysicsUpdate()
        {
            base.OnPhysicsUpdate();
            _controller.Collector.Rigidbody.MovePosition((Vector2)_controller.Collector.Position +
                                                        _moveDirection * (_moveSpeed * Time.fixedDeltaTime));
        }
    }
}