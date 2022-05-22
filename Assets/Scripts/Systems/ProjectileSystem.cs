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
        

        public void Run(EcsSystems systems)
        {
            foreach (int _projentity in _projfilter.Value)
            {
                ref var _proj = ref _projpool.Value.Get(_projentity);
                _proj.projectile.transform.position += new Vector3(0f,1f,16f) * 0.01f ;
                if (_proj.projectile.transform.position.magnitude >= 15f)
                {
                    GameObject.Destroy(_proj.projectile.gameObject);
                    _projpool.Value.Del(_projentity);
                }
                
                
            }
        }
    }
}

