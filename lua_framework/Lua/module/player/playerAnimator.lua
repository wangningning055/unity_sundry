local playerAnimator = {}
local module = require("module.module")
local CodeType = require("module.handler.codeType")
local startSpeed = 2
function playerAnimator:New(o)
	local instance = o or {}
	setmetatable(instance, self)
	self.__index = self
	return instance;
end

function playerAnimator:Init(playerObj, animator, player)
	self.obj = playerObj
	self.animator = animator
	self.layerCtr = 0;
	self.player = player

	self.debugTime = 0.1
	print("Animator初始化！！！")
end

-- function playerAnimator:SetLayerWeight(delta)
-- 	-- print(delta)
-- 	self.layerCtr = self.layerCtr + delta * startSpeed > 1 and 1 or self.layerCtr + delta *startSpeed
-- 	-- print("Down!!! ", self.layerCtr)
-- end
-- function playerAnimator:SetLayerWeight2(delta)
-- 	self.layerCtr  = self.layerCtr - delta * startSpeed < 0 and 0 or self.layerCtr - delta * startSpeed
-- 	-- print(delta)
-- 	-- print("Up!!! ", self.layerCtr)
	
-- end

function playerAnimator:Update(delta)
	-- local data = self.player.handler:GetTend() > 1 and 1 or self.player.handler:GetTend()
	-- self.animator:SetLayerWeight(1, data)
	if(self.debugTime > 0.1) then
		-- print(self.player.handler:GetTend().x.."      "..self.player.handler:GetTend().y)
		self.debugTime = 0
	else
		self.debugTime = self.debugTime + delta
	end
end

function playerAnimator:SetBool()

end



return playerAnimator