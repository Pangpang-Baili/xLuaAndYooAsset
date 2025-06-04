using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using YooAsset;
using YooAsset.Editor;

public class LuaRule : IFilterRule
{
    public bool IsCollectAsset(FilterRuleData data)
    {
        return Path.GetExtension(data.AssetPath) == ".lua";
    }
}
