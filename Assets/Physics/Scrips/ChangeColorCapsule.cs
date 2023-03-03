using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChangeColorCapsule : MonoBehaviour
{
    private Material mat;

    public Color32 colorGreen;
    public Color32 colorBlue;
    public Color32 colorViolet;
    public Color32 colorRed;
    // Start is called before the first frame update
    void Start()
    {
        mat = GetComponent<MeshRenderer>().material;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnTriggerEnter(Collider c)
    {
        if (c.tag == "TriggerGreen")
        {
            mat.SetColor("_EmissiveColor", colorGreen);
        }
        if (c.tag == "TriggerBlue")
        {
            mat.SetColor("_EmissiveColor", colorBlue);
        }
        if (c.tag == "TriggerViolet")
        {
            mat.SetColor("_EmissiveColor", colorViolet);
        }
        if (c.tag == "TriggerRed")
        {
            mat.SetColor("_EmissiveColor", colorRed);
        }
    }
}
