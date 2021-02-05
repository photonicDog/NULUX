// WindowStateManager
using System.Collections;
using UnityEngine;

public class WindowStateManager : MonoBehaviour
{
    public static WindowStateManager Instance;

    public Animator endAnim;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Debug.Log("Multiple WSM in the scene!");
            Object.Destroy(base.gameObject);
        }
        Object.DontDestroyOnLoad(base.gameObject);
    }

    public void EndGame()
    {
        StartCoroutine(DoEndGame());
    }

    private IEnumerator DoEndGame()
    {
        endAnim.SetBool("leaving", value: true);
        yield return new WaitForSeconds(1.5f);
        Application.Quit();
    }
}
