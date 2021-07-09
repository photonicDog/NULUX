/*using System;
using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using TMPro;
using UnityEngine;

public class NOVANote : SerializedMonoBehaviour {
    public Note note;
    public List<LineDataCommand> cmds;
    public List<VisualSet> visualSets;
    public VisualSet currentVisualSet;
    public Animator fade;

    public bool enableTells;

    [SerializeField] private LineRenderer tailRenderer;
    
    public SpriteRenderer head = default;
    public SpriteRenderer cap = default;

    [SerializeField] private GameObject hitEffect = default;
    [SerializeField] private GameObject holdEffect = default;
    [SerializeField] private SpriteRenderer telegraphSprite;
    [SerializeField] private TextMeshPro tellUI;

    private HoldParticle _holdBurn;
    private float _distanceIntoHold;

    public bool activated;
    public bool mobileNote;
    
    public void Initialize(Note n, List<LineDataCommand> lc) {
        note = n;
        cmds = lc;
        
        currentVisualSet = visualSets[(int)note.NoteType];
        head.sprite = currentVisualSet.head;
        telegraphSprite.sprite = currentVisualSet.telegraph;
        cap.sprite = currentVisualSet.cap;
        tailRenderer.material = currentVisualSet.arcMaterial;
        
        tellUI.gameObject.SetActive(enableTells);
        tellUI.text = currentVisualSet.noteTell;

        head.sortingOrder += -10000000 + (int)(note.Start * 480);
        tailRenderer.sortingOrder += -10000000 + (int)(note.Start * 480) - 1;
        cap.sortingOrder += -10000000 + (int)(note.Start * 480) - 2;
    }

    public void FadeIn(float fadeSpeed) {
        activated = true;
        StartCoroutine(Fade());
    }

    private void Update() {
        if (activated && mobileNote) {
            transform.position = CalculateMovingNotePosition();
        }
    }

    public IEnumerator Fade() {
        float startTime = Conductor.Instance.GetSongTime();
        if (note.Duration > 0) {
            tailRenderer.gameObject.SetActive(true);
            tailRenderer.gameObject.SetActive(true);
            cap.gameObject.SetActive(true);
        }
        
        fade.StartPlayback();
        telegraphSprite.gameObject.SetActive(true);
        while (Conductor.Instance.GetSongTime() <= note.Start) {
            float a = Mathf.InverseLerp(startTime, note.Start, Conductor.Instance.GetSongTime());
            fade.Play(0, -1, a);
            yield return new WaitForEndOfFrame();
        }
    }

    public void PlaceNote(NOVALineDeprecated nl) {
        activated = cmds[0].data.Mobile;
        transform.position = CalculateNotePosition();
    }

    public HoldParticle HitHold() {
        HoldParticle hp = Instantiate(holdEffect, transform.position, Quaternion.identity).GetComponent<HoldParticle>();
        hp.noteObject = note;
        return hp;
    }
    public void Kill(bool hit) {
        if (hit) {
            Instantiate(hitEffect, transform.position, Quaternion.identity);
        }
        
        head.enabled = false;
    }

    public void StartAnimateHold() {
        _holdBurn = HitHold();
        
    }

    public void AnimateHold(float currentTime) {
        LineDataCommand ldc = DetermineCurrentCMD(currentTime);
        Vector2 cursorPosition = ldc.GetNotePosition(currentTime, note.Position);
        head.gameObject.transform.position = cursorPosition;
        _holdBurn.gameObject.transform.position = cursorPosition;
    }

    public void EndAnimateHold() {
        if (_holdBurn) Destroy(_holdBurn.gameObject);
    }
    
    public Vector2 CalculateNotePosition() {
        Vector2 res = new Vector2();

        res = cmds[0].GetNotePosition(note.Start, note.Position);

        if (note.Duration > 0) {
            tailRenderer.positionCount = 21;
            Vector2 incrementalPosition = Vector2.zero;
            int lineAccumulator = 0;
            for (int i = 0; i <= 20; i++) {
                float increment = Mathf.Lerp(note.Start, note.Start + note.Duration, (float)i / 20f);
                if (increment > cmds[lineAccumulator].endTime) lineAccumulator++;
                if (lineAccumulator >= cmds.Count) lineAccumulator--;
                incrementalPosition = cmds[lineAccumulator].GetNotePosition(increment, note.Position);
                tailRenderer.SetPosition(i, incrementalPosition + new Vector2(0, 0.001f));
            }

            cap.gameObject.transform.localPosition = incrementalPosition - res;
        }
        
        return res;
    }
    
    public Vector2 CalculateMovingNotePosition() {
        Vector2 res = new Vector2();

        res = cmds[0].GetMobileNotePosition(note.Start + (note.Start + note.Duration) - Conductor.Instance.GetSongTime(), note.Position);

        if (note.Duration > 0) {
            tailRenderer.positionCount = 21;
            Vector2 incrementalPosition = Vector2.zero;
            int lineAccumulator = 0;
            for (int i = 0; i <= 20; i++) {
                float increment = Mathf.Lerp(note.Start, note.Start + note.Duration, (float)i / 20f);
                if (increment > cmds[lineAccumulator].endTime) lineAccumulator++;
                if (lineAccumulator >= cmds.Count) lineAccumulator--;
                incrementalPosition = cmds[lineAccumulator].GetNotePosition(increment, note.Position);
                tailRenderer.SetPosition(i, incrementalPosition + new Vector2(0, 0.001f));
            }

            cap.gameObject.transform.localPosition = incrementalPosition - res;
        }
        
        return res;
    }

    private LineDataCommand DetermineCurrentCMD(float time) {
        int lineAccumulator = 0;
        while (true) {
            if (time > cmds[lineAccumulator].endTime) lineAccumulator++;
            else return cmds[lineAccumulator];
            if (lineAccumulator >= cmds.Count) return cmds[lineAccumulator-1];
        }
    }
}

[Serializable]
public class VisualSet {
    public Sprite head;
    public Sprite cap;
    public Material arcMaterial;
    public Sprite telegraph;
    public string noteTell;
}*/