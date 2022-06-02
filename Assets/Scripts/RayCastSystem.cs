using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Leopotam.EcsLite;
using Voody.UniLeo.Lite;
using Leopotam.EcsLite.Di;


namespace Client {
    sealed class RayCastSystem : IEcsInitSystem, IEcsRunSystem
    {
        private Camera cam;
        private Touch touch;
        private RaycastHit _target, _cell;
        private int unitMask = 1 << 3;
        private int cellMask = 1 << 6;
        readonly EcsFilterInject<Inc<TouchComponent>> _touchfilter = default;
        readonly EcsPoolInject<TouchComponent> _touchpool = default;
        readonly EcsPoolInject<DraggedComponent> _draggedpool = default;
        readonly EcsPoolInject<UnoccupyComponent> _unoccpool = default;
        readonly EcsFilterInject<Inc<UnitComponent>> _unitfilter = default;
        readonly EcsPoolInject<UnitComponent> _unitpool = default;
        readonly EcsPoolInject<IsAnimatedComponent> _isanimpool = default;
        EcsWorld _world;

        public void Init(EcsSystems systems)
        {
            cam = Camera.main;
            _world = systems.GetWorld();
        
        }

        public void Run(EcsSystems systems)
        {
            foreach (int _touchentity in _touchfilter.Value)
            {
                ref var _touch = ref _touchpool.Value.Get(_touchentity);
                if (_touch.phase != TouchPhase.Began)
                {
                    return;
                }
                else
                { 
                    Ray point = cam.ScreenPointToRay(new Vector3(_touch.position.x, _touch.position.y, cam.nearClipPlane));
                    Debug.DrawRay(point.origin, point.direction * 100f, Color.red);
                    bool _ishit = Physics.Raycast(point, out _target, 50f, unitMask);
                    if (!_ishit)
                    {
                        return;
                    }
                    else
                    {
                        Debug.Log(_target.collider.transform);
                        foreach (int _unitentity in _unitfilter.Value)
                        {
                            ref var _unit = ref _unitpool.Value.Get(_unitentity);
                            if(_target.collider.transform.position == _unit.position)
                            {
                                _unit.anim.Play("falling_idle");
                                _draggedpool.Value.Add(_unitentity);
                                _isanimpool.Value.Add(_unitentity);
                                ref var _unocc = ref _unoccpool.Value.Add(_world.NewEntity());
                                _unocc.unoccupy = _unit.position;
                            }
                        }
                    }
                }
            }
        }
    }
}
