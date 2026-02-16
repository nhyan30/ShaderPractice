using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Video;

public class VideoPlayerScript : MonoBehaviour
{
    RawImage rawImage;
    VideoPlayer videoPlayer;
    private void Awake()
    {
        rawImage = GetComponent<RawImage>();
        videoPlayer = GetComponent<VideoPlayer>();
        RenderTexture rt = new RenderTexture((int)videoPlayer.clip.width, (int)videoPlayer.clip.height, 32);
        videoPlayer.targetTexture = rt;
        rawImage.texture = rt;
    }

}
