using System;
using UnityEngine;

public class CullableParticleParent : MonoBehaviour{
    private void Update() {
        if (transform.childCount == 0) {
            Destroy(gameObject);
        }
    }
}
