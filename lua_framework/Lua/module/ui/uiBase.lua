---@class uiBase
local uiBase = {}
---@return uiBase
local uiMgr = require("module.ui.uiMgr")
function uiBase:New(o)
	local instance = o or {}
	setmetatable(instance, self)
	self.__index = self
	return instance
end

local private = {}
uiBase._state = 2 -- ui默认为hide状态
local state_loading = 0
local state_showing = 1
local state_hide = 2
uiBase.private = private;
function uiBase:Init()

end
function uiBase:Awake()
	
end

function uiBase:Show()
	if self._state == state_showing then
		print("已经显示，无法再次显示")
		return
	end
	print("show panel : " .. self.panelName)
	private.show(self)
end


function uiBase:SetData()
	
end

function uiBase:Register(evt, handle, val)
	if (self.evtIdList == nil) then
		self.evtIdList = {}
	end
	local evtId = evt:register(handle, val)
	local list = {}
	list.evt = evt
	list.handl = handle
	self.evtIdList[evtId] = list;
end

function uiBase:UnRegister(evtId)
	local data = self.evtIdList[evtId]
	data.evt:unRegister(evtId)
	data.evt = nil
	data.handl = nil
	data = nil
end

function uiBase:UnRegisterAll()
	for key, value in pairs(self.evtIdList) do
		self:UnRegister(key)
	end
end

function uiBase:Hide()
	if self._state == state_hide then
		print("已经隐藏，无法再次隐藏")
		return
	end
	self.private.hide(self)
end

function uiBase.private.show(self)
	if self._state == state_loading then
		warn("ui正在加载中,无法显示")
	end
	self._state = state_loading
	local aaa = uiMgr.LoadUI(self.panelName)
	self._transform = aaa.transform
	self._transform.gameObject:SetActive(true)
	self._state = state_showing
	self:Awake()
end

function uiBase.private.hide(self)
	if self._state == state_loading then
		warn("ui正在加载中, 无法隐藏")
	end
	self._state = state_hide
	uiMgr.HideUI(self._transform.gameObject, self.panelName)
	self._transform.gameObject:SetActive(false)
	self._transform = nil
	self:UnRegisterAll()
end


return uiBase