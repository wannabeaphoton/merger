using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;
using UnityEngine.UI;

namespace Client
{
    sealed class BossHitSystem : IEcsRunSystem
    {
        readonly EcsFilterInject<Inc<BossComponent>> _bossfilter = default;
        readonly EcsFilterInject<Inc<BossHitComponent>> _hitfilter = default;
        readonly EcsPoolInject<BossHitComponent> _hitpool = default;
        readonly EcsPoolInject<BossComponent> _bosspool = default;
        public void Run(EcsSystems system)
        {
            foreach (int _bossentity in _bossfilter.Value)
            {
                ref var _boss = ref _bosspool.Value.Get(_bossentity);
                foreach (int _hitentity in _hitfilter.Value)
                {
                    ref var _hit = ref _hitpool.Value.Get(_hitentity);
                    _boss.health -= _hit.damage;
                    _boss.animator.Play("Hit");
                    _boss.healthbar.GetComponent<Image>().material.SetFloat("_health", 0.01f * _boss.health);
                    _hitpool.Value.Del(_hitentity);
                }
            }
        }
    }
}