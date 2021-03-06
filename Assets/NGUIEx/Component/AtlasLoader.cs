﻿using System;
using UnityEngine;
using System.Collections.Generic;
using comunity;
using commons;

namespace ngui.ex
{
    public class AtlasLoader : comunity.Script
    {
        [Serializable]
        public class AtlasPair
        {
            public UIAtlas dst;
            public AssetRef asset;
        }

        public bool loadOnStart;
        public AtlasPair[] atlases;

        void Start()
        {
            if (loadOnStart)
            {
                Load(null);
            }
        }

        [ContextMenu ("Load")]
        private void Load()
        {
            Load(null);
        }

        public void Load(Action callback)
        {
            int count = 0;
            for (int i = 0; i < atlases.Length; ++i)
            {
                AtlasPair a = atlases[i];
                log.Debug("Loading atlas {0}", a.asset.path);
                a.asset.LoadAsset<GameObject>(o => {
                    UnityEngine.Object.DontDestroyOnLoad(o);
                    var atlas = o.GetComponent<UIAtlas>();
                    a.dst.replacement = atlas;
                    count++;
                    log.Info("Set atlas {0} to {1}", atlas, a.dst);
                    if (count == atlases.Length)
                    {
                        callback.Call();
                    }
                });
            }
        }
    }
}
