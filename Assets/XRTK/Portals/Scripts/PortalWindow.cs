using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

public class PortalWindow : MonoBehaviour {

    public static event Action<PortalWindow, bool> InOtherWorld = delegate { };

    [SerializeField] private List<Material> materials = new List<Material>();

    public Transform device;

    private bool wasInFront;
    private bool isColliding;
    private bool isInOtherWorld;

    private void Start()
    {
        device = Camera.main.transform;
        SetStencil(CompareFunction.Equal);
    }

    private void SetStencil(CompareFunction func)
    {
        foreach(Material mat in materials) {
            mat.SetInt("_StencilTest", (int)func);
        }
    }

    internal void AddMaterials(List<Material> arg0)
    {
        foreach (Material mat in arg0)
        {
            if(materials.IndexOf(mat) < 0) {
                materials.Add(mat);
            }
        }
    }


    private void OnTriggerStay(Collider other)
    {
        if (other.tag != Camera.main.tag)
            return;


        if (transform.position.z > other.transform.position.z)
        {
            // outside of world
            Debug.Log("outside of world");
            isInOtherWorld = false;
            SetStencil(CompareFunction.Equal);

        }
        else
        {
            Debug.Log("inside of world");
            isInOtherWorld = true;
            SetStencil(CompareFunction.NotEqual);
        }

        InOtherWorld(this, isInOtherWorld);
    }
}
