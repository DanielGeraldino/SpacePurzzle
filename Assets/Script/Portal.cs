using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Portal : MonoBehaviour
{
    public bool portalOn = false;
    public GameObject objPortalOn;
    public GameObject objPortalOff;
    
    //public GameObject objPortalOff;
    
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

        if (portalOn)
        {
            objPortalOn.SetActive(true);
            objPortalOff.SetActive(false);
            GetComponent<CapsuleCollider2D>().isTrigger = true;

        }
        else
        {
            objPortalOn.SetActive(false);
        }
    }
}
