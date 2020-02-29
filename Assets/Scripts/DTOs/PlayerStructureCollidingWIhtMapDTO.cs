using UnityEngine;

namespace Assets.Scripts.DTOs
{
    public class PlayerStructureCollidingWIhtMapDTO
    {
        public int? PlayerStructureInstanceId { get; set; }
        public int? MapInstanceId { get; set; }
        public Transform PlayerStructureTransform { get; set; }
    }
}
