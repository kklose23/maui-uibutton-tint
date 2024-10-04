using Foundation;
using UIKit;

namespace MonkeyFinder;

[Register("AppDelegate")]
public class AppDelegate : MauiUIApplicationDelegate
{
	protected override MauiApp CreateMauiApp() 
	{
    Microsoft.Maui.Controls.Handlers.Items.CollectionViewHandler.Mapper.AppendToMapping("ScrollsToTop", (h, v) =>
    {
      var uiView = h.PlatformView.Subviews.FirstOrDefault(x => x is UICollectionView);
      if (uiView is UICollectionView collectionView)
      {
        collectionView.ScrollsToTop = true;
      }
    });

    return MauiProgram.CreateMauiApp();
  }
}
