using Leopotam.EcsLite;
using UnityEngine;
using Leopotam.EcsLite.Di;

namespace Client {
    sealed class DragSystem : IEcsRunSystem {
        readonly EcsCustomInject<Config> _config = default;
        public void Run (EcsSystems systems) {
            var cam = Camera.main;
            EcsWorld _world = systems.GetWorld();
            EcsFilter _draggedfilter = _world.Filter<DraggedComponent>().Inc<UnitComponent>().End();
            EcsFilter _touchfilter = _world.Filter<TouchComponent>().End();
            EcsFilter _unitfilter = _world.Filter<UnitComponent>().End();
            var _touchpool = _world.GetPool<TouchComponent>();
            var _draggedpool = _world.GetPool<DraggedComponent>();
            foreach (int _touchentity in _touchfilter)
            {   
                foreach (int _draggedentity in _draggedfilter)
                {
                    ref var _touch = ref _touchpool.Get(_touchentity);
                    if (_touch.phase == TouchPhase.Canceled)
                    {
                        _draggedpool.Del(_draggedentity);
                        return;
                    }

                    var _mcheckpool = _world.GetPool<MergeCheckComponent>();
                    var _unitpool = _world.GetPool<UnitComponent>();
                    var _isanimpool = _world.GetPool<IsAnimatedComponent>();
                    ref var _unit = ref _unitpool.Get(_draggedentity);
                    Vector3 position = new Vector3(_touch.position.x, _touch.position.y, cam.WorldToScreenPoint(_unit.model.transform.position).z);
                    Vector3 worldPosition = cam.ScreenToWorldPoint(position);
                    _unit.model.GetComponent<Rigidbody>().MovePosition(new Vector3(worldPosition.x, 1.5f, worldPosition.z));
                    //Debug.DrawRay(_unit.model.transform.position, Vector3.down * 5f, Color.green);
                    if (_touch.phase == TouchPhase.Ended)
                    {
                        _mcheckpool.Add(_draggedentity);
                        _isanimpool.Del(_draggedentity);
                        _draggedpool.Del(_draggedentity);
                    }
                }
            }
        }
    }
}