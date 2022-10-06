using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterRotate : MonoBehaviour
{
    [SerializeField] private Camera playerCamera;

    private void FixedUpdate()
    {
        transform.rotation = Quaternion.Euler(0f, playerCamera.transform.eulerAngles.y, 0f);
    }
}
