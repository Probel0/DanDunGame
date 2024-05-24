using System.ComponentModel;
using UnityEngine;

public abstract class BaseModel
{
    public virtual void Initialize()
    {
        Debug.Log("Load Recourses...");
        //! Load...
        Debug.Log("Load Success");
    }
}
