-- 2025年05月30日 14:46:18 星期五
local UIManager = {}
local ResourceManager = CS.ResourceManager
local uiPrefabs = {}
local uiRoot = nil

function UIManager:OnInit()

    uiRoot = ResourceManager.Instance:LoadAsset("Resource/Prefabs/UI/UIRoot.prefab")
    if uiRoot == nil then
        print("UIManager:OnInit - Canvas not found")
    end
end

function UIManager:ShowUI(uiName)
    if uiName == nil then
        return
    end

    local uiPrefab = ResourceManager.Instance:LoadAsset("Resource/Prefabs/UI/" .. uiName .. ".prefab")
    if uiPrefab == nil then
        print("UIManager:ShowUI - Failed to load UI prefab: " .. uiName)
        return
    end

    local uiInstance = CS.UnityEngine.GameObject.Instantiate(uiPrefab)
    table.insert(uiPrefabs, uiInstance)
    return uiInstance
end

function UIManager:HideUI(uiInstance)
    if uiInstance == nil then
        return
    end

    uiInstance:SetActive(false)
    for i, v in ipairs(uiPrefabs) do
        if v == uiInstance then
            table.remove(uiPrefabs, i)
            break
        end
    end
end

function UIManager:OnDestroy()

end

return UIManager
