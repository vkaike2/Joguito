using Assets.Scripts.Components.Map;
using Assets.Scripts.DTOs;
using Assets.Scripts.Extensions;
using Assets.Scripts.ScriptableComponents.Boss;
using Assets.Scripts.ScriptableComponents.Mob;
using Assets.Scripts.Structure.Boss;
using Assets.Scripts.Structure.Mob;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Scripts.Components.Tile
{
    public class TileMapRandomGeneratorComponent : BaseComponent
    {
        #region SERIALIZABLE ATTRIBUTES
        [Header("BOSS SPAWN POINT")]
        [SerializeField]
        private Transform _bossSpawnPoint;

        [Header("RANDOM SPRITES")]
        [SerializeField]
        private List<TileMapAnimatorAttribute> _tileMapAnimatorAttributeList;
        #endregion

        #region PRIVATE ATTRIBUTES
        private List<TileMapComponnent> _tileMapChildrensList;
        private MapComponent _mapComponent;
        #endregion

        #region PUBLIC METHODS
        public void SpawnNewBoss(BossScriptable randomBoss)
        {
            GameObject bossGameObject = GameObject.Instantiate(randomBoss.BossPrefab, _bossSpawnPoint.position, Quaternion.identity);
            bossGameObject.GetComponentInChildren<BossStructure>().TunIntoABoss(randomBoss);
        }

        public void SpawnNewObjects(GameObject prefab, int amount)
        {
            this.ForceInitialization();

            for (int i = 0; i < amount; i++)
            {
                TileMapComponnent tileMap = _tileMapChildrensList.GetRandomTileMapWihtoutObstacle(new List<EnumSide>() { EnumSide.CENTER });

                tileMap.SpawnObject(prefab);
                if (tileMap is null) return;
            }
        }

        public void SpawnNewMobs(MobScriptable mobScriptable, int amount)
        {
            this.ForceInitialization();

            List<TileMapComponnent> tileMapWithoutObjectsList = _tileMapChildrensList
                .Where(e => !e.HasObstacle && e.Side != EnumSide.BOTTOM && e.Side != EnumSide.BOTTOM_RIGHT && e.Side != EnumSide.LEFT_BOTTOM).ToList();
            List<int> indexToSpanw = new List<int>();

            while (amount != 0)
            {
                int index = UnityEngine.Random.Range(0, tileMapWithoutObjectsList.Count);
                if (indexToSpanw.Any(e => e == index))
                {
                    continue;
                }
                else
                {
                    indexToSpanw.Add(index);
                    amount--;
                }
            }

            foreach (int index in indexToSpanw)
            {
                TileMapComponnent tileMap = tileMapWithoutObjectsList[index];

                GameObject mobGameObject = GameObject.Instantiate(mobScriptable.MobPrefab, tileMap.transform.position, Quaternion.identity);
                mobGameObject.GetComponentInChildren<MobStructure>().TunIntoAMob(mobScriptable, _mapComponent.Tier);
            }
        }
        #endregion

        #region UNITY METHODS
        private void Start()
        {
            this.StartEveryTile();
        }
        #endregion

        #region PRIVATE METHODS
        private void StartEveryTile()
        {
            foreach (TileMapComponnent tileMap in _tileMapChildrensList)
            {
                TileMapAnimatorAttribute animatorAttribute = _tileMapAnimatorAttributeList.FirstOrDefault(e => e.EnumSide == tileMap.Side);
                tileMap.SetInitialAnimator(animatorAttribute.AnimatorController, animatorAttribute.AnimatorLayerIdList.GetRandomLayer());
            }
        }

        private void ForceInitialization()
        {
            if (_tileMapChildrensList is null) _tileMapChildrensList = this.GetComponentsInChildren<TileMapComponnent>().ToList();
            if (_mapComponent is null) _mapComponent = this.GetComponentInParent<MapComponent>();
        }
        #endregion

        #region ABSTRACT METHODS
        protected override void SetInitialValues()
        {
            this.ForceInitialization();
        }

        protected override void ValidateValues()
        {
        }
        #endregion
    }
}
