using System;
using UnityEngine;
using Object = UnityEngine.Object;
using com.ktgame.assets.loader.core;
using Cysharp.Threading.Tasks;

namespace com.ktgame.assets.loader.resources
{
	public sealed class ResourcesAssetLoader : IAssetLoader
	{
		private int _nextRequestId;

		public AssetRequest<TAsset> Load<TAsset>(string address) where TAsset : Object
		{
			var requestId = _nextRequestId++;

			var request = new AssetRequest<TAsset>(requestId);
			var setter = (IAssetRequest<TAsset>)request;
			var result = Resources.Load<TAsset>(address);

			setter.SetResult(result);

			var status = result != null ? AssetRequestStatus.Succeeded : AssetRequestStatus.Failed;
			setter.SetStatus(status);

			if (result == null)
			{
				var exception = new InvalidOperationException($"Requested asset（Key: {address}）was not found.");
				setter.SetOperationException(exception);
			}

			setter.SetProgressFunc(() => 1.0f);
			setter.SetTask(UniTask.FromResult(result));
			return request;
		}

		public AssetRequest<Object> Load(string address)
		{
			return Load<Object>(address);
		}

		public AssetRequest<TAsset> LoadAsync<TAsset>(string address) where TAsset : Object
		{
			var requestId = _nextRequestId++;

			var request = new AssetRequest<TAsset>(requestId);
			var setter = (IAssetRequest<TAsset>)request;
			var utcs = new UniTaskCompletionSource<TAsset>();

			var req = Resources.LoadAsync<TAsset>(address);

			req.completed += _ =>
			{
				var result = req.asset as TAsset;
				setter.SetResult(result);

				var status = result != null ? AssetRequestStatus.Succeeded : AssetRequestStatus.Failed;
				setter.SetStatus(status);

				if (result == null)
				{
					var exception = new InvalidOperationException($"Requested asset（Key: {address}）was not found.");
					setter.SetOperationException(exception);
				}

				utcs.TrySetResult(result);
			};

			setter.SetProgressFunc(() => req.progress);
			setter.SetTask(utcs.Task);
			return request;
		}

		public AssetRequest<Object> LoadAsync(string address)
		{
			return LoadAsync<Object>(address);
		}

		public void Release(AssetRequest request) { }
	}
}
