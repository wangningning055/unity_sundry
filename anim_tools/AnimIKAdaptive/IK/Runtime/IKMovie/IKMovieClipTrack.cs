// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System.Collections.Generic;
using System.Linq;
using UnityEngine.Timeline;
using UnityEngine;
using XLua;
using vw_animation_ik_runtime;
[TrackColor(0.0f, 1.0f, 0.0f)]
[TrackClipType(typeof(IKMovieClip))]
[TrackBindingType(typeof(TrackBindingData))]
[LuaCallCSharp]
public class IKMovieClipTrack : TrackAsset
{
    private static List<TimelineClip> clips = new List<TimelineClip>();

    protected override void OnCreateClip(TimelineClip clip)
    {

        var parent = clip.GetParentTrack();

        TimelineAsset asset = parent.parent as TimelineAsset;
        foreach (var track in asset.GetOutputTracks())
        {
            if (track.name == "PlayerTrack")
            {
                foreach (var cp in track.GetClips())
                {
                    clips.Add(cp);
                }
            }
        }

        if (m_Clips.Count > clips.Count)
        {
            return;
        }

        int count = m_Clips.Count - 1;
        var cs = parent.GetClips().ToList();
        var c = cs[count];
        c.start = clips[count].start;
        c.duration = clips[count].duration;
        c.displayName = clips[count].displayName;

        var newClip = c.asset as IKMovieClip;
        newClip.SetAnimationClip(clips[count].animationClip);

        // parent.start = clips[count].start;
    }
}
