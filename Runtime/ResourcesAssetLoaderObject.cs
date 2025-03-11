using com.ktgame.assets.loader.core;
using UnityEngine;

namespace com.ktgame.assets.loader.resources
{
	[CreateAssetMenu(fileName = "ResourcesAssetLoader", menuName = "Ktgame/Asset Loader/Resources Asset Loader")]
	public class ResourcesAssetLoaderObject : AssetLoaderObject, IAssetLoader
	{
		private readonly ResourcesAssetLoader _loader = new ResourcesAssetLoader();

		public override AssetRequest<TAsset> Load<TAsset>(string address)
		{
			return _loader.Load<TAsset>(address);
		}

		public override AssetRequest<Object> Load(string address)
		{
			return _loader.Load(address);
		}

		public override AssetRequest<TAsset> LoadAsync<TAsset>(string address)
		{
			return _loader.Load<TAsset>(address);
		}

		public override AssetRequest<Object> LoadAsync(string address)
		{
			return _loader.Load(address);
		}

		public override void Release(AssetRequest request)
		{
			_loader.Release(request);
		}
	}
}
