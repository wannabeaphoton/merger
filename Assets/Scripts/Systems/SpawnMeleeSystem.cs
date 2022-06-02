using Leopotam.EcsLite;
using UnityEngine;
using Leopotam.EcsLite.Di;

namespace Client {
    sealed class SpawnMeleeSystem : IEcsRunSystem {

        
        readonly EcsFilterInject<Inc<SpawnMeleeComponent>> _spawnfilter = default;
        readonly EcsPoolInject<SpawnMeleeComponent> _spawnpool = default;
        readonly EcsPoolInject<OccupiedComponent> _occpool = default;
        readonly EcsPoolInject<CellComponent> _cellpool = default;
        readonly EcsPoolInject<UnitComponent> _unitpool = default;
        readonly EcsCustomInject<Config> _config = default;
        readonly EcsFilterInject<Inc<CellComponent>, Exc<OccupiedComponent>> _filter = default;
        public void Run (EcsSystems systems) {
            foreach (int spawntrigger in _spawnfilter.Value)
            { 
                int id = _filter.Value.GetRawEntities()[Random.Range(0, _filter.Value.GetEntitiesCount())];
                ref var _cell = ref _cellpool.Value.Get(id);
                Vector3 position = _cell.position;
                _occpool.Value.Add(id);
                int _unitentity =_unitpool.Value.GetWorld().NewEntity();
                ref var _unit = ref _unitpool.Value.Add(_unitentity);
                _unit.tier = 1;
                _unit.type = 1;
                _unit.position = _cell.position;
                _unit.model = (GameObject)Object.Instantiate(_config.Value.meleePrefab, position, Quaternion.identity);
                _unit.anim = _unit.model.GetComponentInChildren<Animator>();
                _spawnpool.Value.Del(spawntrigger);
            }

        }
    }
}