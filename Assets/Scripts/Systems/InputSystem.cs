using Leopotam.EcsLite;
using UnityEngine;
using Leopotam.EcsLite.Di;


namespace Client
{
    sealed class InputSystem : IEcsInitSystem, IEcsRunSystem
    {
        readonly EcsFilterInject<Inc<InputComponent>, Exc<DisableInputComponent>> _filter = default;
        readonly EcsPoolInject<TouchComponent> _touchpool = default;
        readonly EcsCustomInject<Config> _config = default;

        private float _stationaryTimer, _touchdelay;

        public void Init(EcsSystems systems)
        {
            var _world = systems.GetWorld();
            _world.GetPool<InputComponent>().Add(_world.NewEntity());
        }

        public void Run(EcsSystems systems)
        {
            foreach (var entity in _filter.Value)
            {
                if (Input.touchCount == 0) return;
                var _activetouch = Input.GetTouch(0);

                var phase = _activetouch.phase;
                if (phase == TouchPhase.Began)
                {
                    _touchdelay = 0;
                    if (!_touchpool.Value.Has(entity))
                    {
                        _touchpool.Value.Add(entity);
                    }
                    ref var _touch = ref _touchpool.Value.Get(entity);
                    _touch.phase = TouchPhase.Began;
                    _touch.position = _activetouch.position;
                    //_touch.velocity = Vector3.zero;
                    return;
                }



                if (phase == TouchPhase.Ended || phase == TouchPhase.Canceled)
                {
                    _touchdelay = 0;
                    if (!_touchpool.Value.Has(entity))
                    {
                        _touchpool.Value.Add(entity);
                    }
                    ref var _touch = ref _touchpool.Value.Get(entity);
                    _touch.phase = TouchPhase.Ended;
                    _touch.position = _activetouch.position;
                    //touchComp.Velocity = Vector3.zero;
                    return;
                }

                _touchdelay += Time.deltaTime;
                if (_touchdelay < _config.Value.touchdelay) return;

                if (phase == TouchPhase.Moved)
                {
                    _stationaryTimer = 0;

                    if (!_touchpool.Value.Has(entity)) _touchpool.Value.Add(entity);
                    ref var _touch = ref _touchpool.Value.Get(entity);

                    
                    _touch.phase = TouchPhase.Moved;
                    _touch.position = _activetouch.position;
                    return;
                }
                else if (phase == TouchPhase.Stationary)
                {
                    // чтобы не дергался во время медленного движения пальцем
                    // если зашли в состояние Stationary, то не сразу убирать старый Direction, а через 0.2 сек
                    if (!_touchpool.Value.Has(entity)) _touchpool.Value.Add(entity);
                    ref var _touch = ref _touchpool.Value.Get(entity);
                    _stationaryTimer += Time.deltaTime;
                    if (_stationaryTimer >= 0.2f)
                    {
                        _touch.phase = TouchPhase.Stationary;
                        _touch.position = _activetouch.position;
                    }
                }
            }
        }
    }
}
