using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Google.Protobuf;
using Google.Protobuf.Reflection;
using Test;
using Test2;
using System.IO;
using LuaInterface;

public class Main : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
	    testLuaCallProtoObj();

	    var peopleType = typeof(people);
	    foreach (var fieldInfo in peopleType.GetFields())
	    {
		    Debug.Log(fieldInfo.Name);
	    }
    }

	// Update is called once per frame
	void Update()
    {
        
    }

    void testCallProtoObj()
    {
	    people per = new people
		{
		    Name = "ddy"
	    };

	    per.Num.Add(1);
	    per.Num.Add(2);

	    byte[] bytes;
	    using (MemoryStream stream = new MemoryStream())
	    {
		    per.WriteTo(stream);

		    bytes = stream.ToArray();
	    }

	    string print = "";
	    foreach (var b in bytes)
	    {
		    print += (int)b + " ";
	    }
	    Debug.Log(print);

	    people parsePer = people.Parser.ParseFrom(bytes);
	    print = parsePer.ToString();
	    Debug.Log(print);
    }

    private static readonly string strLua = @"
		local PerObj = Test2.people()
		PerObj.Name = 'DDY'
		print('haha', PerObj.Name)
	";
	void testLuaCallProtoObj()
	{
		LuaState luaState = new LuaState();
		LuaBinder.Bind(luaState);
		luaState.Start();

		luaState.DoString(strLua, "Main.cs");
	}
}
