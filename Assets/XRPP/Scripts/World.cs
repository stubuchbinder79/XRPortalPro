using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class World : MonoBehaviour {

    [Tooltip("The portal that this GameObject will be rendered in")]
    [SerializeField] private PortalWindow portalWindow;

    [Tooltip("Should this GameObject follow the device once it enters the world (Set to 'true' for 360 video)")]
    public bool followDevice = false;

    [SerializeField] private List<Material> materials = new List<Material>();

    private bool deviceIsInWorld = false;

    private void Awake()
    {
        portalWindow = portalWindow ?? FindObjectOfType<PortalWindow>();
        PortalWindow.InOtherWorld += PortalWindow_InOtherWorld;

        var renderers = GetComponentsInChildren<Renderer>();

        if (renderers.Length > 0)
        {
            foreach (var r in renderers)
            {
                foreach (Material mat in r.materials)
                {
                    if(this.materials.IndexOf(mat) < 0) {
                        this.materials.Add(mat);
                    }

                }
            }
        }

        portalWindow.AddMaterials(this.materials);
    }

  
    private void OnDestroy()
    {
        PortalWindow.InOtherWorld += PortalWindow_InOtherWorld;
    }

    private void Update()
    {
        if(followDevice && deviceIsInWorld) {
            transform.position = portalWindow.device.position;
        }
    }

    void PortalWindow_InOtherWorld(PortalWindow arg1, bool arg2)
    {
        if(arg1.Equals(portalWindow)) {
            deviceIsInWorld = arg2;
        } else {
            deviceIsInWorld = false;
        }
    }

}
