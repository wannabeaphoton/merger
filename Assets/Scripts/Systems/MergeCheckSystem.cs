using Leopotam.EcsLite;
using UnityEngine;
using Leopotam.EcsLite.Di;
using System.Collections;

namespace Client
{
    sealed class MergeCheckSystem : IEcsRunSystem
    {
        readonly EcsPoolInject<MergeCheckComponent> _mcheckpool = default;
        readonly EcsPoolInject<MergeComponent> _mergepool = default;
        readonly EcsPoolInject<CellComponent> _cellpool = default;
        readonly EcsPoolInject<Drag> _dragpool = default;
        readonly EcsFilterInject<Inc<CellComponent>> _cellfilter = default;
        readonly EcsFilterInject<Inc<Drag, MergeCheckComponent>> _dragfilter = default;
        readonly EcsPoolInject<OccupiedComponent> _occupiedpool = default;
        readonly EcsCustomInject<Config> _config = default;
        private Hashtable _map = new Hashtable();

        public void Run(EcsSystems systems)
        {
            _map = _config.Value.tagmap;
            foreach (int _cellentity in _cellfilter.Value)
            {
                ref var _cell = ref _cellpool.Value.Get(_cellentity);
                foreach (int _dragentity in _dragfilter.Value)
                {
                    ref var _drag = ref _dragpool.Value.Get(_dragentity);
                    if (_drag.mergecheck.collider.transform.position == _cell.cell.transform.position)
                    {
                        if (_occupiedpool.Value.Has(_cellentity))
                        {
                            Debug.Log("Merge!");
                            if (_drag.rigidbody.tag == _cell.unit.tag & _drag.rigidbody.tag != "T4")
                            {
                                GameObject.Destroy(_cell.unit, 0.1f);
                                _mergepool.Value.Add(_cellentity);
                                ref var _mtag = ref _mergepool.Value.Get(_cellentity);
                                _mtag.tag = _drag.rigidbody.tag;
                                Debug.Log(_map[_mtag.tag]);
                                _drag.rigidbody.tag = _map[_drag.rigidbody.tag].ToString();
                                _cell.unit = _drag.rigidbody.gameObject;
                                _drag.rigidbody.position = _cell.cell.transform.position;
                            }
                            else
                            {
                                _drag.rigidbody.position = _drag.originposition;
                                foreach (int _cellreentity in _cellfilter.Value)
                                {
                                    ref var _cellre = ref _cellpool.Value.Get(_cellreentity);
                                    if (_drag.originposition == _cellre.cell.transform.position)
                                    {
                                        _occupiedpool.Value.Add(_cellreentity);
                                        _cellre.unit = _drag.rigidbody.gameObject;
                                    }

                                }
                            }
                        }
                        else
                        {   
                            Debug.Log("cellposition = " + _cell.cell.transform.position);
                            Debug.Log("targetcollider = " + _drag.mergecheck.collider.transform.position);
                            _drag.rigidbody.position = _cell.cell.transform.position;
                            _occupiedpool.Value.Add(_cellentity);
                            _cell.unit = _drag.rigidbody.gameObject;
                        }
                        _drag.rigidbody.useGravity = true;
                        Animator anim = _drag.rigidbody.gameObject.GetComponentInChildren<Animator>();
                        anim.Play("Idle");
                        _dragpool.Value.Del(_dragentity);
                        _mcheckpool.Value.Del(_dragentity);
                    }
                }
            }
        }
    }
}