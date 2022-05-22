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
                EcsFilter _filter = _world.Filter<Drag>().End();
                var _mergepool = _world.GetPool<MergeCheckComponent>();
                var _pool = _world.GetPool<Drag>();
                LayerMask _layer = 1 << 6;
                RaycastHit _checker;


                foreach (int entity in _filter)
                {
                    ref Drag _drag = ref _pool.Get(entity);
                    _drag.rigidbody.useGravity = false;
                    Vector3 position = new Vector3(_touch.position.x, _touch.position.y, cam.WorldToScreenPoint(_drag.rigidbody.transform.position).z);
                    Vector3 worldPosition = cam.ScreenToWorldPoint(position);
                    //_drag.rigidbody.position = new Vector3(worldPosition.x, 1.5f, worldPosition.z);
                    _drag.rigidbody.MovePosition(new Vector3(worldPosition.x, 1.5f, worldPosition.z));
                    Debug.DrawRay(_drag.rigidbody.position, Vector3.down * 5f, Color.green);
                    if (Physics.Raycast(new Ray(_drag.rigidbody.position, Vector3.down * 5f), out _checker, 3f, _layer))
                    {
                        _drag.mergecheck = _checker;
                    }
                    if (_touch.phase == TouchPhase.Ended)
                    {
                        _mergepool.Add(entity);
                        Debug.Log(_drag.mergecheck.collider.transform.position);
                    }
                }
                
            }
            // add your run code here.
        }
    }
}