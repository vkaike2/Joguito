using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Scripts.Components.Plant
{
    public class PlantAnimatorVariables
    {
        public int MiddleState = Animator.StringToHash("middle-state");
        public int FinalState = Animator.StringToHash("final-state");
    }
}
