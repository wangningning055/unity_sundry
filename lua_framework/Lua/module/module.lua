local module = {}


function module.Init()
	print("module Init")
	module.test1 = require("module.module1.moduleTest1")
	module.test2 = require("module.module2.moduleTest2")
	module.uiMgr = require("module.ui.uiMgr")
	module.poolMgr = require("module.pool.poolMgr")
	module.player = require("module.player.playerMgr")
	module.monster = require("module.monster.monster")
	module.event = require("module.event.event")
	module.handler = require("module.handler.handler")
	module.handler:Init()
	module.test1:Init()
	module.test2.Init()
	module.uiMgr.Init()
	module.poolMgr.Init()
	module.player:Init()
end
function module.Update(delta)
	module.handler:Update(delta)
	module.player:Update(delta)
	
end
return module;