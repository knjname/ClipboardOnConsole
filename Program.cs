using System;
using System.IO;
using System.Collections.Specialized;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;

namespace ClipboardOnConsole
{

	class ClipboardHelper
	{
		[STAThread]
		static void Main(string[] args)
		{
			// argument analysis
			if (args.Length == 0)
			{
				IntoClipboard(GetStringFromSTDIN());
			}
			else
			{
				string option = args[0].Replace("/", "").Replace("-", "").ToLower();

				switch (option)
				{
					case "show":
					case "s":
					case "display":
					case "d":
						System.Console.Write(Clipboard.GetText());
						break;
					case "fd":
						if(Clipboard.ContainsFileDropList())
						{
							foreach( string eachFileSymbol in Clipboard.GetFileDropList())
							{
								Console.WriteLine(eachFileSymbol);
							}
						}
						break;
					case "f":
					case "fi":
						StringCollection availableFileNames = new StringCollection();
						foreach(string eachFilePath in Console.In.ReadToEnd().Split('\n'))
						{
							if(File.Exists(eachFilePath) || ( (option != "fi") && Directory.Exists(eachFilePath) ))
							{
								availableFileNames.Add(Path.GetFullPath(eachFilePath));
							}
						}

						if (availableFileNames.Count > 0)
						{
							Clipboard.SetFileDropList(availableFileNames);
						}
						else
						{
							Console.Error.WriteLine("No entories were selected.");
						}
						break;
					case "help":
					case "h":
					case "?":
					default:
						ShowHelps();
						break;
				}
			}

		}

		private static void ShowHelps()
		{
			System.Console.WriteLine(
@"You can specify one of these options (case insensitive):

[h]elp / ?         : Shows this help message.
[s]how / [d]isplay : Shows the content of your clipboard.
fd                 : Shows file lists from file-drop-list which has been stored in your clipboard.
f / fi             : Accepts <STDIN> as ""file-drop-list"" data and save into your clipboard.
                     (Unaccesible files will not be stored.)
                     This option enables you to paste <STDIN> lines as actual files on the explorer.
                     IMPORTANT: 'fi' option IGNORES DIRECTORY path.
no option          : Just stores the data emitted from <STDIN> into your clipboard.
");
		}

		static string GetStringFromSTDIN()
		{
			return Console.In.ReadToEnd();
		}

		static void IntoClipboard(string contents)
		{
			Clipboard.SetData("Text", contents);
		}
	}
}
