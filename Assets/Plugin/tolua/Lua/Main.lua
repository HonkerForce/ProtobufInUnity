--主入口函数。从这里开始lua逻辑
function Main()					
	print("logic start")	 		
end

--场景切换通知
function OnLevelWasLoaded(level)
	collectgarbage("collect")
	Time.timeSinceLevelLoad = 0
end

function OnApplicationQuit()
end

function TestToLuaShow(testToLua)
	if testToLua ~= nil then
		print("TestToLuaShow被调用")
	end

	local per = testToLua.person

	local strPrint = string.format("testToLua结构：%s", tostring(testToLua))
	print("testToLua.NUMBER", testToLua.NUMBER)
	print(per.Name, per.Age, tostring(per.Gender), per.Phone)
end