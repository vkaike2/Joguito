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
    public class NormalMonsterAttackBehaviour : StateMachineBehaviour
    {
        #region PRIVATE ATRIBUTES
        private DamageDealerComponent _damageDealerComponent;
        private DamageTakerComponent _damageTakerComponent;
        private InteractableComponent _interactableComponent;
        private bool _isAttacking;
        #endregion

        #region Unity Methods
        override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            _isAttacking = false;
            _damageDealerComponent = animator.GetComponentInParent<DamageDealerComponent>();
            _interactableComponent = animator.GetComponentInParent<InteractableComponent>();
            _damageTakerComponent = animator.GetComponentInParent<DamageTakerComponent>();
        }

        override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            if (_isAttacking) return;
            if (_damageTakerComponent != null && !_damageTakerComponent.CanDoSomeAction) return;

            Transform currentPosition = animator.transform;

            RaycastHit2D[] hitList = Physics2D.RaycastAll(currentPosition.position, Vector2.zero);
            List<DamageTakerComponent> mapsToAttack = new List<DamageTakerComponent>();

            foreach (RaycastHit2D hit in hitList)
            {
                mapsToAttack.AddRange(hit.collider.gameObject.GetComponents<DamageTakerComponent>().Where(e => !e.PlayerInteractWith).ToList());
            }

            if (mapsToAttack.Any())
            {
                _isAttacking = true;
                _interactableComponent.SetInteractableState(EnumInteractableState.Atack, mapsToAttack[0].GetInstanceID());
                mapsToAttack[0].StartDefenseOperation(_damageDealerComponent);
            }
        }
        #endregion
    }
}
