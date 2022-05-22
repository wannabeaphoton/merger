using System.Collections;
using System.Collections.Generic;
using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;
using Leopotam.EcsLite.Unity.Ugui;
using Leopotam.EcsLite.ExtendedSystems;
using UnityEngine;
using UnityEngine.Scripting;

namespace Client
{


public class UISystem : EcsUguiCallbackSystem
{
        EcsWorldInject _world = default;
        

        

        [Preserve]
        [EcsUguiClickEvent("meleebttn")]
        void SpawnMelee(in EcsUguiClickEvent evt)
        {
            EcsFilter _filter = _world.Value.Filter<CellComponent>().Exc<OccupiedComponent>().End();
            var _pool = _world.Value.GetPool<SpawnMeleeComponent>();
            int length = _filter.GetEntitiesCount();
                       
            if (length > 0)
            {
                var entity = _world.Value.NewEntity();
                _pool.Add(entity);
                
                
                //int id = Random.Range(0, length);
                //Debug.Log("rawID = ");
                //_pool.Add(_filter.GetRawEntities()[id]);
            }
            
        }

    
    
}

}
