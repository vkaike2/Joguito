using Assets.Scripts.Components;
using Assets.Scripts.Components.DamageDealer;
using Assets.Scripts.Components.Interactable;
using Assets.Scripts.Components.MovementMouse;
using Assets.Scripts.Components.PlantSpot;
using Assets.Scripts.Interface;
using Assets.Scripts.Managers.Inputs;
using Assets.Scripts.Managers.PlayerState;
using Assets.Scripts.Structure.Player;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Assets.Scripts.Component.MouseCursor
{
    public class MouseCursorComponent : BaseComponent
    {
        #region PUBLIC VOID
        public bool HasItemUnderTheCursor { get; set; }
        #endregion

        #region PRIVATE ATTRIBURES
        private PlayerStateManager _playerStateManager;
        private InputManager _inputManager;
        private bool _leftButtomPressed = false;
        private bool _rightButtomPressed = false;
        private AudioComponent _audioComponent;
        private bool _canMove;
        #endregion

        #region UNITY METHODS
        private void FixedUpdate()
        {
            ManageMouseOver();
            ManageMouseUniqueLeftClick();
            ManageMouseUniqueRightClick();

            ManageMouseConinuousClick();
        }

        #endregion

        #region PRIVATE METHODS
        private Vector2 ManageMouseClick(Action<RaycastResult> callbackHitUI, Action<RaycastHit2D> callbackHit)
        {
            Vector2 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            RaycastHit2D[] hitList = Physics2D.RaycastAll(mousePosition, Vector2.zero);

            PointerEventData pointerData = new PointerEventData(EventSystem.current)
            {
                pointerId = -1,
                position = Input.mousePosition
            };

            List<RaycastResult> htiUIList = new List<RaycastResult>();
            EventSystem.current.RaycastAll(pointerData, htiUIList);

            // => Normal Objects Under the Mouse
            foreach (RaycastHit2D hit in hitList)
            {
                callbackHit(hit);
            }

            // => UI Objects Under The Mouse
            foreach (RaycastResult hitUI in htiUIList)
            {
                callbackHitUI(hitUI);
            }

            return mousePosition;
        }

        private void ManageMouseOver()
        {
            InteractableComponent interactableComponent = _playerStateManager.GetActivePlayerStructure().GetInteractableComponent();
            if (interactableComponent is null) return;

            this.ManageMouseClick((hitUI) =>
            {

            },
            (hit) =>
            {
                IPlantable plantable = hit.collider.gameObject.GetComponent<IPlantable>();
                plantable?.MouseOver(true);
            });
        }

        private void ManageMouseConinuousClick()
        {
            if (_inputManager.MouseLeftButton == 1)
            {
                List<IInteractable> interactableList = new List<IInteractable>();
                List<Button> buttonList = new List<Button>();

                bool hitSomeUIComponent = false;

                Vector2 mousePosition = this.ManageMouseClick(
                (hitUI) =>
                {
                    hitSomeUIComponent = true;
                },
                (hit) =>
                {
                    buttonList.AddRange(hit.collider.gameObject.GetComponents<Button>());
                    interactableList.AddRange(hit.collider.gameObject.GetComponents<IInteractable>().ToList());
                });
                if (hitSomeUIComponent || HasItemUnderTheCursor) return;

                IInteractable interactableChoice = interactableList.OrderBy(e => e.Order()).FirstOrDefault();

                _canMove = interactableChoice is null && _canMove;

                if(interactableChoice is IDamageTaker damagable)
                {
                    _canMove = damagable.IsGround && _canMove;
                }
                _canMove = !buttonList.Any() && _canMove;



                MovementMouseComponent movementMouseComponent = _playerStateManager.GetActivePlayerStructure().GetMovementMouseComponent();
                if(_canMove) movementMouseComponent.ObjectGoToWalkContinuous(mousePosition);
            }
        }

        private void ManageMouseUniqueLeftClick()
        {
            if (_inputManager.MouseLeftButton == 1 && !_leftButtomPressed)
            {
                _leftButtomPressed = true;
                _audioComponent.Audio_Click();

                List<IInteractable> interactableList = new List<IInteractable>();
                List<Button> buttonList = new List<Button>();
                List<IPlayer> playersList = new List<IPlayer>();


                bool hitSomeUIComponent = false;
                _canMove = true;
                Vector2 mousePosition = this.ManageMouseClick((hitUI) =>
                {
                    hitSomeUIComponent = true;
                },
                (hit) =>
                {
                    buttonList.AddRange(hit.collider.gameObject.GetComponents<Button>());
                    interactableList.AddRange(hit.collider.gameObject.GetComponents<IInteractable>().ToList());
                    playersList.AddRange(hit.collider.gameObject.GetComponents<IPlayer>().ToList());
                });

                if (hitSomeUIComponent) return;

                IInteractable interactableChoice = interactableList.OrderBy(e => e.Order()).FirstOrDefault();

                if (playersList.Any() && interactableChoice is IDamageTaker)
                {
                    interactableChoice = interactableList.Where(e => !(e is IDamageTaker)).OrderBy(e => e.Order()).FirstOrDefault();
                }

                if (interactableChoice is IDamageTaker damagable)
                {
                    DamageDealerComponent damageDealerComponent = _playerStateManager.GetActivePlayerStructure().GetDamageDealerComponent();
                    
                    if (damageDealerComponent is null) return;

                    damagable.StartCombat(damageDealerComponent);
                }

                if (interactableChoice is IPickable pickable)
                {
                    pickable.PickUp();
                }

                if (interactableChoice is IPlantable plantable)
                {
                    plantable.Interact();
                }

            }
            else if (_inputManager.MouseLeftButton == 0 && _leftButtomPressed)
            {
                _leftButtomPressed = false;
            }
        }

        private void ManageMouseUniqueRightClick()
        {
            List<IPlayer> playersList = new List<IPlayer>();
            if (_inputManager.MouseRightButton == 1 && !_rightButtomPressed)
            {
                _rightButtomPressed = true;

                Vector2 mousePosition = this.ManageMouseClick((hitUI) =>
                {
                },
                (hit) =>
                {
                    playersList.AddRange(hit.collider.gameObject.GetComponents<IPlayer>().ToList());
                });

                playersList.FirstOrDefault()?.SwitchPlayer();
            }
            else if (_inputManager.MouseRightButton == 0 && _rightButtomPressed)
            {
                _rightButtomPressed = false;
            }
        }
        #endregion

        #region ABSTRACT METHODS
        protected override void SetInitialValues()
        {
            _playerStateManager = GameObject.FindObjectOfType<PlayerStateManager>();
            _inputManager = GameObject.FindObjectOfType<InputManager>();
            _audioComponent = this.GetComponent<AudioComponent>();
            HasItemUnderTheCursor = false;
        }

        protected override void ValidateValues() { }
        #endregion
    }

}
