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
        readonly EcsPoolInject<UnitComponent> _unitpool = default;
        readonly EcsFilterInject<Inc<CellComponent>, Exc<OccupiedComponent>> _cellfilter = default;
        readonly EcsFilterInject<Inc<MergeCheckComponent, UnitComponent>> _mergecheckfilter = default;
        readonly EcsPoolInject<OccupiedComponent> _occupiedpool = default;
        readonly EcsFilterInject<Inc<UnitComponent>, Exc<MergeCheckComponent>> _unitfilter = default;
        readonly EcsPoolInject<ReoccupyComponent> _reoccpool = default;
        LayerMask _unitlayer = 1 << 3;
        LayerMask _celllayer = 1 << 6;
        RaycastHit _unitchecker, _cellchecker;

        public void Run(EcsSystems systems)
        {
            

            foreach (int _mergeentity in _mergecheckfilter.Value)
            {
                ref var _unit = ref _unitpool.Value.Get(_mergeentity);
                Debug.DrawRay(_unit.model.transform.position, Vector3.down * 5f, Color.cyan);
                if (Physics.Raycast(new Ray(_unit.model.transform.position, Vector3.down * 5f), out _unitchecker, 3f, _unitlayer))
                {
                    foreach (int _unitcheckentity in _unitfilter.Value)
                    {
                        ref var _unitcheck = ref _unitpool.Value.Get(_unitcheckentity);
                        if (_unitchecker.collider.transform.position == _unitcheck.position)
                        {
                            if (_unitcheck.tier == _unit.tier && _unitcheck.type == _unit.type && _unit.tier != 4)
                            {
                                GameObject.Destroy(_unitcheck.model);
                                //_unit.model.transform.position = _unitcheck.position + Vector3.up;
                                _unit.model.transform.position = _unitcheck.position;
                                _unit.model.GetComponent<Rigidbody>().useGravity = true;
                                _unit.position = _unitcheck.position;
                                _unitpool.Value.Del(_unitcheckentity);
                                _mergepool.Value.Add(_mergeentity);
                            }
                            else
                            {
                                _unit.model.transform.position = _unit.position;
                                var _reoccentity = _reoccpool.Value.GetWorld().NewEntity();
                                ref var _reocc = ref _reoccpool.Value.Add(_reoccentity);
                                _reocc.reoccupy = _unit.position;
                            }
                        }
                        
                    }
                }
                else if (Physics.Raycast(new Ray(_unit.model.transform.position, Vector3.down * 5f), out _cellchecker, 3f, _celllayer))
                {
                    foreach (int _cellentity in _cellfilter.Value)
                    {
                        ref var _cell = ref _cellpool.Value.Get(_cellentity);
                        if (_cell.position == _cellchecker.collider.transform.position)
                        {
                            _unit.position = _cell.position;
                            _unit.model.transform.position = _cell.position;
                            _occupiedpool.Value.Add(_cellentity);

                        }
                    }
                }
                else
                {
                    _unit.model.transform.position = _unit.position;
                    var _reoccentity = _reoccpool.Value.GetWorld().NewEntity();
                    ref var _reocc = ref _reoccpool.Value.Add(_reoccentity);
                    _reocc.reoccupy = _unit.position;
                }
                _mcheckpool.Value.Del(_mergeentity);
                _unit.anim.Play("Idle");
            }
        }
    }
}