using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;
using UnityEngine.UI;

namespace Client
{
    sealed class MergeSystem : IEcsRunSystem
    {
        readonly EcsFilterInject<Inc<MergeComponent>> _mergefilter = default;
        readonly EcsPoolInject<MergeComponent> _mergepool = default;
        readonly EcsPoolInject<UnitComponent> _unitpool = default;
        
        
        
        public void Run(EcsSystems systems)
        {   

            foreach (int _entity in _mergefilter.Value)
            {
                ref var _unit = ref _unitpool.Value.Get(_entity);
                if (_unit.tier <= 3)
                {
                    _unit.tier += 1;
                    Canvas _canvas = _unit.model.GetComponentInChildren<Canvas>();
                    Image[] _images = _canvas.GetComponentsInChildren<Image>(true);
                    foreach (Image _image in _images)
                    {
                        _image.gameObject.SetActive(false);
                    }
                    _images[_unit.tier - 1].gameObject.SetActive(true);
                }
                
                _mergepool.Value.Del(_entity);
            }
        }
    }
}

