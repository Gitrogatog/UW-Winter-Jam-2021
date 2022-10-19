using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IWeapon
{
    //public void SetShootPoint(Transform newShootPoint);
    public void Attack();
    
    public void Disable();
    public void Enable();
    public void Reload();
    public void Aim(Vector3 aimPoint);
}
