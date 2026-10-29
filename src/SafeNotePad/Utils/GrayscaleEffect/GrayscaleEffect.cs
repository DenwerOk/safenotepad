using System;
using System.Reflection;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Effects;
using SafeNotePad.Utils;

namespace GrayscaleEffect
{
	public class GrayscaleEffect : ShaderEffect
	{
		private static PixelShader _pixelShader = new PixelShader
		{
            UriSource = new Uri(Helper.GetResourcePath(@"resources/grayscaleeffect.ps"))
		};
		public static readonly DependencyProperty InputProperty = ShaderEffect.RegisterPixelShaderSamplerProperty("Input", typeof(GrayscaleEffect), 0);
		public static readonly DependencyProperty DesaturationFactorProperty = DependencyProperty.Register("DesaturationFactor", typeof(double), typeof(GrayscaleEffect), new UIPropertyMetadata(0.0, ShaderEffect.PixelShaderConstantCallback(0), new CoerceValueCallback(GrayscaleEffect.CoerceDesaturationFactor)));
		public Brush Input
		{
			get
			{
				return (Brush)base.GetValue(GrayscaleEffect.InputProperty);
			}
			set
			{
				base.SetValue(GrayscaleEffect.InputProperty, value);
			}
		}
		public double DesaturationFactor
		{
			get
			{
				return (double)base.GetValue(GrayscaleEffect.DesaturationFactorProperty);
			}
			set
			{
				base.SetValue(GrayscaleEffect.DesaturationFactorProperty, value);
			}
		}
		public GrayscaleEffect()
		{
			base.PixelShader = GrayscaleEffect._pixelShader;
			base.UpdateShaderValue(GrayscaleEffect.InputProperty);
			base.UpdateShaderValue(GrayscaleEffect.DesaturationFactorProperty);
		}
		private static object CoerceDesaturationFactor(DependencyObject d, object value)
		{
			GrayscaleEffect grayscaleEffect = (GrayscaleEffect)d;
			double num = (double)value;
			if (num < 0.0 || num > 1.0)
			{
				return grayscaleEffect.DesaturationFactor;
			}
			return num;
		}
	}
}
