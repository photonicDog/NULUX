// NOVALine
using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

public class NOVALineDeprecated : SerializedMonoBehaviour
{
	public LineDataCommand currentCMD;

	private Vector2 maxBound;

	public Sprite telegraph;
	public bool activated;
	
	[SerializeReference]
	private Transform lineObject;

	private Bounds bounds;

	private LineRendererCircle lrc;

	public float LineObjectScale => lineObject.localScale.y;

	private void Awake()
	{
	}

	public void SetActivated(bool active) {
		lineObject.gameObject.SetActive(active);
		activated = active;
		if (currentCMD.data.Style == LineStyle.PULSE) lrc = lineObject.GetComponent<LineRendererCircle>();
	}
	
	public void FadeIn() {
		SetActivated(true);
		if (currentCMD.data.Style == LineStyle.PULSE) {
			StartCoroutine(PulseFade());
		}
		else StartCoroutine(Fade());
	}
	
	public IEnumerator Fade() {
		SpriteRenderer lineSpr = lineObject.GetComponent<SpriteRenderer>();
		if (currentCMD.data.Style == LineStyle.CIRCLE) lineSpr = lineObject.GetChild(0).GetComponent<SpriteRenderer>();
		float startTime = Conductor.Instance.GetSongTime();
		float catchConductor = Conductor.Instance.playing ? Conductor.Instance.GetSongTime() : 0;
		while (catchConductor <= currentCMD.startTime) {
			float a = Mathf.InverseLerp(startTime, currentCMD.startTime, Conductor.Instance.GetSongTime());
			var color = lineSpr.color;
			color = new Color(color.r, color.g, color.b, a);
			lineSpr.color = color;
			yield return new WaitForEndOfFrame();
			catchConductor = Conductor.Instance.playing ? Conductor.Instance.GetSongTime() : 0;
		}
	}
	
	public IEnumerator PulseFade() {
		LineRenderer lineSpr = lineObject.GetComponent<LineRenderer>();
		float startTime = Conductor.Instance.GetSongTime();
		while (Conductor.Instance.GetSongTime() <= currentCMD.startTime) {
			float a = Mathf.InverseLerp(startTime, currentCMD.startTime, Conductor.Instance.GetSongTime());
			var color = lineSpr.startColor;
			color = new Color(color.r, color.g, color.b, a);
			lineSpr.startColor = color;
			lineSpr.endColor = color;
			yield return new WaitForEndOfFrame();
		}
	}

	public void CalculateHitLineMovement(float currentBeat)
	{
		CalculateHitLineMovement(currentCMD, currentBeat, lineObject);
	}

	public void CalculateHitLineMovement(LineDataCommand lc, float currentBeat, Transform objTransform)
	{
		if (lc != null && !(objTransform == null)) {
			LineState state = lc.CalculateLineState(currentBeat);
			
			if (currentCMD.data.Style == LineStyle.PULSE) {
				lrc.radius = state.scale;
			}
			if (currentCMD.data.Style == LineStyle.CIRCLE) {
				lineObject.GetChild(0).localScale = new Vector3(lineObject.GetChild(0).localScale.x, state.scale, 1);
				lineObject.GetChild(0).localPosition = new Vector3(0, state.scale/2, 0);
			}
			else {
				lineObject.localScale = new Vector3(lineObject.localScale.x, state.scale, 1);
			}
			
			lineObject.transform.position = state.position;
			lineObject.transform.rotation = state.rotation;

			
			if (lc.endTime < Conductor.Instance.GetSongTime())
			{
				Destroy(objTransform.gameObject);
				lrc = null;
			}
		}
	}
}
