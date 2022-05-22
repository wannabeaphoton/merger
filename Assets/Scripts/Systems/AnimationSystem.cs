using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;

namespace Client
{
    sealed class AnimationSystem : IEcsRunSystem
    {
        readonly EcsFilterInject<Inc<IsAnimatedComponent>> _filter = default;
        readonly EcsPoolInject<IsAnimatedComponent> _isanimpool = default;
        readonly EcsPoolInject<CellComponent> _cellpool = default;
        readonly EcsPoolInject<ProjectileComponent> _projpool = default;
        readonly EcsCustomInject<Config> _config = default;


        public void Run(EcsSystems systems)
        {
            foreach (int _isanimentity in _filter.Value)
            {
                ref var _cell = ref _cellpool.Value.Get(_isanimentity);
                Animator _anim = _cell.unit.GetComponentInChildren<Animator>();
                _anim.Play("Attack1");

                if (_anim.GetCurrentAnimatorStateInfo(0).normalizedTime == 0f && !_anim.GetCurrentAnimatorStateInfo(0).IsName("Idle"))
                {
                    var _projentity = _projpool.Value.GetWorld().NewEntity();
                    ref var _projectile = ref _projpool.Value.Add(_projentity);
                    _projectile.startpoint = _cell.cell.transform.position + Vector3.up;
                    _projectile.projectile = (ParticleSystem)Object.Instantiate(_config.Value.projectile, _projectile.startpoint, Quaternion.identity);
                }
                
                if (_anim.GetCurrentAnimatorStateInfo(0).normalizedTime >= 1f && !_anim.GetCurrentAnimatorStateInfo(0).IsName("Idle"))
                {
                    _anim.Play("Idle");
                    _isanimpool.Value.Del(_isanimentity);
                }
            }
        }
    }
}

