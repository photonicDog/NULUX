// FreePlayManager
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class FreePlayManager : MonoBehaviour
{
    public static FreePlayManager Instance;

    public Track prototypeTrack;

    private void Awake()
    {
        Object.DontDestroyOnLoad(base.gameObject);
        if (Instance == null)
        {
            Instance = this;
            SceneManager.sceneLoaded += CheckDestroyableScene;
        }
        else
        {
            Debug.Log("Multiple FreePlayManager in the scene!");
            Object.Destroy(base.gameObject);
        }
    }

    private void Start()
    {
    }

    private void Update()
    {
    }

    private void CheckDestroyableScene(Scene s, LoadSceneMode ls)
    {
        if (s.buildIndex == 2 || s.buildIndex == 3)
        {
            SceneManager.sceneLoaded -= CheckDestroyableScene;
            Object.Destroy(base.gameObject);
        }
    }

    public void FreePlaySelect()
    {
        StartCoroutine(OnFreePlaySelect());
    }

    private IEnumerator OnFreePlaySelect()
    {
        while (!GameObject.Find("SongManager"))
        {
            yield return new WaitForEndOfFrame();
        }
        GameObject.Find("SongManager").GetComponent<SongManager>().SetCurrentTrack(prototypeTrack);
    }
}
