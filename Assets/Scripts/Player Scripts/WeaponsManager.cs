using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponsManager : MonoBehaviour
{
    public Transform weaponHold;
    //public Transform shootPoint;
	public GameObject[] gunPrefabs;
    private List<IWeapon> allGuns;
	IWeapon equippedGun;
    int equippedIndex = 0;

    void Awake(){
        allGuns = new List<IWeapon>();
        for(int i = 0; i < gunPrefabs.Length; i++){
            AddGun(gunPrefabs[i]);
            if(i == 0){
                equippedGun = allGuns[i];
            }
            else{
                allGuns[i].Disable();
            }
        }
    }

    public void EquipGun(int gunIndex){
        if(gunIndex != equippedIndex){
            allGuns[equippedIndex].Disable();
            equippedIndex = gunIndex;
            equippedGun = allGuns[gunIndex];
            equippedGun.Enable();
        }
    }

    public void ShootGun(){
        equippedGun.Attack();
    }

    public void AddGun(GameObject newGunPrefab){
        GameObject currentGun = Instantiate(newGunPrefab, weaponHold.position, weaponHold.rotation);
        allGuns.Add(currentGun.GetComponent<IWeapon>());
        currentGun.transform.SetParent(weaponHold);
    }

    public float GunHeight {
		get {
			return weaponHold.position.y;
		}
	}

	public void Aim(Vector3 aimPoint) {
		if (equippedGun != null) {
			equippedGun.Aim(aimPoint);
		}
	}

	public void Reload() {
		if (equippedGun != null) {
			equippedGun.Reload();
		}
	}
}
