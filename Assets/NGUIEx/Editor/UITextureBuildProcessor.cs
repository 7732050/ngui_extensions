﻿
using UnityEngine;
using UnityEditor;
using ngui.ex;
using build;
using comunity;

namespace ngui.ex
{
    public class UITextureBuildProcessor : ComponentBuildProcess
    {
        protected override void VerifyComponent(Component comp)
        {
            UITexture tex = comp as UITexture;
            TexLoader l = tex.GetComponent<TexLoader>();
            if (tex.mainTexture != null && AssetBundlePath.inst.IsCdnAsset(tex.mainTexture)
                && (l == null || IsTextureMismatch(tex)))
            {
                TexSetter setter = tex.GetComponentEx<TexSetter>();
                var aref = new AssetRef();
                aref.cdn = true;
                aref.SetPath(tex.mainTexture);
                setter.textures.Clear();
                setter.textures.Add(aref);
                BuildScript.SetDirty(comp.gameObject);
                BuildScript.SetDirty(setter);
            }
        }

        // check if applied by prefab and instance has different texture
        private bool IsTextureMismatch(UITexture tex)
        {
            TexSetter s = tex.GetComponent<TexSetter>();
            if (s == null || s.textures.Count == 0)
            {
                return false;
            }
            return s.textures.Count == 1 && s.textures[0].path != EditorAssetUtil.GetAssetRelativePath(tex.mainTexture);
        }
        
        protected override void PreprocessComponent(Component comp)
        {
        }
        
        protected override void PreprocessOver(Component c)
        {
        }
        
        public override System.Type compType
        {
            get
            {
                return typeof(UITexture);
            }
        }
    }
}