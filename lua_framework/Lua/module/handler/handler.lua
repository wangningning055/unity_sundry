local handler = {}
local KeyCode = CS.UnityEngine.KeyCode
local Input = CS.UnityEngine.Input
local eventId = 0
local CodeType = require("module.handler.codeType")


function handler:Init()
	self.CodeDic = {
		[KeyCode.W] = {[0] = CodeType.W, [1] = CodeType["-W"]},
		[KeyCode.A] = {[0] = CodeType.A, [1] = CodeType["-A"]},
		[KeyCode.S] = {[0] = CodeType.S, [1] = CodeType["-S"]},
		[KeyCode.D] = {[0] = CodeType.D, [1] = CodeType["-D"]},
		[KeyCode.Space] = {[0] = CodeType.Space, [1] = CodeType["-Space"]}
	}
	self.eventDic = {}
	for key, value in pairs(CodeType) do
		self.eventDic[value] = {}
	end
end

function handler:AddCodeEvent(codeType, func, isOne, ...)
	eventId = eventId + 1

	local tab = {[0] = func, [1] =...,  [2] = isOne}
	self.eventDic[codeType][eventId] = tab

	return eventId
end

function handler:RemoveCodeEvent(codeType, eventId)
	-- self.CodeDic[CodeType[codeType]][eventId] = nil
	self.eventDic[codeType][eventId] = nil
end

function handler:Update(delta)
	for key, value in pairs(self.CodeDic) do
		if(Input.GetKeyDown(key)) then
			for k, v in pairs(self.eventDic[value[0]]) do
				if(v[2] == true) then
					v[0](v[1])
				end
			end
		end

		if(Input.GetKeyUp(key)) then
			for k, v in pairs(self.eventDic[value[1]]) do
				if(v[2] == true) then
					v[0](v[1])
				end
			end
		end

		if(Input.GetKey(key)) then
			for k, v in pairs(self.eventDic[value[0]]) do
				if(v[2] == false) then
					v[0](v[1], delta)
				end
			end
		else
			for k, v in pairs(self.eventDic[value[1]]) do
				if(v[2] == false) then
					v[0](v[1], delta)
				end
			end
		end
	end
end














return handler