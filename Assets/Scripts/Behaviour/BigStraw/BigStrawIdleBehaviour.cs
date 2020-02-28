using Assets.Scripts.Components.DamageDealer;
using Assets.Scripts.Components.DamageTaker;
using Assets.Scripts.Components.Interactable;
using Assets.Scripts.Interface;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Assets.Scripts.Behaviour.BigStraw
{
    public class BigStrawIdleBehaviour : StateMachineBehaviour
    {
        #region SERIALIZABLE ATRIBUTES
        [Header("Configuration")]
        [SerializeField]
        private EnumState _currentState;
        #endregion

        #region PRIVATE ATRIBUTES
        private DamageDealerComponent _damageDealerComponent;
        private InteractableComponent _interactableComponent;
        private bool _isAttacking;
        #endregion

        #region Unity Methods
        override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            if (_currentState == EnumState.Idle)
            {
                _isAttacking = false;
                _damageDealerComponent = animator.GetComponentInParent<DamageDealerComponent>();
                _interactableComponent = animator.GetComponentInParent<InteractableComponent>();
            }
        }

        override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            if (_currentState == EnumState.Idle)
            {
                if (_isAttacking) return;

                Transform currentPosition = animator.transform;

                RaycastHit2D[] hitList = Physics2D.RaycastAll(currentPosition.position, Vector2.zero);
                List<DamageTakerComponent> mapsToAttack = new List<DamageTakerComponent>();

                foreach (RaycastHit2D hit in hitList)
                {
                    mapsToAttack.AddRange(hit.collider.gameObject.GetComponents<DamageTakerComponent>().Where(e => !e.PlayerInteractWith).ToList());
                }

                if(mapsToAttack.Any())
                {
                    _isAttacking = true;
                    _interactableComponent.SetInteractableState(EnumInteractableState.Atack, mapsToAttack[0].GetInstanceID());
                    mapsToAttack[0].StartDefenseOperation(_damageDealerComponent);
                }
            }
        }
        #endregion

        // OnStateExit is called when a transition ends and the state machine finishes evaluating this state
        //override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        //{
        //    
        //}

        // OnStateMove is called right after Animator.OnAnimatorMove()
        //override public void OnStateMove(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        //{
        //    // Implement code that processes and affects root motion
        //}

        // OnStateIK is called right after Animator.OnAnimatorIK()
        //override public void OnStateIK(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        //{
        //    // Implement code that sets up animation IK (inverse kinematics)
        //}
    }

    public enum EnumState
    {
        Idle
    }
}
