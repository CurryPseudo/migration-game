using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Sirenix.OdinInspector;
using Sirenix.Serialization;
public class MapController : SerializedMonoBehaviour {
    public SpriteRenderer blackEffectRenderer;
    public float blackFadeTime = 2;
    public List<AudioSource> voice;
    public House destroyingHouse = null;
    public new Camera camera;
    public Vector2 CameraPosition {
        get {
            return camera.transform.position;
        }
        set {
            camera.transform.position = new Vector3(value.x, value.y, camera.transform.position.z);
        }
    }
    [OdinSerialize]
    public Map map;
    public float shakePower;
    public float shakeSpeed;
    public float shakeTime;
    public float cameraMoveSpeed;
    private void OnDrawGizmos() {
        for(int i = 0; i < map.size.x + 1; i++) {
            float x = i;
            Vector2 start = new Vector2(x - 0.5f, -0.5f);
            Vector2 end = new Vector2(x - 0.5f, map.size.y - 0.5f);
            Gizmos.DrawLine(map.MapToWorldPoint(start), map.MapToWorldPoint(end));
        }
        for(int i = 0; i < map.size.y + 1; i++) {
            float y = i;
            Vector2 start = new Vector2(-0.5f,y - 0.5f);
            Vector2 end = new Vector2(map.size.x - 0.5f, y - 0.5f);
            Gizmos.DrawLine(map.MapToWorldPoint(start), map.MapToWorldPoint(end));
        }
    }
    [Button("Move on")]
    public void MoveOn() {
        StartCoroutine(MoveOnProcess());
    }
    public IEnumerator MoveOnProcess() {
        Time.timeScale = 0;
        voice[0].Play();
        Func<float> getRandom = () => UnityEngine.Random.value * shakeSpeed;
        Vector2 originShake = new Vector2(getRandom(), getRandom());
        {
            float timeCount = 0;
            Vector2 origin = CameraPosition;
            Func<Vector2> noise = () => new Vector2(Mathf.PerlinNoise(originShake.x, 0), Mathf.PerlinNoise(0, originShake.y)) * 2 - new Vector2(1, 1);
            CameraPosition = origin + shakePower * noise();
            while(timeCount < shakeTime) {
                yield return null;
                timeCount += Time.unscaledDeltaTime;
                originShake += new Vector2(1, 1) * shakeSpeed * Time.unscaledDeltaTime;
                CameraPosition = origin + shakePower * noise();
            }
            CameraPosition = origin;
        }
        yield return new WaitForSecondsRealtime(0.5f);
        map.FallAllCurrentGround();
        yield return new WaitForSecondsRealtime(1f);
        voice[1].Play();
        Vector2 moveDirection = map.MapToWorldDirection(new Vector2(1, 0));
        float moveTime = moveDirection.magnitude / cameraMoveSpeed;
        {
            Vector2 origin = CameraPosition;
            float timeCount = 0;
            while(timeCount < moveTime) {
                yield return null;
                timeCount += Time.unscaledDeltaTime;
                float value = timeCount / moveTime;
                CameraPosition = origin + moveDirection * value;
            }
            CameraPosition = origin + moveDirection;
        }

        map.MoveOnActive();
        if(destroyingHouse != null) {
            voice[2].Play();
            yield return StartCoroutine(destroyingHouse.Destroying());
            {
                float timeCount = 0;
                while(timeCount < blackFadeTime) {
                    yield return null;
                    timeCount += Time.unscaledDeltaTime;
                    blackEffectRenderer.color = Color.Lerp(Color.clear, Color.black, timeCount / blackFadeTime);
                }
            }
            yield return new WaitForSecondsRealtime(1f);
            Time.timeScale = 1;
            SceneManager.LoadScene(1);
        }
        else {
            Time.timeScale = 1;
        }
        foreach(var player in UnityEngine.Object.FindObjectsOfType<PlayerController>()) {
            var pos = player.player.PositionInMap;
            pos = map.ScreenClamp(pos);
            player.Position = map.MapToWorldPoint(pos);
        }
    }
    public IEnumerator MainProcess() {
        while(true) {
            yield return StartCoroutine(map.WaitToMoveOn());
            yield return StartCoroutine(MoveOnProcess());
        }
    }
    public IEnumerator Main() {
        blackEffectRenderer.color = Color.black;
        map._Init();
        float timeCount = 0;
        while(timeCount < blackFadeTime) {
            yield return null;
            timeCount += Time.unscaledDeltaTime;
            blackEffectRenderer.color = Color.Lerp(Color.black, Color.clear, timeCount / blackFadeTime);
        }
        yield return StartCoroutine(MainProcess());
    }
    public void Update() {
        
    }

}