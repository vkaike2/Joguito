using Assets.Scripts.Components.Combat;
using Assets.Scripts.Components.Interactable;
using Assets.Scripts.Managers.Inputs;
using Assets.Scripts.Managers.PlayerState;
using Assets.Scripts.Utils;
using System;
using System.Collections;
using UnityEngine;

namespace Assets.Scripts.Components.MovementMouse
{
    /// <summary>
    ///     Used to movement a object using the mouse
    /// </summary>
    [RequireComponent(typeof(Animator))]
    [RequireComponent(typeof(Rigidbody2D))]
    public class MovementMouseComponent : BaseComponent
    {
        #region SERIALIZABLE ATRIBUTES
        [Header("Configuration Fields")]
        [Tooltip("velocity of the gameObject")]
        [SerializeField]
        private float _velocity;
        [Range(0, 1)]
        [Tooltip("range where gameObject stop walk from click position")]
        [SerializeField]
        private float _stopRange;
        #endregion

        #region PRIVATE ATRIBUTES
        private PlayerStateManager _playerState;
        private InputManager _inputManager;
        private Animator _animator;
        private Rigidbody2D _rigidBody2D;
        private InteractableComponent _interactableComponent;
        private MovementCursorAnimatorVariables _animatorVariables;
        private Vector2 _mouseDirection;
        private Vector2 _mouseOnClickPosition;
        private Coroutine _moveToExcatPosition;
        private bool _internCanMove;
        private int? _instaceIdToCollider = null;
        #endregion

        #region PUBLIC METHODS
        public void ObjectGoToContinuous(Vector2 position, float? stopRange = null)
        {
            if (stopRange is null)
                stopRange = _stopRange;

            if (_moveToExcatPosition != null)
                StopCoroutine(_moveToExcatPosition);

            _moveToExcatPosition = StartCoroutine(MoveToExactPosition(position, stopRange));
        }

        public void ObjectGoTo(Vector2 position, float? stopRange = null)
        {
            if (stopRange is null)
                stopRange = _stopRange;

            if (_moveToExcatPosition == null)
                _moveToExcatPosition = StartCoroutine(MoveToExactPosition(position, stopRange));
        }
        public void ObjectGoTo(Vector2 position, int instanceID)
        {
            if (_moveToExcatPosition == null)
                _moveToExcatPosition = StartCoroutine(MoveToCombatComponent(position, instanceID));
        }

        public void Animator_CantMove()
        {
            _rigidBody2D.velocity = Vector2.zero;
            _internCanMove = false;
        }
        public void Animator_CanMove()
        {
            _internCanMove = true;
        }
        #endregion

        #region UNITY METHODS
        private void FixedUpdate()
        {
            //this.UpdateAnimator();

            if (!_internCanMove) return;

            this.StopPlayer();
            this.MovementObject();
        }
        #endregion

        #region COLLIDER METHODS
        private void OnTriggerEnter2D(Collider2D collision)
        {
            ManageTheStopCollision(collision);
        }

        private void OnTriggerStay2D(Collider2D collision)
        {
            ManageTheStopCollision(collision);
        }
        #endregion

        #region COROUTINES
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
            _mouseOnClickPosition = transform.position;
            _moveToExcatPosition = null;
        }
        IEnumerator MoveToCombatComponent(Vector2 movePosition, int instaceID)
        {
            Vector2 mouseDirection = (movePosition - (Vector2)transform.position).normalized;
            _instaceIdToCollider = instaceID;

            while (_instaceIdToCollider != null)
            {
                _rigidBody2D.velocity = mouseDirection * (_velocity * Time.deltaTime);
                this.ChangePlayerSide(_rigidBody2D.velocity.x >= 0);

                yield return new WaitForFixedUpdate();
            }

            _rigidBody2D.velocity = Vector2.zero;
            _mouseOnClickPosition = transform.position;
            _moveToExcatPosition = null;
        }
        #endregion

        #region PRIVATE METHODS
        private void UpdateAnimator()
        {
            _animator.SetBool(_animatorVariables.Running, _rigidBody2D.velocity != Vector2.zero);
        }
        private void MovementObject()
        {
            _animator.SetBool(_animatorVariables.Running, _rigidBody2D.velocity != Vector2.zero);

            if (!_isActive) return;
            if (_inputManager.MouseLeftButton == 1 && !_playerState.PlayerCantMove)
            {
                _mouseOnClickPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                _mouseDirection = (_mouseOnClickPosition - (Vector2)transform.position).normalized;

                if (_interactableComponent.IsInteracting())
                {
                    if (_moveToExcatPosition != null)
                    {
                        StopCoroutine(_moveToExcatPosition);
                        _moveToExcatPosition = null;
                        _instaceIdToCollider = null;
                    }
                    _interactableComponent.RemoveInteractableState();
                }

            }

            if (_moveToExcatPosition != null)
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
        private void StopPlayer()
        {
            if (_moveToExcatPosition != null) return;

            if (Vector2.Distance(_mouseOnClickPosition, transform.position) < _stopRange)
            {
                _rigidBody2D.velocity = Vector2.zero;
            }
        }

        private void ChangePlayerSide(bool right)
        {
            this.transform.localScale = new Vector2(right ? 1 : -1, 1);
        }

        private void ManageTheStopCollision(Collider2D collision)
        {
            if (_instaceIdToCollider is null) return;

            MovementMouseCollider movementMouseCollider = collision.gameObject.GetComponent<MovementMouseCollider>();

            if (movementMouseCollider is null) return;
            if (movementMouseCollider.GetInstanceID() == _instaceIdToCollider.Value)
                _instaceIdToCollider = null;
        }
        #endregion

        #region ABSTRACT METHODS
        protected override void ValidateValues()
        {
            // => Required Fields
            if (_playerState == null) Debug.LogError(ValidatorUtils.ValidateNullAtGameObject(nameof(_playerState), this.gameObject.name));
            if (_inputManager == null) Debug.LogError(ValidatorUtils.ValidateNullAtGameObject(nameof(_inputManager), this.gameObject.name));
        }

        protected override void SetInitialValues()
        {
            _internCanMove = true;
            _animator = this.GetComponent<Animator>();
            _rigidBody2D = this.GetComponent<Rigidbody2D>();
            _interactableComponent = this.GetComponent<InteractableComponent>();
            _playerState = GameObject.FindObjectOfType<PlayerStateManager>();
            _inputManager = GameObject.FindObjectOfType<InputManager>();

            _isActive = false;
            _animatorVariables = new MovementCursorAnimatorVariables();

            if (_velocity == 0) _velocity = 5;
            if (_stopRange == 0) _stopRange = 0.2f;
        }
        #endregion
    }
}
