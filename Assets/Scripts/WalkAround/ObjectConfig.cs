﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Serialization;

    [Serializable]
    public class InteractEvent : UnityEvent<ObjectConfig> {
    }

    public class ObjectConfig : SerializedMonoBehaviour {
        [Header("Basic Attributes")]
        public bool IsPlayer;
        public bool HasCollision;
        public bool IsVisible;
        public bool IsControllable;
        public bool IsSwitchableSprite;
        [Header("Interaction")]
        public bool IsInteractable;
        public bool IsTalkspritable;
        public bool IsStated;
        [ShowIf("IsStated")] public bool IsDialogueTrigger;
        public bool IsSimpleDialogue;
        public bool IsLookable;
        [ShowIf("IsLookable")] public bool HasInteractBubble;
        [ShowIf("@this.IsDialogueTrigger || this.IsSimpleDialogue || this.IsTalkspritable")]
        public bool HasTalkBubble;
        [Header("Special")]
        public bool IsDoor;
        public bool IsCutsceneTrigger;
        public string ID;

        [ShowIf("IsInteractable")] public InteractEvent onInteract;
        [ShowIf("IsLookable")] public InteractEvent onRadius;
        [ShowIf("IsLookable")] public InteractEvent offRadius;
        [ShowIf("IsDoor")] public InteractEvent onDoor;
        [ShowIf("IsTalkspritable")] public TalkspriteController talkspriteController;
        [ShowIf("@this.IsDialogueTrigger || this.IsSimpleDialogue")] public string dialogueKey;
        [ShowIf("IsStated")] public NPCState npcState;
        [ShowIf("HasInteractBubble")] public SpriteRenderer interactBubble;
        [ShowIf("@this.HasTalkBubble || this.IsPlayer")] public SpriteRenderer talkBubble;
        [ShowIf("IsSwitchableSprite")] public SpriteSwitcher switcher;
        [ShowIf("IsCutsceneTrigger")] public Dictionary<string, bool> negateIf;
        
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

        public void Leave(ObjectConfig trigger) {
            offRadius.Invoke(trigger);
        }
        public void Interact(ObjectConfig trigger) {
            onInteract.Invoke(trigger);
        }

        public void Door(ObjectConfig trigger) {
            onDoor.Invoke(trigger);
        }
    }

