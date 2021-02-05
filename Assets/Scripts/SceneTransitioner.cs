using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

public class SceneTransitioner : MonoBehaviour {
    
    public GameObject button;
    // Start is called before the first frame update
    void Start()
    {
        EventSystem.current.SetSelectedGameObject(button);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void MoveScene(int i) {
        SceneManager.LoadScene(i);
    }
}
