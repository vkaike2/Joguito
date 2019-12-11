using Assets.Scripts.Components.Interactable;
using Assets.Scripts.Managers.Inputs;
using Assets.Scripts.Managers.PlayerState;
using Assets.Scripts.Utils;
using System.Collections;
using UnityEngine;

namespace Assets.Scripts.Components.MovementMouse
{
    [HelpURL("https://slimwiki.com/vkaike9/movementmousecomponent")]
    [RequireComponent(typeof(Animator))]
    [RequireComponent(typeof(Rigidbody2D))]
    [RequireComponent(typeof(InteractableComponent))]
    public class MovementMouseComponent : BaseComponent
    {
#pragma warning disable 0649
        private PlayerStateManager _playerState;
        private InputManager _inputManager;

        [Header("Configuration Fields")]
        [Tooltip("velocity of the gameObject")]
        [SerializeField]
        private float _velocity;
        [Range(0, 1)]
        [Tooltip("range where gameObject stop walk from click position")]
        [SerializeField]
        private float _stopRange;

        public bool Active => true;

        private Animator _animator;
        private Rigidbody2D _rigidBody2D;
        private InteractableComponent _interactableComponent;

        private MovementCursorComponentAnimatorVariables _animatorVariables;

        private Vector2 _mouseDirection;
        private Vector2 _mouseOnClickPosition;

        private Coroutine _MoveToExcatPosition;
#pragma warning restore 0649


        private void Start()
        {
            _playerState.SetMovementMouseComponent(this);
        }

        private void OnDestroy()
        {
            _playerState.RemoveMovementMouseComponent(this);
        }

        private void FixedUpdate() => this.MovementObject();

        public void ObjectGoTo(Vector2 position, float? stopRange)
        {
            if (_MoveToExcatPosition == null)
                _MoveToExcatPosition = StartCoroutine(MoveToExactPosition(position, stopRange));
        }

        IEnumerator MoveToExactPosition(Vector2 movePosition, float? stopRange)
        {
            Vector2 mouseDirection = (movePosition - (Vector2)transform.position).normalized;

            while (Vector2.Distance(movePosition, transform.position) >= stopRange)
            {
                _rigidBody2D.velocity = mouseDirection * (_velocity * Time.deltaTime);
                this.ChangePlayerSide(_rigidBody2D.velocity.x >= 0);

                yield return new WaitForFixedUpdate();
            }

            _rigidBody2D.velocity = Vector2.zero;
            _MoveToExcatPosition = null;
        }

        private void MovementObject()
        {
            _animator.SetBool(_animatorVariables.Running, _rigidBody2D.velocity != Vector2.zero);

            if (_inputManager.MouseLeftButton == 1 && !_playerState.PlayerCantMove)
            {
                _mouseOnClickPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                _mouseDirection = (_mouseOnClickPosition - (Vector2)transform.position).normalized;

                if (_MoveToExcatPosition != null)
                {
                    StopCoroutine(_MoveToExcatPosition);
                    _MoveToExcatPosition = null;
                    _interactableComponent.RemoveInteractableState();
                }
            }

            if (_MoveToExcatPosition != null)
            {
                _mouseOnClickPosition = transform.position;
                return;
            };

            if (Vector2.Distance(_mouseOnClickPosition, transform.position) >= _stopRange)
            {
                _rigidBody2D.velocity = _mouseDirection * (_velocity * Time.deltaTime);
                this.ChangePlayerSide(_rigidBody2D.velocity.x >= 0);
            }
            else
            {
                _rigidBody2D.velocity = Vector2.zero;
            }
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
            _animator = this.GetComponent<Animator>();
            _rigidBody2D = this.GetComponent<Rigidbody2D>();
            _interactableComponent = this.GetComponent<InteractableComponent>();
            _playerState = GameObject.FindObjectOfType<PlayerStateManager>();
            _inputManager = GameObject.FindObjectOfType<InputManager>();

            _animatorVariables = new MovementCursorComponentAnimatorVariables();

            if (_velocity == 0) _velocity = 5;
            if (_stopRange == 0) _stopRange = 0.2f;
        }
    }
}
