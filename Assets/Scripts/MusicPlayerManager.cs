using Sirenix.OdinInspector;
using Sirenix.Serialization;
using UnityEngine;
using Debug = System.Diagnostics.Debug;

public class MusicPlayerManager : SerializedMonoBehaviour
{
    [OdinSerialize] private GameObject MusicPlayerPrefab { get; set; }
    private void Awake()
    {
        if(GameObject.FindGameObjectsWithTag("MusicPlayer").Length != 0) return;
        Debug.Assert(Camera.main != null, "Camera.main != null");
        var musicPlayer = Instantiate(MusicPlayerPrefab, Camera.main.transform.position, Quaternion.identity);
        DontDestroyOnLoad(musicPlayer);
    }
}
