using UnityEngine;


namespace Client {
    public struct Drag {
        public Rigidbody rigidbody;
        public Vector3 originposition;
        public RaycastHit mergecheck;
    }
}