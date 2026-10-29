using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq.Expressions;
using System.Windows;
namespace GalaSoft.MvvmLight
{
	/// <summary>
	/// A base class for the ViewModel classes in the MVVM pattern.
	/// </summary>
	public abstract class ViewModelBase : ObservableObject
	{
		private static bool? _isInDesignMode;
		/// <summary>
		/// Gets a value indicating whether the control is in design mode
		/// (running under Blend or Visual Studio).
		/// </summary>
		public bool IsInDesignMode
		{
			get
			{
				return ViewModelBase.IsInDesignModeStatic;
			}
		}
		/// <summary>
		/// Gets a value indicating whether the control is in design mode
		/// (running in Blend or Visual Studio).
		/// </summary>
		public static bool IsInDesignModeStatic
		{
			get
			{
				if (!ViewModelBase._isInDesignMode.HasValue)
				{
					DependencyProperty prop = DesignerProperties.IsInDesignModeProperty;
					ViewModelBase._isInDesignMode = new bool?((bool)DependencyPropertyDescriptor.FromProperty(prop, typeof(FrameworkElement)).Metadata.DefaultValue);
				}
				return ViewModelBase._isInDesignMode.Value;
			}
		}
		/// <summary>
		/// Raises the PropertyChanged event if needed, and broadcasts a
		/// PropertyChangedMessage using the Messenger instance (or the
		/// static default instance if no Messenger instance is available).
		/// </summary>
		/// <typeparam name="T">The type of the property that
		/// changed.</typeparam>
		/// <param name="propertyName">The name of the property that
		/// changed.</param>
		/// <param name="oldValue">The property's value before the change
		/// occurred.</param>
		/// <param name="newValue">The property's value after the change
		/// occurred.</param>
		/// <param name="broadcast">If true, a PropertyChangedMessage will
		/// be broadcasted. If false, only the event will be raised.</param>
		/// <remarks>If the propertyName parameter
		/// does not correspond to an existing property on the current class, an
		/// exception is thrown in DEBUG configuration only.</remarks>
		protected virtual void RaisePropertyChanged<T>(string propertyName, T oldValue = null, T newValue = null) where T: class
		{
			if (string.IsNullOrEmpty(propertyName))
			{
				throw new ArgumentException("This method cannot be called with an empty string", "propertyName");
			}
			this.RaisePropertyChanged(propertyName);
		}
		/// <summary>
		/// Raises the PropertyChanged event if needed, and broadcasts a
		/// PropertyChangedMessage using the Messenger instance (or the
		/// static default instance if no Messenger instance is available).
		/// </summary>
		/// <typeparam name="T">The type of the property that
		/// changed.</typeparam>
		/// <param name="propertyExpression">An expression identifying the property
		/// that changed.</param>
		/// <param name="oldValue">The property's value before the change
		/// occurred.</param>
		/// <param name="newValue">The property's value after the change
		/// occurred.</param>
		/// <param name="broadcast">If true, a PropertyChangedMessage will
		/// be broadcasted. If false, only the event will be raised.</param>
		protected virtual void RaisePropertyChanged<T>(Expression<Func<T>> propertyExpression, T oldValue, T newValue)
		{
			PropertyChangedEventHandler handler = base.PropertyChangedHandler;
			if (handler != null)
			{
				string propertyName = ObservableObject.GetPropertyName<T>(propertyExpression);
				if (handler != null)
				{
					handler(this, new PropertyChangedEventArgs(propertyName));
				}
			}
		}
		/// <summary>
		/// Assigns a new value to the property. Then, raises the
		/// PropertyChanged event if needed, and broadcasts a
		/// PropertyChangedMessage using the Messenger instance (or the
		/// static default instance if no Messenger instance is available). 
		/// </summary>
		/// <typeparam name="T">The type of the property that
		/// changed.</typeparam>
		/// <param name="propertyExpression">An expression identifying the property
		/// that changed.</param>
		/// <param name="field">The field storing the property's value.</param>
		/// <param name="newValue">The property's value after the change
		/// occurred.</param>
		/// <param name="broadcast">If true, a PropertyChangedMessage will
		/// be broadcasted. If false, only the event will be raised.</param>
		/// <returns>True if the PropertyChanged event was raised, false otherwise.</returns>
		protected bool Set<T>(Expression<Func<T>> propertyExpression, ref T field, T newValue)
		{
			if (EqualityComparer<T>.Default.Equals(field, newValue))
			{
				return false;
			}
			this.RaisePropertyChanging<T>(propertyExpression);
			T oldValue = field;
			field = newValue;
			this.RaisePropertyChanged<T>(propertyExpression, oldValue, field);
			return true;
		}
		/// <summary>
		/// Assigns a new value to the property. Then, raises the
		/// PropertyChanged event if needed, and broadcasts a
		/// PropertyChangedMessage using the Messenger instance (or the
		/// static default instance if no Messenger instance is available). 
		/// </summary>
		/// <typeparam name="T">The type of the property that
		/// changed.</typeparam>
		/// <param name="propertyName">The name of the property that
		/// changed.</param>
		/// <param name="field">The field storing the property's value.</param>
		/// <param name="newValue">The property's value after the change
		/// occurred.</param>
		/// <param name="broadcast">If true, a PropertyChangedMessage will
		/// be broadcasted. If false, only the event will be raised.</param>
		/// <returns>True if the PropertyChanged event was raised, false otherwise.</returns>
		protected bool Set<T>(string propertyName, ref T field, T newValue = null) where T: class
		{
			if (EqualityComparer<T>.Default.Equals(field, newValue))
			{
				return false;
			}
			this.RaisePropertyChanging(propertyName);
			T oldValue = field;
			field = newValue;
			this.RaisePropertyChanged<T>(propertyName, oldValue, field);
			return true;
		}
	}
}
