using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PreloadScene : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        LoadingScreen.Instance.Show(SceneManager.LoadSceneAsync(1));
    }
}
