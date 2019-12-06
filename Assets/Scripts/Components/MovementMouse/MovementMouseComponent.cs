using Assets.Scripts.Managers.Inputs;
using Assets.Scripts.Managers.PlayerState;
using Assets.Scripts.Utils;
using UnityEngine;

namespace Assets.Scripts.Components.MovementMouse
{
    [RequireComponent(typeof(Animator))]
    [RequireComponent(typeof(Rigidbody2D))]
    public class MovementMouseComponent : BaseComponent
    {
#pragma warning disable 0649
        #region Required Fields
        [Header("Required Fields")]
        [SerializeField]
        private PlayerStateManager _playerState;
        [SerializeField]
        private InputManager _inputManager;
        #endregion

        #region Configuration Fields
        [Header("Configuration Fields")]
        [Tooltip("velocity of the gameObject")]
        [SerializeField]
        private float _velocity;
        [Range(0, 1)]
        [Tooltip("range where gameObject stop walk from click position")]
        [SerializeField]
        private float _stopRange;
        #endregion
#pragma warning restore 0649

        #region Unity Componnents
        private Animator _animator;
        private Rigidbody2D _rigidBody2D;
        #endregion

        private MovementCursorComponentAnimatorVariables _animatorVariables;

        private Vector2 _mouseDirection;
        private Vector2 _mouseOnClickPosition;

        private void FixedUpdate() => this.MovementObject();

        public void ObjectGoTo(Vector2 position)
        {
            this.MovementObject(position);
        }

        private void MovementObject(Vector2? movePosition = null)
        {
            if (movePosition != null)
            {
                _mouseOnClickPosition = movePosition.Value;
                _mouseDirection = (_mouseOnClickPosition - (Vector2)transform.position).normalized;
            }

            if (_inputManager.MouseLeftButton == 1 && !_playerState.PlayerCantMove)
            {
                _mouseOnClickPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                _mouseDirection = (_mouseOnClickPosition - (Vector2)transform.position).normalized;
            }

            if (Vector2.Distance(_mouseOnClickPosition, transform.position) >= _stopRange)
            {
                _rigidBody2D.velocity = _mouseDirection * (_velocity * Time.deltaTime);
                this.ChangePlayerSide(_rigidBody2D.velocity.x >= 0);
            }
            else
            {
                _rigidBody2D.velocity = Vector2.zero;
            }
            _animator.SetBool(_animatorVariables.Running, _rigidBody2D.velocity != Vector2.zero);
        }
        private void ChangePlayerSide(bool right)
        {
            this.transform.rotation = new Quaternion(0, right ? 0 : 180, 0, 0);
        }

        protected override void ValidateValues()
        {
            // => Required Fields
            if (_playerState == null) Debug.LogError(ValidatorUtils.ValidateNullAtGameObject(nameof(_playerState), this.gameObject.name));
            if (_inputManager == null) Debug.LogError(ValidatorUtils.ValidateNullAtGameObject(nameof(_inputManager), this.gameObject.name));
        }

        protected override void SetInitialValues()
        {
            // => Unity Components
            _animator = this.GetComponent<Animator>();
            _rigidBody2D = this.GetComponent<Rigidbody2D>();

            // => Animator Variables
            _animatorVariables = new MovementCursorComponentAnimatorVariables();

            // => Configuration fields
            if (_velocity == 0) _velocity = 5;
            if (_stopRange == 0) _stopRange = 0.2f;
        }
    }
}
