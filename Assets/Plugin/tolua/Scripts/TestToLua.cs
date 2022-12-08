using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using LuaInterface;

public enum GENDER
{
    NONE = 0,
    man = 1,
    woman = 2,
}

public class Person
{
    public string Name { get; set; }
    public int Age { get; set; }
    public GENDER Gender { get; set; }
    public string Phone { get; set; }

    public Person()
    {
	    Name = "";
	    Age = 0;
        Gender = GENDER.NONE;
        Phone = "";
    }
}

public class TestToLua : MonoBehaviour
{
	public const string strLua =
		@"
			local obj = UnityEngine.GameObject.Find('objPoint')
			obj.name = 'ddy'
			print('Hello World!')

			function test(gameObject)
				if gameObject ~= nil then
					print(type(gameObject))
				end
				local trans = gameObject.transform
				gameObject:AddComponent(typeof(UnityEngine.ParticleSystem))
				trans.position = UnityEngine.Vector3(0, 1, 0)
			end

			local per = Person()
			print('¥¥Ω®Person')
			per.Name = 'DDY'
		";

    public GameObject objPoint;

	public static readonly int NUMBER;

    public Person person;

	static TestToLua()
    {
	    NUMBER = (int)GENDER.man;
    }
	// Start is called before the first frame update
	void Start()
	{
		objPoint = new GameObject("objPoint");
		person = new Person() { Name = "DongDy", Age = 22, Gender = GENDER.man, Phone = "15853753910" };
	}

    // Update is called once per frame
    void Update()
    {

    }

    void OnGUI()
    {
	    if (GUI.Button(new Rect(Vector2.one, new Vector2(50, 30)), "≤‚ ‘"))
	    {
		    LuaState luaState = new LuaState();
			// luaState.AddSearchPath();
		    LuaBinder.Bind(luaState);
            luaState.Start();
            luaState.DoString(strLua, "TestToLua.cs");
			luaState.DoFile("Main.lua");

            LuaFunction luaFunc = luaState.GetFunction("test");
            luaFunc.BeginPCall();
			luaFunc.Push(objPoint);
			luaFunc.PCall();
			luaFunc.EndPCall();
			luaFunc.Dispose();
			luaFunc = luaState.GetFunction("TestToLuaShow");
			luaFunc.BeginPCall();
			luaFunc.Push(this);
			luaFunc.PCall();
			luaFunc.EndPCall();
			luaFunc.Dispose();
	    }
    }
}
