using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CanTryCode : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        LuisManager.getInstance().setCanTryCode(true);
        GameManager.getInstance().tiTextInformation.SetText("Ecrit le code sur le sol avec la souris puis dit \"Essaye le code\"");
    }

    private void OnTriggerExit(Collider other)
    {
        LuisManager.getInstance().setCanTryCode(false);
    }
}
