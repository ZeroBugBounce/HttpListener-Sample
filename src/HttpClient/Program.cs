using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading;

namespace HttpClient
{
	class Program
	{
		static void Main(string[] args)
		{
			while (true)
			{
				Console.WriteLine("Type int values");
				var client = new WebClient();
				var values = new NameValueCollection();
				values.Add("newValue", Console.ReadLine());

				var responseBytes = client.UploadValues("http://localhost:999/", "POST", values);
				var responseText = Encoding.Unicode.GetString(responseBytes);

				Console.WriteLine(responseText);
			}
		}
	}
}
