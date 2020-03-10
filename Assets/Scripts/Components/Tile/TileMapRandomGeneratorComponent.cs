using Assets.Scripts.Extensions;
using Assets.Scripts.ScriptableComponents.Boss;
using Assets.Scripts.Structure.Boss;
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

        public void SpawnNewBoss(BossScriptable randomBoss)
        {
            GameObject bossGameObject = GameObject.Instantiate(randomBoss.BossPrefab, _bossSpawnPoint.position, Quaternion.identity);
            bossGameObject.GetComponentInChildren<BossStructure>().TunIntoABoss(randomBoss);
        }

        public void SpawnNewObjects(GameObject prefab, int amount)
        {
            if (_tileMapChildrensList is null) _tileMapChildrensList = this.GetComponentsInChildren<TileMapComponnent>().ToList();
            for (int i = 0; i < amount; i++)
            {
                TileMapComponnent tileMap = _tileMapChildrensList.GetRandomTileMapWihtoutObstacle(new List<EnumSide>() { EnumSide.CENTER });

                tileMap.SpawnObject(prefab);
                if (tileMap is null) return;
            }
        }
        #endregion

        #region ABSTRACT METHODS
        protected override void SetInitialValues()
        {
            if (_tileMapChildrensList is null) _tileMapChildrensList = this.GetComponentsInChildren<TileMapComponnent>().ToList();
        }

        protected override void ValidateValues()
        {
        }
        #endregion
    }
}
