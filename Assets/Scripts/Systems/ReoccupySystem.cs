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
        readonly EcsFilterInject<Inc<CellComponent, OccupiedComponent>> _cellfilter = default;
        readonly EcsPoolInject<CellComponent> _cellpool = default;
        readonly EcsPoolInject<ReoccupyComponent> _repool = default;
        readonly EcsPoolInject<Drag> _dragpool = default;
        readonly EcsPoolInject<OccupiedComponent> _occpool = default;
        readonly EcsPoolInject<IsAnimatedComponent> _isanimpool = default;
        public void Run(EcsSystems systems)
        {
            foreach (int _cellentity in _cellfilter.Value)
            {
                ref var _cell = ref _cellpool.Value.Get(_cellentity);
                foreach (int _dragentity in _refilter.Value)
                {
                    ref var _drag = ref _dragpool.Value.Get(_dragentity);
                    if (_drag.originposition == _cell.cell.transform.position)
                    {
                        _cell.unit = null;
                        _occpool.Value.Del(_cellentity);
                        _isanimpool.Value.Del(_cellentity);
                        _repool.Value.Del(_dragentity);
                    }
                }
            }
        }
    }
}

