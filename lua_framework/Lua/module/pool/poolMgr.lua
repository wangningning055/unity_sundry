local poolMgr = {}
poolMgr.objPool = {}
poolMgr.uiPool = require("module.pool.uiPool")

function poolMgr.Init()
	poolMgr.uiPool:Init()
end
function poolMgr:GetObj()
	
end

function poolMgr:GetUI(uiName)
	self.uiPool:Out(uiName)
end

function poolMgr:InUI(obj, uiName)
	self.uiPool:In(obj, uiName)
end

return poolMgr