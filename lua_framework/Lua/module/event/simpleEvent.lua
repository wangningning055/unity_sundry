local simpleEvent = {}


function simpleEvent:New(o)
	local instance = o or {}
	setmetatable(instance, self);
	self.__index = self
	instance.handleList = {}
	instance.evtId = 0;
	print("初始化事件！！！！！！！！！！！！ ")
	return instance;
end


function simpleEvent:register(handl, val)
	local handles = {}
	handles.handl = handl
	handles.val = val
	self.evtId = self.evtId + 1;
	self.handleList[self.evtId] = handles;
	return self.evtId
end



function simpleEvent:unRegister(evtId)
	self.handleList[evtId] = nil
end

function simpleEvent:trigger(...)
	for _, value in pairs(self.handleList) do


		pcall(value.handl, value.val, ...)
	end
end




















return simpleEvent