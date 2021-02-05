using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using Yarn.Unity;

public class ShipManager : MonoBehaviour {

    public List<GameObject> locationButtons;
    
    public DialogueRunner runner;

    [Header("Alert/Skit")] public GameObject alert;

    public List<DialogueCondition> skits;
    public Dictionary<int, string> activeNodes;

    private List<GameObject> alerts;

    private EventSystem e;
    void Awake() {
        e = EventSystem.current;
        e.SetSelectedGameObject(locationButtons[0]);
    }
    // Start is called before the first frame update
    void Start() {
        StoryModeGameManager.Instance.stateUpdate += UpdateSkits;
        alerts = new List<GameObject>();
        activeNodes = new Dictionary<int, string>();
        UpdateSkits();
    }

    // Update is called once per frame
    void Update()
    {
        foreach (var n in activeNodes) {
            if (n.Key == -1) {
                runner.StartDialogue(n.Value);
                activeNodes.Remove(n.Key);
                break;
            }
        }
    }

    private void OnDestroy() {
        StoryModeGameManager.Instance.stateUpdate -= UpdateSkits;
    }

    private void UpdateAlerts() {
        foreach (GameObject oldAlert in alerts) {
            Destroy(oldAlert);
        }

        for (int i = 0; i < 4; i++) {
            if (activeNodes.ContainsKey(i)) {
                alerts.Add(Instantiate(alert, Vector3.up*70 + locationButtons[i].transform.position, Quaternion.identity, locationButtons[i].transform));
            }
        }
    }

    private void UpdateSkits() {
        activeNodes.Clear();
        
        foreach (DialogueCondition skit in skits) {
            Gamestate g = StoryModeGameManager.Instance._gamestate;
            // If the conditions are matched
            if (
                  (skit.integerFlags.Where(a=>g.integerFlags.ContainsKey(a.Key)).All(a=>a.Value == g.integerFlags[a.Key]))
               && (skit.booleanFlags.Where(a=>g.booleanFlags.ContainsKey(a.Key)).All(a=>a.Value == g.booleanFlags[a.Key]))
            )
            {
                activeNodes.Add(skit.location, skit.node);
            }
        }
        
        UpdateAlerts();
    }

    public void LaunchAreaDialogue(int i) {
        if (activeNodes.ContainsKey(i)) {
            string node = activeNodes.First(a => a.Key == i).Value ?? null;
            activeNodes.Remove(activeNodes.First(a => a.Key == i).Key);
        
            if (node != null) {
                runner.StartDialogue(node);
            }
        
            UpdateAlerts();
        }
    }
    
    public void LaunchSong() {
        LoadingScreen.Instance.Show(SceneManager.LoadSceneAsync("Song"));
    }
}
