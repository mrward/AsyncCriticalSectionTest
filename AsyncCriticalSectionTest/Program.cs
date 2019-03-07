//
// Program.cs
//
// Author:
//       Matt Ward <matt.ward@microsoft.com>
//
// Copyright (c) 2019 Microsoft
//
// Permission is hereby granted, free of charge, to any person obtaining a copy
// of this software and associated documentation files (the "Software"), to deal
// in the Software without restriction, including without limitation the rights
// to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
// copies of the Software, and to permit persons to whom the Software is
// furnished to do so, subject to the following conditions:
//
// The above copyright notice and this permission notice shall be included in
// all copies or substantial portions of the Software.
//
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
// IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
// FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
// AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
// LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
// OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN
// THE SOFTWARE.

using System;
using System.Threading;
using System.Threading.Tasks;
using MonoDevelop.Projects;

namespace AsyncCriticalSectionTest
{
	class MainClass
	{
		static AsyncCriticalSection referenceCacheLock = new AsyncCriticalSection ();
		static ManualResetEvent evt = new ManualResetEvent (false);

		public static void Main (string[] args)
		{
			for (int i = 0; i < 10000; ++i) {
				Run (i);
			}
			Thread.Sleep (5000);
			evt.Set ();
			Console.ReadKey ();
		}

		static void Run (int i)
		{
			Task.Run (async () => {
				await LoadProject (i);
			});
		}

		static async Task LoadProject (int i)
		{
			using (await referenceCacheLock.EnterAsync ().ConfigureAwait (false)) {
				evt.WaitOne ();
				Console.WriteLine ($"Done {i}");
			}
		}
	}
}
