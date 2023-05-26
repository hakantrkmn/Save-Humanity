using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract  class BaseEffector : MonoBehaviour,IClickable
{
    public EffectorTypes type;

    public virtual void OnMouseDown()
    {
        Destroy(gameObject);
    }
}
