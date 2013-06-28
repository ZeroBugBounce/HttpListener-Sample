using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading;
using System.Web;

namespace StandAloneHttpListener
{
	class Program
	{
		static int valueToSet = 0;

		static void Main(string[] args)
		{
			var thread = new Thread(_ => RunHttpListener());
			thread.Start();

			Console.WriteLine("Server starting...");
			Console.ReadLine();
		}

		static void RunHttpListener()
		{
			HttpListener listener = new HttpListener();
			listener.Prefixes.Add("http://*:999/");
			listener.Start();

			Console.WriteLine("Server started");

			while (true)
			{
				var context = listener.GetContext();
				var requestUrl = context.Request.Url;

				var rawValues = context.Request.ContentEncoding.GetString(
					context.Request.InputStream.ReadToEnd());

				var values = HttpUtility.ParseQueryString(rawValues);

				valueToSet = int.Parse(values.GetValues(0)[0]);

				Console.WriteLine("valueToSet is now {0}", valueToSet);

				context.Response.ContentEncoding = Encoding.Unicode;

				byte[] output = Encoding.Unicode.GetBytes(string.Format("valueToSet is now {0}", valueToSet));
				context.Response.OutputStream.Write(output, 0, output.Length);

				context.Response.Close();
			}
		}
	}

	static class Extensions
	{
		public static byte[] ReadToEnd(this Stream stream)
		{
			MemoryStream outputStream = new MemoryStream();
			int readCount = -1;
			byte[] buffer = new byte[4096];
			while ((readCount = stream.Read(buffer, 0, 4096)) > 0)
			{
				outputStream.Write(buffer, 0, readCount);
			}

			return outputStream.ToArray();
		}
	}

}
