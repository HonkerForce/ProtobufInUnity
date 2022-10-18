using System;
using System.IO;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.Diagnostics;
using System.Runtime.InteropServices;
using Unity.VisualScripting;

public class ProtoCompileTool
{
	public static readonly string ProtoFilePath = Path.Combine(Environment.CurrentDirectory, "Assets", "Scripts", "Proto");
	public static readonly string GeneralFilePath = Path.Combine(Environment.CurrentDirectory, "Assets", "Scripts", "Protobuf");

	[MenuItem("Tool/Protobuf/Compile")]
	public static void CompileProto()
	{
#if UNITY_STANDALONE_WIN
		string strProtoc = "protoc.exe";
#else
		string strProtoc = "protoc";
#endif
		strProtoc = Path.Combine(Environment.CurrentDirectory, "Assets", "Scripts", "Proto", strProtoc);

		if (Directory.Exists(ProtoFilePath) == false)
		{
			UnityEngine.Debug.LogError("Proto文件路径非法！");
			return;
		}

		if (Directory.Exists(GeneralFilePath) == false)
		{
			Directory.CreateDirectory(GeneralFilePath);
		}

		string strParam = $"--csharp_out=\"{GeneralFilePath}\" --proto_path=\"{ProtoFilePath}\" {ProtoFilePath}\\*.proto";
		//UnityEngine.Debug.Log(strParam);

		RunCmd(strProtoc, strParam, bIsWaitExit: true);

		UnityEngine.Debug.Log("Proto File Compile Finish!");

		AssetDatabase.Refresh();
	}

	public static Process RunCmd(string strExe, string strParam, string strWorkingDirectory = ".", bool bIsWaitExit = false)
	{
		bool redirectStandardOutput = false;
		bool redirectStandardError = false;
		bool useShellExecute = false;

#if UNITY_STANDALONE_WIN
		redirectStandardOutput = false;
		redirectStandardError = false;
		useShellExecute = true;
#else
		redirectStandardOutput = true;
		redirectStandardError = true;
		useShellExecute = false;
#endif

		if (bIsWaitExit)
		{
			redirectStandardOutput = true;
			redirectStandardError = true;
			useShellExecute = false;
		}

		ProcessStartInfo info = new ProcessStartInfo
		{
			FileName = strExe,
			Arguments = strParam,
			CreateNoWindow = true,
			UseShellExecute = useShellExecute,
			WorkingDirectory = strWorkingDirectory,
			RedirectStandardOutput = redirectStandardOutput,
			RedirectStandardError = redirectStandardError,
		};

		try
		{
			Process process = Process.Start(info);

			if (bIsWaitExit)
			{
				process.WaitForExit();
				if (process.ExitCode != 0)
				{
					throw new Exception($"{process.StandardOutput.ReadToEnd()} {process.StandardError.ReadToEnd()}");
				}
			}

			return process;
		}
		catch (Exception e)
		{
			throw new Exception($"dir: {Path.GetFullPath(strWorkingDirectory)}, command: {strExe} {strParam}", e);
		}

	}
}
