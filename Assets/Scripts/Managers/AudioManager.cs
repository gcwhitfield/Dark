using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FMODUnity;

public class AudioManager : SingletonPersistant<AudioManager>
{
    public EventReference footstep;
    public EventReference normalDoorOpen;
    public EventReference pageTurn;

    // "SetupAudio", "PlayAudio, and "PlayAudioNoRelease" were derived from Joshua Rain's AudioManager.cs
    // https://github.com/gcwhitfield/Advanced-Game-Studio-Game/blob/main/Assets/Scripts/Audio/AudioManager.cs
    public FMOD.Studio.EventInstance SetupAudio(EventReference audio, GameObject gb = null)
    {
        FMOD.Studio.EventInstance audioInstance;
        audioInstance = RuntimeManager.CreateInstance(audio);
        if (gb != null)
        {
            RuntimeManager.AttachInstanceToGameObject(audioInstance, gb.transform);
        }
        return audioInstance;
    }

    public FMOD.Studio.EventInstance PlayAudio(EventReference audio, GameObject gb = null)
    {
        FMOD.Studio.EventInstance audioInstance;
        audioInstance = RuntimeManager.CreateInstance(audio);
        if (gb != null)
        {
            RuntimeManager.AttachInstanceToGameObject(audioInstance, gb.transform);
        }
        audioInstance.start();
        audioInstance.release();
        return audioInstance;
    }

    public FMOD.Studio.EventInstance PlayAudioNoRelease(EventReference audio, GameObject gb = null)
    {
        FMOD.Studio.EventInstance audioInstance;
        audioInstance = RuntimeManager.CreateInstance(audio);
        if (gb != null)
        {
            RuntimeManager.AttachInstanceToGameObject(audioInstance, gb.transform);
        }
        audioInstance.start();
        return audioInstance;
    }
}
