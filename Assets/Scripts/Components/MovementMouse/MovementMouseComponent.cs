using Assets.Scripts.Component.MouseCursor;
using Assets.Scripts.Managers.Inputs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Scripts.Components.MovementMouse
{
    [RequireComponent(typeof(Animator))]
    [RequireComponent(typeof(Rigidbody2D))]
    public class MovementMouseComponent : MonoBehaviour
    {
        [Header("Required Fields")]
        [SerializeField]
        private MouseCursorComponent _mouseCursor;
        [SerializeField]
        private InputManager _inputManager;


        [Header("Configuration Fields")]
        [SerializeField]
        private float _velocity;
        [SerializeField]
        [Range(0, 1)]
        private float _stopRange;

        private Animator _animator;
        private readonly int aRunning = Animator.StringToHash("running");

        private Rigidbody2D _rigidBody2D;

        private Vector2 _mouseDirection;
        private Vector2 _mouseOnClickPosition;

        private void Start()
        {
            if (_velocity == 0) _velocity = 5;
            if (_stopRange == 0) _stopRange = 0.2f;
            _mouseDirection = this.transform.position;
            _mouseOnClickPosition = this.transform.position;

            _animator = this.GetComponent<Animator>();
            _rigidBody2D = this.GetComponent<Rigidbody2D>();
        }

        private void FixedUpdate()
        {
            this.MovementPlayer();
            this.AnimationController();
        }

        private void MovementPlayer()
        {
            if (_inputManager.MouseLeftButton == 1 && !_mouseCursor.CantMovePlayer)
            {
                _mouseOnClickPosition = _mouseCursor.CurrentPosition;
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
        }

        private void AnimationController()
        {
            _animator.SetBool(aRunning, _rigidBody2D.velocity != Vector2.zero);
        }

        private void ChangePlayerSide(bool right)
        {
            this.transform.rotation = new Quaternion(0, right ? 0 : 180, 0, 0);
        }
    }
}
