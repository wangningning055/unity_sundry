local playerMgr = {}
playerMgr.playerList = {}
playerMgr.playerNum = 0
local player = require("module.player.player")
local module = require("module.module")
local CodeType = require("module.handler.codeType")
function playerMgr:Init()
	self.mainPlayer = player:New()
end
function playerMgr:Attack()
	self.mainPlayer:Attack()
end


function playerMgr:Add(player)
	self.playerNum = self.playerNum + 1
	self.playerList[self.playerNum] = player
end

function playerMgr:Update(delta)
	self.mainPlayer:Update(delta)
end

function playerMgr:Delete()
end




return playerMgr