local BaseUI = require("module.ui.uiBase")
local Com_Infinity_List = BaseUI:New()

local FindObj = CS.LuaUtils.FindGameObject

function Com_Infinity_List:Awake()
	local t = self._transform
	self.ScrollView = FindObj(t, "Scrool View")
	
end


function Com_Infinity_List:SetData()
	
end

function Com_Infinity_List:Show()
	
end



return Com_Infinity_List