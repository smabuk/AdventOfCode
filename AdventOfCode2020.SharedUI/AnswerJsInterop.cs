using System;
using System.Threading.Tasks;

using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;

namespace AdventOfCode2020.SharedUI {
	// This class provides an example of how JavaScript functionality can be wrapped
	// in a .NET class for easy consumption. The associated JavaScript module is
	// loaded on demand when first needed.
	//
	// This class can be registered as scoped DI service and then injected into Blazor
	// components for use.

	public class AnswerJsInterop : IAsyncDisposable {
		private readonly Lazy<Task<IJSObjectReference>> moduleTask;

		public AnswerJsInterop(IJSRuntime jsRuntime) {
			moduleTask = new(() => jsRuntime.InvokeAsync<IJSObjectReference>(
			   "import", "./_content/AdventOfCode2020.SharedUI/answerJsInterop.js").AsTask());
		}

		public async ValueTask<string> CopyToClipboard(ElementReference reference) {
			var module = await moduleTask.Value;
			return await module.InvokeAsync<string>("copyText", reference);
			// return await module.InvokeAsync<string>("showPrompt", message);
		}

		public async ValueTask DisposeAsync() {
			if (moduleTask.IsValueCreated) {
				var module = await moduleTask.Value;
				await module.DisposeAsync();
			}
		}
	}
}
