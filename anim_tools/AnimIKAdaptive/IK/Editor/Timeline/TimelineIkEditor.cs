// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System.Collections;
using System.Collections.Generic;
using Unity.EditorCoroutines.Editor;
using UnityEditor;
using UnityEngine.Timeline;

public class MovieMaker
{
    private List<TimelineClip> clips = new List<TimelineClip>();


    private IEnumerator MakeMovie(TimelineAsset asset)
    {
        bool createFlag = true;

        IKMovieClipTrack ikMovieClipTrack = null;

        if (asset != null)
        {
            clips.Clear();
            foreach (var track in asset.GetOutputTracks())
            {
                if (track.GetType().Name == "AnimationTrack")
                {
                    foreach (var clip in track.GetClips())
                    {
                        clips.Add(clip);
                    }
                }

                if (track.GetType().Name == "IKMovieClipTrack")
                {
                    createFlag = false;
                    ikMovieClipTrack = track as IKMovieClipTrack;
                    foreach (var clip in track.GetClips())
                    {
                        track.DeleteClip(clip);
                    }
                }
            }

            if (createFlag)
            {
                ikMovieClipTrack = asset.CreateTrack<IKMovieClipTrack>();
            }


            for (int i = 0; i < clips.Count; i++)
            {
                var curClip = clips[i];
                var baseClip = ikMovieClipTrack.CreateClip<IKMovieClip>();
                baseClip.start = curClip.start;
                baseClip.displayName = curClip.displayName;
                baseClip.duration = curClip.duration;
                var newClip = baseClip.asset as IKMovieClip;
                newClip.SetAnimationClip(curClip.animationClip);
            }
        }

        yield return null;
    }

    public IEnumerator Execute()
    {
        var objs = Selection.objects;
        int curNum = 1;
        foreach (var obj in objs)
        {
            TimelineAsset asset = obj as TimelineAsset;
            EditorUtility.DisplayProgressBar("正在处理", asset.name, curNum * 1.0f / objs.Length);
            curNum++;
            yield return MakeMovie(asset);
        }

        yield return null;
        EditorUtility.ClearProgressBar();
        AssetDatabase.SaveAssets();
        AssetDatabase.Refresh();
    }
}

public class TimelineIkEditor
{
#if UNITY_EDITOR
    [MenuItem("Assets/Timeline/创建IkMovieClip")]
    private static void CreateUnitCategoryModel()
    {
        MovieMaker maker = new MovieMaker();
        EditorCoroutineUtility.StartCoroutine(maker.Execute(), maker);
        EditorUtility.DisplayDialog("完成", "全部处理完成", "ok");
    }

#endif
}
