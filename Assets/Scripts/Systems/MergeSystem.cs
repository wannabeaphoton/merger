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
        readonly EcsPoolInject<CellComponent> _cellpool = default;
        readonly EcsCustomInject<Config> _config = default;
        string _tag, _key;
        private Hashtable _map;
        
        
        public void Run(EcsSystems systems)
        {   

            foreach (int _entity in _mergefilter.Value)
            {
                ref var _merge = ref _mergepool.Value.Get(_entity);
                ref var _cell = ref _cellpool.Value.Get(_entity);
                _map = _config.Value.tagmap;
                _key = _merge.tag;
                _tag = _map[_key].ToString();
                Canvas _canvas = _cell.unit.GetComponentInChildren<Canvas>();
                Image[] _images = _canvas.GetComponentsInChildren<Image>(true);
                foreach (Image _image in _images)
                {
                    if (_image.tag != _tag)
                    {
                        
                        _image.gameObject.SetActive(false);
                    }
                    else
                    {
                        Debug.Log(_tag + " & " + _image.tag);
                        _image.gameObject.SetActive(true);
                    }
                }
                _mergepool.Value.Del(_entity);
            }
        }
    }
}

