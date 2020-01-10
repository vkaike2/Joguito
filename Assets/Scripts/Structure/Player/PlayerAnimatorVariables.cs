using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Scripts.Structure.Player
{
    public class PlayerAnimatorVariables
    {
        public int Die => Animator.StringToHash("die");
    }
}
