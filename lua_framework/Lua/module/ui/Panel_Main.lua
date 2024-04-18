local uiBase = require("module.ui.uiBase")
---@class ui.Panel_Main:uiBase
local Panel_Main = uiBase:New()
Panel_Main.panelName = "Panel_Main"
local FindGameObject = CS.LuaUtils.FindGameObject
local FindText = CS.LuaUtils.FindText
local FindButton = CS.LuaUtils.FindButton
local FindImage = CS.LuaUtils.FindImage
local player = require("module.module").player
local module = require("module.module")
function Panel_Main:Awake()
	local t = self._transform
	self.Button_Head = FindButton(t, "Button_Head")
	self.Image_Head = FindImage(t, "Button_Head")
	self.Text_Name = FindText(t, "Text_Name")
	self.Text_PlayerData = FindText(t, "Text_PlayerData")
	self.Button_Attack = FindButton(t, "Button_Attack")
	self.Button_ShowMonster = FindButton(t, "Button_ShowMonster")
	if self.Button_Head then
		self.Button_Head.onClick:AddListener(function ()
			self:OnButton_Head_Click();
		end)
	end
	if self.Button_Attack then
		self.Button_Attack.onClick:AddListener(function ()
			self:OnButton_Attack_Click();
		end)
	end
	if self.Button_ShowMonster then
		self.Button_ShowMonster.onClick:AddListener(function ()
			self:OnButton_ShowMonster_Click();
		end)
	end
	self:Register(module.event.playerHurt, self.Update, self)
end

function Panel_Main:Update(val)
	print("调用了主ui的刷新" .. val)
end

function Panel_Main:OnButton_Head_Click()
	print("OnButttonHeadClick")

end
function Panel_Main:OnButton_Attack_Click()
	print("OButtonAttackClick")
	player:Attack()

end

function Panel_Main:OnButton_ShowMonster_Click()
	print("OnbuttonMonsterClick")

end
return Panel_Main