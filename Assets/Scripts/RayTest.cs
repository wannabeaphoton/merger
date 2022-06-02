using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Leopotam.EcsLite;
using Voody.UniLeo.Lite;


namespace Client { 
public class RayTest : MonoBehaviour
    {
        private Camera cam;
        private Touch touch;
        private RaycastHit _target, _cell;
        private int unitMask = 1 << 3;
        private int cellMask = 1 << 6;

    



        void Start()
        {
            cam = Camera.main;

        
        }

        void Update()
        {


            if (Input.touchCount > 0)
            {
                touch = Input.GetTouch(0);
                Ray point = cam.ScreenPointToRay(new Vector3(touch.position.x, touch.position.y, cam.nearClipPlane));
                Debug.DrawRay(point.origin, point.direction * 100f, Color.red);
                bool _ishit = Physics.Raycast(point, out _target, 50f, unitMask);
                if (_ishit && touch.phase == TouchPhase.Began)
                {
                    EcsWorld _world = WorldHandler.GetMainWorld();
                    var _draggedpool = _world.GetPool<DraggedComponent>();
                    var _unoccupypool = _world.GetPool<UnoccupyComponent>();
                    var _filter = _world.Filter<UnitComponent>().End();
                    var _unitpool = _world.GetPool<UnitComponent>();
                    var _isanimpool = _world.GetPool<IsAnimatedComponent>();
                    Debug.Log(_target.collider.transform);
                    foreach (int _unitentity in _filter)
                    {
                        ref var _unit = ref _unitpool.Get(_unitentity);
                        if(_target.collider.transform.position == _unit.position)
                        {
                            _unit.model.GetComponent<Rigidbody>().useGravity = false;
                            _unit.anim.Play("falling_idle");
                            _draggedpool.Add(_unitentity);
                            _isanimpool.Add(_unitentity);
                            ref var _unocc = ref _unoccupypool.Add(_world.NewEntity());
                            _unocc.unoccupy = _unit.position;
                            
                        }
                    }
                    
                }
            }

       
            
        
        }
    }

}
