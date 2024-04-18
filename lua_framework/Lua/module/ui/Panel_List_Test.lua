local uiBase = require("module.ui.uiBase")
---@class ui.Panel_List_Test:uiBase
local Panel_List_Test = uiBase:New()
local Com_InfinityList = require("module.ui.com.Com_Infinity_List")

function Panel_List_Test:Awake()
	Com_InfinityList:Awake()
end

function Panel_List_Test:Update()
	
end





return Panel_List_Test