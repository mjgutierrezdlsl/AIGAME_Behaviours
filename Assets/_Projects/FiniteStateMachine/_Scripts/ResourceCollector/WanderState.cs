using UnityEngine;

namespace AIGAME.FSM
{
    [System.Serializable]
    public class WanderState : State
    {
        [field: SerializeField] public float Range { get; private set; }
        [SerializeField] private float _moveSpeed = 5f;
        [SerializeField] private float _moveInterval = 1f;
        [SerializeField] private float _distanceThreshold = 0.3f;
        private Vector2 _targetPosition;
        private Vector2 _moveDirection;
        private CollectorStateController _controller;
        private float _elapsedTime;
        private bool _isFacingLeft;
        protected override void OnStateEnter()
        {
            base.OnStateEnter();
            _controller = StateController as CollectorStateController;
            _targetPosition = _controller.Collector.Position + Random.insideUnitSphere * Range;
        }
        protected override void OnStateUpdate()
        {
            base.OnStateUpdate();

            _controller.Collector.Animator.SetBool("isMoving", _moveDirection != Vector2.zero);

            _moveDirection = (_targetPosition - (Vector2)_controller.Collector.Position).normalized;

            if (_moveDirection.x < 0)
            {
                _isFacingLeft = true;
            }
            else if (_moveDirection.x > 0)
            {
                _isFacingLeft = false;
            }
            _controller.Collector.SpriteRenderer.flipX = _isFacingLeft;

            if (Vector2.Distance(_controller.Collector.Position, _targetPosition) < _distanceThreshold)
            {
                if (_elapsedTime < _moveInterval)
                {
                    _elapsedTime += Time.deltaTime;
                    _moveDirection = Vector2.zero;
                }
                else
                {
                    _elapsedTime = 0f;
                    _targetPosition = _controller.Collector.Position + Random.insideUnitSphere * Range;
                }
            }

            if (Physics2D.OverlapCircle(_controller.Collector.Position, _controller.Seek.Range, _controller.Seek.VisibleLayer))
            {
                _controller.ChangeState(_controller.Seek);
            }
        }
        protected override void OnPhysicsUpdate()
        {
            base.OnPhysicsUpdate();
            _controller.Collector.Rigidbody.MovePosition((Vector2)_controller.Collector.Position + _moveDirection * _moveSpeed * Time.fixedDeltaTime);
        }
    }
}
