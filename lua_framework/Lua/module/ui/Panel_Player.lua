local uiBase = require("module.ui.uiBase")
---@class ui.Panel_Player:uiBase
local Panel_Player = uiBase:New()
Panel_Player.panelName = "Player"
function Panel_Player:Awake()
	print("Panel_Player Awake!!!!")
end





return Panel_Player
