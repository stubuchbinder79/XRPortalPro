using UnityEngine;
using UnityEngine.Video;

public class VideoWorld : World
{
    [Tooltip("is this a local video clip or streamed from a url?")]
    public VideoSource videoSource = VideoSource.VideoClip;

    [Tooltip("url of video")]
    public string videoUrl = "http://www.stubuchbinder.com/video/beach.mp4";

    // VideoPlayer component attached to the VideoSphere game object
    public VideoPlayer videoPlayer { get; private set; }

    private void Awake()
    {
        videoPlayer = GetComponentInChildren<VideoPlayer>();
    }

    private void Start()
    {
        videoPlayer.source = videoSource;

        if(videoSource == VideoSource.Url)
        {
            videoPlayer.url = videoUrl;
            videoPlayer.playOnAwake = false;
            Debug.Log("video url set to: " + videoUrl);
            videoPlayer.Prepare();
        }
    }

    private void OnEnable()
    {
        videoPlayer.prepareCompleted += VideoPlayer_prepareCompleted;
    }

    private void OnDisable()
    {
        videoPlayer.prepareCompleted -= VideoPlayer_prepareCompleted;
    }


    private void VideoPlayer_prepareCompleted(VideoPlayer source)
    {
        Debug.LogFormat("VideoPlayer.prepare completed!!");
        source.Play();
    }
}
