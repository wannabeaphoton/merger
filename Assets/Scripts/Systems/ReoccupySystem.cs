using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;

namespace Client
{
    sealed class ReoccupySystem : IEcsRunSystem
    {
        readonly EcsFilterInject<Inc<ReoccupyComponent>> _refilter = default;
        readonly EcsFilterInject<Inc<CellComponent>, Exc<OccupiedComponent>> _cellfilter = default;
        readonly EcsPoolInject<CellComponent> _cellpool = default;
        readonly EcsPoolInject<ReoccupyComponent> _repool = default;
        readonly EcsPoolInject<OccupiedComponent> _occpool = default;
        
        public void Run(EcsSystems systems)
        {
            foreach (int _reoccentity in _refilter.Value)
            {
                ref var _reocc = ref _repool.Value.Get(_reoccentity);
                foreach (int _cellentity in _cellfilter.Value)
                {
                    ref var _cell = ref _cellpool.Value.Get(_cellentity);
                    if (_reocc.reoccupy == _cell.position)
                    {
                        _occpool.Value.Add(_cellentity);
                        _repool.Value.Del(_reoccentity);
                    }
                }
            }
        }
    }
}

