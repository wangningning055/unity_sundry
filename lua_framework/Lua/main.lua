local main = {}
local luaCallCharp = CS.LuaCallCharpTest;
local module = require("module.module")
local test = require"module.Test.test"
local KeyCode = CS.UnityEngine.KeyCode
local Input = CS.UnityEngine.Input
function main.Start()
	print("this is lua begin and AssetBundleUpdate!!!!!!")
	local luaCallCharpCls = luaCallCharp()
	print(luaCallCharpCls.num)
	luaCallCharpCls:print()
	module.Init()
end


function main.Update(deltTime)
	module.Update(deltTime)
	if(Input.GetKeyDown(KeyCode.T)) then
		test.test()
	end
	if(Input.GetKeyDown(KeyCode.Y)) then
		test.test1()
	end
	if(Input.GetKeyDown(KeyCode.U)) then
		test.test2()
	end
	if(Input.GetKeyDown(KeyCode.I)) then
		test.test3()
	end
	if(Input.GetKeyDown(KeyCode.O)) then
		test.test4()
	end
end

function main.LateUpdate(deltTime)
end

return main