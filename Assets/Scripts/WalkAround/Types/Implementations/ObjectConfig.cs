using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Serialization;

namespace Assets.Scripts.WalkAround.Objects.Implementations
{
    [Serializable]
    public class InteractEvent : UnityEvent<ObjectConfig> {
    }

    public class ObjectConfig : MonoBehaviour {
        public bool IsPlayer;
        public bool HasCollision;
        public bool IsVisible;
        public bool IsControllable;
        public bool IsInteractable;
        public bool IsTalkspritable;
        public bool IsDialogueTrigger;
        public bool IsSimpleDialogue;
        public bool IsLookable;
        public bool IsDoor;
        public bool IsCutsceneTrigger;
        public string ID;

        [ShowIf("IsInteractable")] public InteractEvent onInteract;
        [ShowIf("IsLookable")] public InteractEvent onRadius;
        [ShowIf("IsDoor")] public InteractEvent onDoor;
        [ShowIf("IsTalkspritable")] public TalkspriteController talkspriteController;
        [ShowIf("@this.IsDialogueTrigger || this.IsSimpleDialogue")] public string dialogueKey;
        
        public void ToggleCollision()
        {
            HasCollision = !HasCollision;
        }
        
        public void ToggleVisible()
        {
            IsVisible = !IsVisible;
        }

        public void ToggleControllable()
        {
            IsControllable = !IsControllable;
        }

        public void ToggleInteractable()
        {
            IsInteractable = !IsInteractable;
        }

        public void SetScriptTrigger(bool trigger)
        {
            IsDialogueTrigger = trigger;
        }
        
        public void SetKey(string trigger)
        {
            ID = trigger;
        }

        public void Look(ObjectConfig trigger) {
            onRadius.Invoke(trigger);
        }

        public void Interact(ObjectConfig trigger) {
            onInteract.Invoke(trigger);
        }

        public void Door(ObjectConfig trigger) {
            onDoor.Invoke(trigger);
        }
    }
}
