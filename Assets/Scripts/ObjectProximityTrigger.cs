using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Assets.Scripts.WalkAround.Objects.Implementations;
using UnityEngine;

public class ObjectProximityTrigger : MonoBehaviour {
    // Start is called before the first frame update

    public ObjectConfig objConfig;
    
    void Start() {

    }

    // Update is called once per frame
    void Update() {

    }

    private void OnTriggerEnter(Collider other) {
        if (!objConfig.IsInteractable) return;
        if (other.gameObject.CompareTag("Player")) {
            GameObject playerObject = other.gameObject;
            Debug.Log("Player object " + playerObject.name + " entered trigger!");
            if (objConfig.IsCutsceneTrigger) {
                foreach (KeyValuePair<string, bool> neg in objConfig.negateIf) {
                    if (StoryModeGameManager.Instance._gamestate.GetFlag(neg.Key) == neg.Value) return;
                }
                WalkaroundManager.Instance.DialogueRunner.StartDialogue(objConfig.ID);
                Destroy(this.gameObject);
            }
        }
    }

    private void OnTriggerStay(Collider other) {
        if (!objConfig.IsInteractable) return;
        if (!other.tag.Equals("Player")) return;
        
        if (objConfig.IsLookable) {
            WalkaroundManager.Instance.ReadPotentialInteraction(objConfig);
            return;
        }
        
        LayerMask lm = ~(1 << LayerMask.NameToLayer("Player"));
        RaycastHit2D h = Physics2D.Raycast(other.transform.position, other.GetComponent<ObjectController>()._facing, 1f,
            lm);


        if (h.collider != null) {
            
            Debug.Log(h.collider.gameObject.name);
            if (h.collider.transform.gameObject.Equals(this.gameObject)) {
                Debug.Log("Successful!");
                WalkaroundManager.Instance.ReadPotentialInteraction(objConfig);
            }
        }
        else {
            WalkaroundManager.Instance.ReadPotentialInteraction(null);
        }
    }
    
    private void OnTriggerExit(Collider other) {
        if (other.gameObject.CompareTag("Player")) {
            WalkaroundManager.Instance.ReadPotentialInteraction(null);
        }
    }
}
