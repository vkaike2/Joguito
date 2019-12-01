using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Scripts.Components.MovementMouse
{
    public class MovementCursorComponentAnimatorVariables
    {
        public int Running => Animator.StringToHash("running");
    }
}
