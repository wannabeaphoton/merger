using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;

namespace Client
{
    sealed class AttackSystem : IEcsRunSystem
    {
        readonly EcsFilterInject<Inc<UnitComponent>, Exc<IsAnimatedComponent>> _unitfilter = default;
        readonly EcsPoolInject<UnitComponent> _unitpool = default;
        readonly EcsPoolInject<ProjectileComponent> _projpool = default;
        readonly EcsCustomInject<Config> _config = default;
        

        public void Run(EcsSystems systems)
        {
            foreach (int _unitentity in _unitfilter.Value)
            {
                ref var _unit = ref _unitpool.Value.Get(_unitentity);
                _unit.atckcd += Time.deltaTime;
                if (_unit.atckcd >= 5f - _unit.tier * 0.5f)
                {
                    _unit.anim.Play("Attack1");
                    _unit.atckcd = 0f;
                    var _projentity = _projpool.Value.GetWorld().NewEntity();
                    ref var _projectile = ref _projpool.Value.Add(_projentity);
                    _projectile.startpoint = _unit.model.transform.position + Vector3.up;
                    _projectile.projectile = (ParticleSystem)Object.Instantiate(_config.Value.projectile, _projectile.startpoint, Quaternion.identity);
                    _projectile.damage = _unit.tier * 2;
                }
            }
        }
    }
}



