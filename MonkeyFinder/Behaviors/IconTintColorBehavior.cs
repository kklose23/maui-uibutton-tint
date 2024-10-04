using System.Runtime.Versioning;

namespace MonkeyFinder.Behaviors;

[UnsupportedOSPlatform("windows")]
public partial class IconTintColorBehavior : PlatformBehavior<Microsoft.Maui.Controls.View>
{
  /// <summary>
  /// Attached Bindable Property for the <see cref="TintColor"/>.
  /// </summary>
  public static readonly BindableProperty TintColorProperty =
    BindableProperty.Create(nameof(TintColor), typeof(Color), typeof(IconTintColorBehavior), default);

  /// <summary>
  /// Property that represents the <see cref="Color"/> that Icon will be tinted.
  /// </summary>
  public Color TintColor
  {
    get => (Color)GetValue(TintColorProperty);
    set => SetValue(TintColorProperty, value);
  }

  public static readonly BindableProperty AttachBehaviorProperty =
    BindableProperty.CreateAttached("AttachBehavior", typeof(Color), typeof(IconTintColorBehavior), defaultValue: null, propertyChanged: OnAttachBehaviorChanged);

  public static Color GetAttachBehavior(BindableObject view)
  {
    return (Color)view.GetValue(AttachBehaviorProperty);
  }

  public static void SetAttachBehavior(BindableObject view, Color value)
  {
    view.SetValue(AttachBehaviorProperty, value);
  }

  static void OnAttachBehaviorChanged(BindableObject bindableObject, object oldValue, object newValue)
  {
    if (bindableObject is not Microsoft.Maui.Controls.View view)
    {
      return;
    }

    var tintColor = (Color)newValue;
    var behavior = view.Behaviors.FirstOrDefault(x => x is IconTintColorBehavior) as IconTintColorBehavior;
    if (behavior is null)
    {
      view.Behaviors.Add(new IconTintColorBehavior
      {
        TintColor = tintColor
      });
    }
    else
    {
      behavior.TintColor = tintColor;
    }
  }
}