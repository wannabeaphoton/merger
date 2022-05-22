using Leopotam.EcsLite;
using UnityEngine;
using Leopotam.EcsLite.Di;

namespace Client {
    sealed class SpawnMeleeSystem : IEcsRunSystem {

        
        readonly EcsFilterInject<Inc<SpawnMeleeComponent>> _spawnfilter = default;
        readonly EcsPoolInject<SpawnMeleeComponent> _spawnpool = default;
        readonly EcsPoolInject<OccupiedComponent> _unitpool = default;
        readonly EcsPoolInject<CellComponent> _cellpool = default;
        readonly EcsCustomInject<Config> _config = default;
        readonly EcsFilterInject<Inc<CellComponent>, Exc<OccupiedComponent>> _filter = default;
        public void Run (EcsSystems systems) {
            foreach (int spawntrigger in _spawnfilter.Value)
            { 
            int id = _filter.Value.GetRawEntities()[Random.Range(0, _filter.Value.GetEntitiesCount())];
            ref var cell = ref _cellpool.Value.Get(id);
            Vector3 position = cell.cell.transform.position+Vector3.up*0.5f;
            _unitpool.Value.Add(id);
            cell.unit = GameObject.Instantiate(_config.Value.meleePrefab, position, Quaternion.identity);

            _spawnpool.Value.Del(spawntrigger);
            }

        }
    }
}