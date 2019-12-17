using Assets.Scripts.Components.ActionSlot;
using Assets.Scripts.Components.PlantSpot;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Scripts.Components.Interactable
{
    public class InteractableAnimatorVariables
    {
        public int PlantSeed => Animator.StringToHash("plant-seed");

        public PlantSpotComponent PlantSpotComponent { get; set; }
        public ActionSlotComponent SelectedActionSlot { get; set; }

        public void ResetAuxiliarObjects()
        {
            PlantSpotComponent = null;
            SelectedActionSlot = null;
        }
    }
}
