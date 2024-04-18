local moduleTest1 = {}
local CS_Loader = CS.LoadForAsset
local CS_LoaderMgr = CS.LoadMgr

local GameObject = CS.UnityEngine.GameObject
local SourceType = CS.SourceType
function moduleTest1:Init()
	print("this is test1")
	self.Loader();
end

function moduleTest1.Loader()
	CS_LoaderMgr.Instance:Load("Cube", typeof(GameObject))
	print("excute loader")
end

return moduleTest1