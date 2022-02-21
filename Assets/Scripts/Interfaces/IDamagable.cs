using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IDamagable
{
    public void ChangeHealth(int value);
    public void ChangeHealth(int value,Vector3 origin);
}
