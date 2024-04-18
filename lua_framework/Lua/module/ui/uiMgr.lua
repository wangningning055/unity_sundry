local uiMgr = {}
local ui = require("module.ui.ui")
local pool = require("Lua.module.pool.poolMgr")
local GameObject = CS.UnityEngine.GameObject
function  uiMgr.Init()
	print("uiMgr Init")
	uiMgr.InitPanelList()
	uiMgr.uiRoot = GameObject.Find("Canvas")
	print("ui init done")
end

function uiMgr.InitPanelList()
	ui.panel_player = require("module.ui.Panel_Player")
	ui.Panel_Main = require("module.ui.Panel_Main")
	ui.Panel_List_Test = require("module.ui.Panel_List_Test")
end

function uiMgr.LoadUI(uiName)
	local obj = pool.uiPool:Out(uiName)
	obj.transform:SetParent(uiMgr.uiRoot.transform, false)
	return obj
end
function uiMgr.HideUI(obj, uiName)
	pool.uiPool:In(obj, uiName)
end


return uiMgr