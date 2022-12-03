using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SetActiveObject : MonoBehaviour
{
    public void SetActiveFalse() 
    {
        this.gameObject.SetActive(false);
    }
    public void SetActiveTrue()
    {
        this.gameObject.SetActive(true);
    }
}
