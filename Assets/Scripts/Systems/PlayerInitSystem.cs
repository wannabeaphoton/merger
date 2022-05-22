using UnityEngine;
using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;


namespace Client {
    sealed class PlayerInitSystem : IEcsInitSystem {
        readonly EcsWorldInject world = default;
        readonly EcsPoolInject<Drag> pool = default;
        
        public void Init (EcsSystems systems) {
            
            GameObject go = GameObject.FindGameObjectWithTag("Player");
            var entity = world.Value.NewEntity();
            ref var moveComponent = ref pool.Value.Add(entity);
            moveComponent.rigidbody = go.GetComponent<Rigidbody>();



    }
    }
}