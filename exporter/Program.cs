using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace exporter
{
	class Program
	{
		static void Main(string[] args)
		{
			//for (int i = 0; i < args.Length; i++)
			//MessageBox.Show(args[i]);
			//MessageBox.Show(Environment.CurrentDirectory);

			if (args.Length < 2)
			{
				MessageBox.Show("Wrong number of arguments. Program done.");
				return;
			}

			string inkF = "";
			var extF = "";
			//			if (args[0].Contains("share"))
			//			{
			//				extF = args[0].Substring(0, args[0].IndexOf("share") - 1) + "inkscape.com";
			//			}
			//			else
			//			{
			extF = @"""C:\Program Files (x86)\Inkscape\inkscape.com""";
			//}

			var folder = "C:\\stick\\td2\\svg\\";

			string[] temps = new string[args.Length];
			for (int i = 0; i < args.Length - 1; i++)
			{
				temps[i] = Path.GetTempFileName();
				File.Copy(args[args.Length - 1], temps[i], true);
			}

			for (int i = 0; i < args.Length - 1; i++)
			{
				var idLine = args[i];
				var group = idLine.Substring(5);

				var startInfo = new ProcessStartInfo
				{
					UseShellExecute = false,
					RedirectStandardOutput = true,
					RedirectStandardError = true,
					RedirectStandardInput= true,
					FileName = extF,
					Arguments = "--shell"
				};

				var p = new Process { StartInfo = startInfo };
				if (p.Start())
				{
					p.StandardInput.Write("--file \"" + temps[i] + "\" ");
					p.StandardInput.Write("--verb EditDeselect ");
					p.StandardInput.Write("--export-id=" + group + " ");
					p.StandardInput.Write("--export-id-only ");
					p.StandardInput.Write("--export-png=\"" + Path.Combine(folder, group + ".png") + "\" \n");
					p.StandardInput.Write("quit\n");
					if (p.WaitForExit(10000))
						File.Delete(temps[i]);
				}
				else
				{
					File.Delete(temps[i]);
				}
				//Console.ReadKey();
			}
		}
	}
}
