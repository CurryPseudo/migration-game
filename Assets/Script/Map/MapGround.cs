using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

public class MapGround : MonoBehaviour {
    public bool toAlert = false;
    public new SpriteRenderer renderer;
    public IEnumerator currentState;
    public Color alertColor;
    public float alertCycle;
    public float fallTime;
    public float fallDistance;
    public AnimationCurve fallCurve;
    public AnimationCurve colorValueCurve;
    public void ChangeState(IEnumerator next) {
        if(currentState != null) {
            StopCoroutine(currentState);
        }
        currentState = next;
        StartCoroutine(currentState);
    }
    public IEnumerator Main() {
        if(renderer == null) renderer = GetComponentInChildren<SpriteRenderer>();
        currentState = Idle();
        StartCoroutine(currentState);
        yield break;
    }
    public IEnumerator Idle() {
        renderer.color = Color.white;
        while(true) {
            yield return null;
        }
    }
    [Button("Test alert")]
    public void ChangeToAlert() {
        ChangeState(Alert());
    }
    public IEnumerator Alert() {
        float timeCount = 0;
        while(true) {
            yield return null;
            timeCount += Time.deltaTime;
            timeCount = timeCount % alertCycle;
            float value = colorValueCurve.Evaluate(timeCount / alertCycle);
            renderer.color = Color.Lerp(Color.white, alertColor, value);
        }
    }
    public IEnumerator Fall() {
        renderer.color = Color.white;
        GetComponent<MapGridPosition>().enabled = false;
        Vector2 position = transform.position;
        float timeCount = 0;
        while(timeCount < fallTime) {
            yield return null;
            timeCount += Time.unscaledDeltaTime;
            float value = timeCount / fallTime;
            value = fallCurve.Evaluate(value);
            renderer.color = Color.Lerp(Color.white, new Color(1, 1, 1, 0), value);
            var target =  position + Vector2.down * fallDistance * value;
            transform.position = new Vector3(target.x, target.y, transform.position.z);
        }
        while(true) {
            renderer.color = Color.clear;
            yield return null;
        }
    }
    
}