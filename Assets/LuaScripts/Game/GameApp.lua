-- 2025年05月29日 21:49:20 星期四
local GameApp = {}

local CS_ResourceManager = CS.ResourceManager

function EnterLoginScene()
    print("Entering game...")
    local asset = CS_ResourceManager.Instance:LoadAsset("Resource/Prefabs/Cube.prefab")
    CS.UnityEngine.GameObject.Instantiate(asset)
end

function GameApp.EnterGame()

    EnterLoginScene()
end

return GameApp
