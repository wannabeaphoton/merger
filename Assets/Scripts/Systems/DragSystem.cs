using Leopotam.EcsLite;
using UnityEngine;

namespace Client {
    sealed class DragSystem : IEcsRunSystem {
        public void Run (EcsSystems systems) {
            var cam = Camera.main;
            
            if (Input.touchCount > 0)
            {
                Touch _touch = Input.GetTouch(0);
                EcsWorld _world = systems.GetWorld();
                EcsFilter _filter = _world.Filter<DraggedComponent>().End();
                var _mergepool = _world.GetPool<MergeCheckComponent>();
                var _unitpool = _world.GetPool<UnitComponent>();
                var _isanimpool = _world.GetPool<IsAnimatedComponent>();
                    foreach (int entity in _filter)
                    {
                    ref var _unit = ref _unitpool.Get(entity);
                    Vector3 position = new Vector3(_touch.position.x, _touch.position.y, cam.WorldToScreenPoint(_unit.model.transform.position).z);
                    Vector3 worldPosition = cam.ScreenToWorldPoint(position);
                    _unit.model.GetComponent<Rigidbody>().MovePosition(new Vector3(worldPosition.x, 1.5f, worldPosition.z));
                    Debug.DrawRay(_unit.model.transform.position, Vector3.down * 5f, Color.green);
                   
                    if (_touch.phase == TouchPhase.Ended)
                        {
                            _mergepool.Add(entity);
                            _isanimpool.Del(entity);
                        }
                    }
                
            }
        }
    }
}