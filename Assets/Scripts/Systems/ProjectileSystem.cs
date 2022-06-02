using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Leopotam.EcsLite.Di;
using Leopotam.EcsLite;

namespace Client
{
    sealed class ProjectileSystem : IEcsRunSystem
    {
        readonly EcsFilterInject<Inc<ProjectileComponent>> _projfilter = default;
        readonly EcsPoolInject<ProjectileComponent> _projpool = default;
        readonly EcsPoolInject<BossHitComponent> _bosshitpool = default;
        readonly EcsCustomInject<Config> _config = default;
        

        public void Run(EcsSystems systems)
        {
            Vector3 _bosspos = _config.Value.boss.transform.position;
            foreach (int _projentity in _projfilter.Value)
            {
                ref var _proj = ref _projpool.Value.Get(_projentity);
                _proj.projectile.transform.position = Vector3.MoveTowards(_proj.projectile.transform.position, _bosspos + Vector3.up * 5f, 0.1f);
                if (_proj.projectile.transform.position == _bosspos + Vector3.up * 5f)
                {
                    var _hitentity = _bosshitpool.Value.GetWorld().NewEntity();
                    ref var _hit = ref _bosshitpool.Value.Add(_hitentity);
                    _hit.damage = _proj.damage;
                    GameObject.Destroy(_proj.projectile.gameObject);
                    _projpool.Value.Del(_projentity);
                }
            }
        }
    }
}

