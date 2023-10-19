// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.Timeline;
using vw_animation_ik_runtime;

[Serializable]
public class IKMovieClip : PlayableAsset, ITimelineClipAsset
{
    public ExposedReference<AnimationClip> Clip;

    public IKSampleData Data;

    public ClipCaps clipCaps
    {
        get { return ClipCaps.None; }
    }

    public void SetIKSampleData(IKSampleData data)
    {
        Data = data;
    }

    public void SetAnimationClip(AnimationClip clip)
    {
        Clip = new ExposedReference<AnimationClip>();
        Clip.defaultValue = clip;
    }

    private IKMovieClipBehaviour template = new IKMovieClipBehaviour();

    public override Playable CreatePlayable(PlayableGraph graph, GameObject owner)
    {
        var playable = ScriptPlayable<IKMovieClipBehaviour>.Create(graph, template);
        IKMovieClipBehaviour clone = playable.GetBehaviour();
        clone.animationClip = Clip.Resolve(graph.GetResolver());
        clone._iKSampleData = Data;
        return playable;
    }
}