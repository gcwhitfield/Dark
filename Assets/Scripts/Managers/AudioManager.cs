using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FMODUnity;

public class AudioManager : SingletonPersistant<AudioManager>
{
    public EventReference footstep;
    public EventReference normalDoorOpen;

    // "SetupAudio", "PlayAudio, and "PlayAudioNoRelease" were copied from Joshua Rain's AudioManager.cs
    // https://github.com/gcwhitfield/Advanced-Game-Studio-Game/blob/main/Assets/Scripts/Audio/AudioManager.cs
    public FMOD.Studio.EventInstance SetupAudio(EventReference audio, GameObject gb)
    {
        FMOD.Studio.EventInstance audioInstance;
        audioInstance = RuntimeManager.CreateInstance(audio);
        RuntimeManager.AttachInstanceToGameObject(audioInstance, gb.transform);
        return audioInstance;
    }

    public FMOD.Studio.EventInstance PlayAudio(EventReference audio, GameObject gb)
    {
        FMOD.Studio.EventInstance audioInstance;
        audioInstance = RuntimeManager.CreateInstance(audio);
        RuntimeManager.AttachInstanceToGameObject(audioInstance, gb.transform);
        audioInstance.start();
        audioInstance.release();
        return audioInstance;
    }

    public FMOD.Studio.EventInstance PlayAudioNoRelease(EventReference audio, GameObject gb)
    {
        FMOD.Studio.EventInstance audioInstance;
        audioInstance = RuntimeManager.CreateInstance(audio);
        RuntimeManager.AttachInstanceToGameObject(audioInstance, gb.transform);
        audioInstance.start();
        return audioInstance;
    }
}