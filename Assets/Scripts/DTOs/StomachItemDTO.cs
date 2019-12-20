using Assets.Scripts.ScriptableComponents.Item;
using System;

namespace Assets.Scripts.DTOs
{
    public class StomachItemDTO
    {
        public Guid Id { get; set; }
        public ItemScriptable Item { get; set; }
        public EnumStomachItemDTOState State => DigestionPercent >= 1 ? EnumStomachItemDTOState.ReadyToPoop : EnumStomachItemDTOState.Digesting;
        public float DigestionPercent { get; set; }
    }

    public enum EnumStomachItemDTOState
    {
        Digesting,
        ReadyToPoop
    }
}
