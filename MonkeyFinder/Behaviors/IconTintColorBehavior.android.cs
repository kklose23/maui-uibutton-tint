#if ANDROID
using Android.Graphics;
using Android.Widget;
using Google.Android.Material.Button;
using Microsoft.Maui.Platform;
using System.ComponentModel;
using AButton = Android.Widget.Button;
using AView = Android.Views.View;
using Color = Microsoft.Maui.Graphics.Color;
using ImageButton = Microsoft.Maui.Controls.ImageButton;

namespace MonkeyFinder.Behaviors;

// Copied and updated from https://github.com/karlandin/maui-toolkit-icon-tint-color-bug/blob/main/Behaviors/IconTintColorBehavior.android.cs
public partial class IconTintColorBehavior
{
  private AView _platformView;

  /// <inheritdoc/>
  protected override void OnAttachedTo(Microsoft.Maui.Controls.View bindable, AView platformView)
  {
    _platformView = platformView;
    ApplyTintColor();

    PropertyChanged += OnPropertyChanged;
    bindable.PropertyChanged += OnElementPropertyChanged;
  }

  /// <inheritdoc/>
  protected override void OnDetachedFrom(Microsoft.Maui.Controls.View bindable, AView platformView)
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
    if (e.PropertyName == Image.SourceProperty.PropertyName ||
        e.PropertyName == ImageButton.SourceProperty.PropertyName || 
        e.PropertyName == Microsoft.Maui.Controls.Button.ImageSourceProperty.PropertyName)
    {
      ApplyTintColor();
    }
  }

  private void ApplyTintColor()
  {
    if (TintColor is null)
    {
      return;
    }

    switch (_platformView)
    {
      case ImageView image:
        SetImageViewTintColor(image, TintColor);
        break;
      case MaterialButton button:
        SetMaterialButtonTintColor(button, TintColor);
        break;
      case AButton button:
        SetButtonTintColor(button, TintColor);
        break;
      default:
        throw new NotSupportedException($"{nameof(IconTintColorBehavior)} only currently supports Android.Widget.Button and {nameof(ImageView)}.");
    }

    static void SetImageViewTintColor(ImageView image, Color color)
    {
      image.SetColorFilter(new PorterDuffColorFilter(color.ToPlatform(), PorterDuff.Mode.SrcIn ?? throw new InvalidOperationException("PorterDuff.Mode.SrcIn should not be null at runtime.")));
    }

    static void SetMaterialButtonTintColor(MaterialButton button, Color color)
    {
      if (button.Icon is null)
      {
        return;
      }

      button.Icon.SetColorFilter(new PorterDuffColorFilter(color.ToPlatform(),
          PorterDuff.Mode.SrcIn ??
          throw new InvalidOperationException("PorterDuff.Mode.SrcIn should not be null at runtime.")));
    }

    static void SetButtonTintColor(AButton button, Color color)
    {
      var drawables = button.GetCompoundDrawables().Where(d => d is not null);

      foreach (var img in drawables)
      {
        img.SetTint(color.ToPlatform());
      }
    }
  }
}
#endif