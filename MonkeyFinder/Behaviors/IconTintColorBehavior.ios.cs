#if IOS
using System.ComponentModel;
using CoreGraphics;
using Foundation;
using Microsoft.Maui.Handlers;
using Microsoft.Maui.Platform;
using UIKit;

namespace MonkeyFinder.Behaviors;

// Copied and updated from https://github.com/CommunityToolkit/Maui/blob/main/src/CommunityToolkit.Maui/Behaviors/PlatformBehaviors/IconTintColor/IconTintColorBehavior.macios.cs
public partial class IconTintColorBehavior
{
  private UIView _platformView;
  private Microsoft.Maui.Controls.View _virtualView;

  /// <inheritdoc/>
  protected override void OnAttachedTo(Microsoft.Maui.Controls.View bindable, UIView platformView)
  {
    _platformView = platformView;
    _virtualView = bindable;
    ApplyTintColor();

    PropertyChanged += OnPropertyChanged;
    bindable.PropertyChanged += OnElementPropertyChanged;
  }

  /// <inheritdoc/>
  protected override void OnDetachedFrom(Microsoft.Maui.Controls.View bindable, UIView platformView)
  {
    PropertyChanged -= OnPropertyChanged;
    bindable.PropertyChanged -= OnElementPropertyChanged;
  }

  private void OnPropertyChanged(object sender, PropertyChangedEventArgs e)
  {
    if (e.PropertyName == TintColorProperty.PropertyName)
    {
      ApplyTintColor();
    }
  }

  private void OnElementPropertyChanged(object sender, PropertyChangedEventArgs e)
  {
    if (sender is not IImageElement element)
    {
      return;
    }

    // Apply tint color if a new image stops loading
    if (e.PropertyName == Image.IsLoadingProperty.PropertyName && !element.IsLoading)
    {
      ApplyTintColor();
    }
  }

  void ApplyTintColor()
  {
    if (TintColor is null)
    {
      return;
    }

    switch (_platformView)
    {
      case UIImageView imageView:
        SetUIImageViewTintColor(imageView, TintColor);
        break;
      case UIButton button:
        SetUIButtonTintColor(button, TintColor);
        break;
      default:
        throw new NotSupportedException($"{nameof(IconTintColorBehavior)} only currently supports {nameof(UIButton)} and {nameof(UIImageView)}.");
    }
  }

  private void SetUIButtonTintColor(UIButton button, Color color)
  {
    if (button?.ImageView?.Image is null)
    {
      return;
    }

    var image = button.ImageView.Image.ImageWithRenderingMode(UIImageRenderingMode.AlwaysTemplate);
    button.SetImage(image, UIControlState.Normal);

    var platformColor = color.ToPlatform();
    button.TintColor = platformColor;
    button.ImageView.TintColor = platformColor;
  }

  static void SetUIImageViewTintColor(UIImageView imageView, Color color)
  {
    if (imageView.Image is null)
    {
      return;
    }

    imageView.Image = imageView.Image.ImageWithRenderingMode(UIImageRenderingMode.AlwaysTemplate);
    imageView.TintColor = color.ToPlatform();
  }

}
#endif