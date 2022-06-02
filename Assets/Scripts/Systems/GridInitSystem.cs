using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;
using UnityEngine;
using Unity;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

namespace Client {
    sealed class GridInitSystem : IEcsInitSystem {

        readonly EcsPoolInject<CellComponent> _pool = default;
        readonly EcsCustomInject<Config> _config = default;
        readonly EcsPoolInject<BossComponent> _bosspool = default;
        Quaternion _rotate = Quaternion.Euler(0, 90, 0);
        private Hashtable _map = new Hashtable();
        public void Init (EcsSystems systems) {

            for (int i = 1; i<=3; i++)
            {
                _map.Add("T" + i.ToString(), "T" + (i + 1).ToString());
                Debug.Log(i + "gives" +_map["T"+i].ToString());
            }
            _map.Add("T4", "T4");
            
            
            var _bossentity = _bosspool.Value.GetWorld().NewEntity();
            ref var _boss = ref _bosspool.Value.Add(_bossentity);
            _boss.model = (GameObject) Object.Instantiate(_config.Value.boss, _config.Value.boss.transform.position, Quaternion.Euler(0, 180, 0));
            _boss.animator = _boss.model.GetComponentInChildren<Animator>();
            _boss.health = 100;
            _boss.healthbar = GameObject.Find("HealthBar");
            _boss.healthbar.GetComponent<Image>().material.SetFloat("_health", 0.01f * _boss.health);

            Vector3 position = new Vector3(1, 0, 1);
            for (var w = 0; w < _config.Value.gridWidth; w++)
            {
                for (var l = 0; l < _config.Value.gridLength; l++)
                {
                    var cellEntity = _pool.Value.GetWorld().NewEntity();
                    ref var cell = ref _pool.Value.Add(cellEntity);
                    var go = (GameObject)Object.Instantiate(_config.Value.prefab, position, Quaternion.identity*_rotate);
                    _rotate *= Quaternion.Euler(0, 90, 0);
                    cell.position = go.transform.position;
                    position.x += 2f;
                    

                }
                position.z += 2f;
                position.x = 1;
                    
            }



        }
    }
}