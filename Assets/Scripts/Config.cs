using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
        public Hashtable tagmap;

    }



}
