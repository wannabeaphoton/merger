using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;
using Leopotam.EcsLite.ExtendedSystems;
using Voody.UniLeo.Lite;
using Leopotam.EcsLite.Unity.Ugui;

using UnityEngine;


namespace Client {
    sealed class EcsStartup : MonoBehaviour {
        [SerializeField] Config _config;
        [SerializeField] EcsUguiEmitter _uguiEmitter;
        EcsSystems _systems;
        
        void Start () {
            // register your shared data here, for example:
            // var shared = new Shared ();
            // systems = new EcsSystems (new EcsWorld (), shared);

            _systems = new EcsSystems (new EcsWorld ());
            _systems
                .Add(new GridInitSystem())
                .Add(new DragSystem())
                .Add(new UISystem())
                .Add(new SpawnMeleeSystem())
                .Add(new MergeCheckSystem())
                .Add(new MergeSystem())
                .Add(new UnoccupySystem())
                .Add(new ReoccupySystem())
                .Add(new ProjectileSystem())
                .Add(new AttackSystem())
                .Add(new BossHitSystem())


                // register your systems here, for example:
                // .Add (new TestSystem1 ())
                // .Add (new TestSystem2 ())

                // register additional worlds here, for example:
                // .AddWorld (new EcsWorld (), "events")
#if UNITY_EDITOR
                // add debug systems for custom worlds here, for example:
                // .Add (new Leopotam.EcsLite.UnityEditor.EcsWorldDebugSystem ("events"))
                .Add(new Leopotam.EcsLite.UnityEditor.EcsWorldDebugSystem())
#endif
                .ConvertScene()
                .Inject(_config)
                .InjectUgui(_uguiEmitter)
                .Init ();
            
        }

        void Update () {
            _systems?.Run ();
        }

       

        void OnDestroy () {
            if (_systems != null) {
                _systems.Destroy ();
                // add here cleanup for custom worlds, for example:
                // _systems.GetWorld ("events").Destroy ();
                _systems.GetWorld ().Destroy ();
                _systems = null;
            }
        }
    }
}