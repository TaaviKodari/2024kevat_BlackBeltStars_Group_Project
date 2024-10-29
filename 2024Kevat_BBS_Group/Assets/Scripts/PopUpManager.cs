using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PopUpManager : MonoBehaviour
{
    // activates or deactivates the inputted GameObject
    public void SetActivePopUp(GameObject popUp, bool active)
    {
        popUp.SetActive(active);
    }
    
    
}
