local playerHandler = {}
local module = require("module.module")
local CodeType = require("module.handler.codeType")
local Vector2 = CS.UnityEngine.Vector2
local speed = 4
local Vector3 = CS.UnityEngine.Vector3
local event = require("module.event.event")
function playerHandler:Init()
	self.inputTend = Vector2.zero
	module.handler:AddCodeEvent(CodeType.W, self.Front, false, self)
	module.handler:AddCodeEvent(CodeType["-W"], self.UpFront, false, self)

	module.handler:AddCodeEvent(CodeType.S, self.Back, false, self)
	module.handler:AddCodeEvent(CodeType["-S"], self.UpBack, false, self)

	module.handler:AddCodeEvent(CodeType.A, self.Left, false, self)
	module.handler:AddCodeEvent(CodeType["-A"], self.UpLeft, false, self)

	module.handler:AddCodeEvent(CodeType.D, self.Right, false, self)
	module.handler:AddCodeEvent(CodeType["-D"], self.UpRight, false, self)
end

function playerHandler:GetTend()
	return self.inputTend
end
function playerHandler:Up()
	-- body
end

function playerHandler:Down()
	-- body
end

function playerHandler:Front(delta)
	self.inputTend.y = (self.inputTend.y + delta * speed) > 1 and 1 or self.inputTend.y + delta * speed
	event.tendKeyDown:trigger(self.inputTend)
end

function playerHandler:Back(delta)

	self.inputTend.y = self.inputTend.y - delta * speed < -1 and -1 or self.inputTend.y - delta * speed
	event.tendKeyDown:trigger(self.inputTend)
	-- body
end

function playerHandler:Left(delta)
	self.inputTend.x = self.inputTend.x - delta * speed < -1 and -1 or self.inputTend.x - delta * speed
	event.tendKeyDown:trigger(self.inputTend)
	-- body
end

function playerHandler:Right(delta)
	self.inputTend.x = self.inputTend.x + delta * speed > 1 and 1 or self.inputTend.x + delta * speed
	event.tendKeyDown:trigger(self.inputTend)
	-- body
end

function playerHandler:UpFront(delta)
	if(self.inputTend.y > 0) then
		self.inputTend.y = (self.inputTend.y - delta * speed) < 0 and 0 or self.inputTend.y - delta * speed
		event.tendKeyDown:trigger(self.inputTend)
	end
end

function playerHandler:UpBack(delta)
	if self.inputTend.y < 0 then
		self.inputTend.y = self.inputTend.y + delta * speed > 0 and 0 or self.inputTend.y + delta * speed
		event.tendKeyDown:trigger(self.inputTend)
	-- body
	end
end

function playerHandler:UpLeft(delta)
	if(self.inputTend.x < 0) then
		self.inputTend.x = self.inputTend.x + delta * speed > 0 and 0 or self.inputTend.x + delta * speed
		event.tendKeyDown:trigger(self.inputTend)
	-- body
	end
end

function playerHandler:UpRight(delta)
	if(self.inputTend.x > 0) then
		self.inputTend.x = self.inputTend.x - delta * speed < 0 and 0 or self.inputTend.x - delta * speed
		event.tendKeyDown:trigger(self.inputTend)
	-- body
	end
end
function playerHandler:UpKey(delta)
	-- if((self.inputTend.sqrMagnitude) > 0.01) then
	-- 	self.inputTend.x = self.inputTend.x > 0 and self.inputTend.x - delta or self.inputTend.x + delta
	-- 	self.inputTend.y = self.inputTend.y > 0 and self.inputTend.y - delta or self.inputTend.y + delta
	-- end
end










return playerHandler