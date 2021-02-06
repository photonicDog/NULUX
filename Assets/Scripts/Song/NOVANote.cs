using System;
using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

public class NOVANote : SerializedMonoBehaviour {
    public Note note;
    public List<LineDataCommand> cmds;
    public List<VisualSet> visualSets;
    public VisualSet currentVisualSet;
    public Animator fade;

    [SerializeField] private LineRenderer tailRenderer;
    
    public SpriteRenderer head = default;
    public SpriteRenderer cap = default;

    [SerializeField] private GameObject hitEffect = default;
    [SerializeField] private SpriteRenderer telegraphSprite;

    private float speedScale = 0;
    private float angle = 0;

    public bool activated;
    
    public void Initialize(Note n, List<LineDataCommand> lc) {
        note = n;
        cmds = lc;
        
        currentVisualSet = visualSets[(int)note.NoteType];
        head.sprite = currentVisualSet.head;
        telegraphSprite.sprite = currentVisualSet.telegraph;
        cap.sprite = currentVisualSet.cap;
        tailRenderer.material = currentVisualSet.arcMaterial;

        if (n.Duration > 0) {
            tailRenderer.gameObject.SetActive(true);
            tailRenderer.gameObject.SetActive(true);
            cap.gameObject.SetActive(true);
        }
    }

    public void FadeIn(float fadeSpeed) {
        activated = true;
        StartCoroutine(Fade());
    }

    public IEnumerator Fade() {
        float startTime = Conductor.Instance.GetSongTime();
        fade.StartPlayback();
        telegraphSprite.gameObject.SetActive(true);
        while (Conductor.Instance.GetSongTime() <= note.Start) {
            float a = Mathf.InverseLerp(startTime, note.Start, Conductor.Instance.GetSongTime());
            fade.Play(0, -1, a);
            yield return new WaitForEndOfFrame();
        }
    }

    public void PlaceNote(NOVALine nl) {
        transform.position = CalculateNotePosition();
    }

    public void HitHold(bool hit) {
        if (hit) {
            Instantiate(hitEffect, transform.position, Quaternion.identity);
        }
    }
    public void Kill(bool hit) {
        if (hit) {
            Instantiate(hitEffect, transform.position, Quaternion.identity);
        }
        
        Destroy(this.gameObject);
        
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
                incrementalPosition = cmds[0].GetNotePosition(increment, note.Position);
                tailRenderer.SetPosition(i, incrementalPosition + new Vector2(0, 0.001f));
            }

            cap.gameObject.transform.localPosition = incrementalPosition - res;
        }
        
        return res;
    }
}

[Serializable]
public class VisualSet {
    public Sprite head;
    public Sprite cap;
    public Material arcMaterial;
    public Sprite telegraph;
}