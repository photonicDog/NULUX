using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Serialization;

namespace Assets.Scripts.WalkAround.Objects.Implementations
{
    public class ObjectConfig : MonoBehaviour
    {
        public bool HasCollision;
        public bool IsVisible;
        public bool IsControllable;
        public bool IsInteractable;
        public bool IsDialogueTrigger;
        public bool IsLookable;
        public bool IsDoor;
        public bool IsCutsceneTrigger;
        [FormerlySerializedAs("NPCKey")] public string Key;

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
            Key = trigger;
        }
    }
}
