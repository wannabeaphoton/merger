using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

namespace Client{
    [CreateAssetMenu]
sealed class Config : ScriptableObject
    {
        public GameObject boss;
        public GameObject prefab;
        public GameObject meleePrefab;
        public GameObject rangedPrefab;
        public ParticleSystem projectile;
        public int gridWidth;
        public int gridLength;
        public float touchdelay;


    }



}
