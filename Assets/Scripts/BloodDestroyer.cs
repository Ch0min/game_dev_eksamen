using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BloodDestroyer : MonoBehaviour
{
    void Update() {
        Destroy(gameObject, 0.2f);
    }
}
