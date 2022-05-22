using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;

namespace Client
{
    sealed class OccupiedSystem : IEcsRunSystem
    {
        readonly EcsFilterInject<Inc<OccupiedComponent, CellComponent>,Exc<IsAnimatedComponent>> _filter = default;
        readonly EcsPoolInject<CellComponent> _cellpool = default;
        readonly EcsPoolInject<IsAnimatedComponent> _isanimpool = default;
        
        public void Run(EcsSystems systems)
        {

            foreach (int _cellentity in _filter.Value)
            {   
                ref var _cell = ref _cellpool.Value.Get(_cellentity);
                Animator _anim = _cell.unit.GetComponentInChildren<Animator>();
                if (_anim.GetCurrentAnimatorStateInfo(0).IsName("Idle") && _anim.GetCurrentAnimatorStateInfo(0).normalizedTime >=1f)
                {
                    _isanimpool.Value.Add(_cellentity);
                }
            }
        }
    }
}

