using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;

namespace Client
{
    sealed class UnoccupySystem : IEcsRunSystem
    {
        readonly EcsFilterInject<Inc<UnoccupyComponent>> _unfilter = default;
        readonly EcsFilterInject<Inc<CellComponent, OccupiedComponent>> _cellfilter = default;
        readonly EcsPoolInject<CellComponent> _cellpool = default;
        readonly EcsPoolInject<UnoccupyComponent> _unpool = default;
        readonly EcsPoolInject<OccupiedComponent> _occpool = default;
        
        public void Run(EcsSystems systems)
        {
            foreach (int _unoccentity in _unfilter.Value)
            {
                ref var _unocc = ref _unpool.Value.Get(_unoccentity);
                foreach (int _cellentity in _cellfilter.Value)
                {
                    ref var _cell = ref _cellpool.Value.Get(_cellentity);
                    if (_unocc.unoccupy == _cell.position)
                    {   
                        _occpool.Value.Del(_cellentity);
                        _unpool.Value.Del(_unoccentity);
                    }
                }
            }
            
        }
    }
}