using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PuzzleCubeSawn : MonoBehaviour
{
    [SerializeField]
    public GameObject cube;

    void Start()
    {
        Invoke("spawnCube", 0.1f);
    }

    public void spawnCube()
    {
        Instantiate(cube.gameObject, this.transform.position, Quaternion.identity, this.gameObject.transform);
    }
}
