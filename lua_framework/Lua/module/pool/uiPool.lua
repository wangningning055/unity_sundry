local uiPool = {}
uiPool.pool = {}
local CS_Loader = CS.LoadForAsset
local CS_LoaderMgr = CS.LoadMgr
local GameObject = CS.UnityEngine.GameObject
-- local SourceType = CS.SourceType

function uiPool:Init()
	self.poolObj = GameObject.Find("uiPool")
end
function uiPool:Out(name)
	local res = uiPool.pool[name]
	if res == nil then
		print("重新加载")
		local obj = CS_LoaderMgr.Instance:Load(name, typeof(GameObject))
		print(obj)
		return obj
	else
		print("从池中获取")
		return res
	end

end

function uiPool:In(obj, name)
	self.pool[name] = obj
	obj.transform:SetParent(self.poolObj.transform)
end


return uiPool