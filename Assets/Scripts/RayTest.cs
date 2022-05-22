using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Leopotam.EcsLite;
using Voody.UniLeo.Lite;
// Convert the 2D position of the mouse into a
// 3D position.  Display these on the game window.

namespace Client { 
public class RayTest : MonoBehaviour
    {
        private Camera cam;
        private Touch touch;
        private RaycastHit unit, _cell;
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
                bool _unit = Physics.Raycast(point, out unit, 50f, unitMask);
                if (_unit && touch.phase == TouchPhase.Began)
                {
                    EcsWorld _world = WorldHandler.GetMainWorld();
                    var _entity = _world.NewEntity();
                    var _dragpool = _world.GetPool<Drag>();
                    var _reoccupypool = _world.GetPool<ReoccupyComponent>();
                    _dragpool.Add(_entity);
                    _reoccupypool.Add(_entity);
                    ref Drag _drag = ref _dragpool.Get(_entity);
                    _drag.rigidbody = unit.rigidbody;
                    _drag.originposition = new Vector3(unit.rigidbody.position.x, 0f, unit.rigidbody.position.z);
                    Animator anim = _drag.rigidbody.gameObject.GetComponentInChildren<Animator>();
                    anim.Play("falling_idle");
                    Debug.Log(_drag.originposition);
                }
            }

       
            
        
        }
    }

}
