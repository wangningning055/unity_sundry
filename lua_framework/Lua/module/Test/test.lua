local test = {}
local ui = require("module.ui.ui")
function test:New(o)
	local instance = o or {}
	setmetatable(o, self)
	self.__index = self;
	return instance;
end

--T
function test.test()

	print("this is T down!!")
	ui.Panel_Main:Show()

end
--Y
function test.test1()

	print("this is T down!!")
	ui.panel_player:Hide()

end
--U
function test.test2()

	print("this is T down!!")
	local function aaaa()
		print("hahahahahahahahah")
		test.aaaa()
	end
	local function bbb()
		print("12312312313123")
	end
	xpcall(aaaa, bbb)

end
--i
function test.test3()

	print("this is T down!!")
	ui.panel_player:Show()

end
--o
function test.test4()

	print("this is T down!!")
	ui.panel_player:Show()

end
return test