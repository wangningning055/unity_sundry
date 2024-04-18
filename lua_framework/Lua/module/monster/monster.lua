local monster = {}
monster.blood = 100
monster.name = "222"

function monster:new(o, blood, name)
	local instance = o or {}
	setmetatable(instance, self)
	self.__index = self
	instance.blood = blood
	instance.name = name
	return instance
end


function monster:Hurt(num)
	self.blood = self.blood - num
	print("monster : " .. self.name .. " hurt!!!!!" .. "  blood is " .. self.blood)
end



return monster