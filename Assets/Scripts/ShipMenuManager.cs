using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem.UI;
using UnityEngine.SceneManagement;

public class ShipMenuManager : MonoBehaviour {

    public List<GameObject> menuScreens;
    public InputSystemUIInputModule InputModule;
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void ShowSubmenu(int index) {
        menuScreens[index].SetActive(true);
    }

    void HideSubmenu(int index) {
        menuScreens[index].SetActive(false);
    }


}
