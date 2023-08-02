using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class weapon_switching : MonoBehaviour
{
    public int heldWeaponIndex = 0;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        int previousHeldWeaponIndex = heldWeaponIndex;

        if (Input.GetButtonDown("GunCycleClockwise"))
        {
            if (heldWeaponIndex >= transform.childCount - 1)
            {
                heldWeaponIndex = 0;
            }
            else
            {
                heldWeaponIndex++;
            }
        }
        if (Input.GetButtonDown("GunCycleCounterclockwise"))
        {
            if (heldWeaponIndex <= 0)
            {
                heldWeaponIndex = transform.childCount - 1;
            }
            else
            {
                heldWeaponIndex--;
            }
        }


        if (previousHeldWeaponIndex != heldWeaponIndex)
        {
            SelectWeapon();
        }
    }

    public void SelectWeapon()
    {
        int i = 0;
        foreach (Transform weapon in transform)
        {
            if (i == heldWeaponIndex)
            {
                weapon.gameObject.SetActive(true);
            }
            else
            {
                weapon.gameObject.SetActive(false);
            }
            i++;
        }
    }
}
