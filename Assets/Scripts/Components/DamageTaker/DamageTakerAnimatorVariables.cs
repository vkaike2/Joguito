using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Scripts.Components.DamageTaker
{
    public class DamageTakerAnimatorVariables
    {
        public int Die => Animator.StringToHash("die");
        public int TakeDamage => Animator.StringToHash("take-damage");

        public int BasicDamage => Animator.StringToHash("basic-damage");
    }
}
