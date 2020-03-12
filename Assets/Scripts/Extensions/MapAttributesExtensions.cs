using Assets.Scripts.Components.Map;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assets.Scripts.Extensions
{
    public static class MapAttributesExtensions
    {

        public static MapAttributes GetMapAttributeForTier(this List<MapAttributes> list, int tierMap)
        {
            List<MapAttributes> attributeList = list.Where(e =>
            {
                if (e.TierRange.x > tierMap || e.TierRange.y < tierMap) return false;
                return true;
            }).ToList();


            // => Add a Default Map
            if (!attributeList.Any()) attributeList.Add(new MapAttributes(EnumSpawnType.Mobs));
            if (attributeList.Count > 1) throw new Exception("One or more maps have the tier: " + tierMap);

            return attributeList[0];
        }
    }
}
