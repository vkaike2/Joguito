using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Scripts.Components.Combat
{
    public class CombatAnimatorVariables
    {
        public int TakeDamage => Animator.StringToHash("take-damage");
        public int Die => Animator.StringToHash("die");
        public int Atack => Animator.StringToHash("attack");

        public int BasicDamage => Animator.StringToHash("basic-damage");
    }
}
