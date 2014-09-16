using System;
using System.Diagnostics;
using System.IO;
using System.Windows.Forms;
using System.Xml;

namespace layer_export
{
	class Program
	{
		static void Main(string[] args)
		{
			if (args.Length != 1)
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


			var doc = new XmlDocument();
			using (var stream = File.Open(args[0], FileMode.Open, FileAccess.Read))
			{
				var xr = XmlReader.Create(stream);
				doc.Load(xr);

			}
			var folder = "C:\\stick\\td2\\svg\\";

			var groups = doc.GetElementsByTagName("g");
			foreach (var g in groups)
			{
				XmlNode xn = g as XmlNode;
				if (xn != null)
				{
					var layer_name = "";
					var id = "";
					foreach (var a in xn.Attributes)
					{
						XmlAttribute xa = a as XmlAttribute;
						if (xa != null)
						{
							if (xa.Name == "inkscape:label")
								layer_name = xa.Value;
							if (xa.Name == "id")
								id = xa.Value;
						}
					}
					if (layer_name != "" && id != "")
					{

						var startInfo = new ProcessStartInfo
						{
							UseShellExecute = false,
							RedirectStandardOutput = true,
							RedirectStandardError = true,
							RedirectStandardInput = true,
							FileName = extF,
							Arguments = "--shell"
						};


						var p = new Process { StartInfo = startInfo };
						if (p.Start())
						{
							p.StandardInput.Write("--file \"" + args[0] + "\" ");
							p.StandardInput.Write("--verb EditDeselect ");
							p.StandardInput.Write("--export-id=" + id + " ");
							p.StandardInput.Write("--export-id-only ");
							p.StandardInput.Write("--export-png=\"" + Path.Combine(folder, layer_name + ".png") + "\" \n");
							p.StandardInput.Write("quit\n");
							//if (p.WaitForExit(10000))
							//File.Delete(temps[i]);
						}
						else
						{
							//File.Delete(temps[i]);
						}
						p.WaitForExit();
					}
				}
			}
		}
	}
}
