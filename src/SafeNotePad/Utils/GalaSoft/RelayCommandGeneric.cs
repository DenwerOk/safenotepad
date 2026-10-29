using GalaSoft.MvvmLight.Helpers;
using System;
using System.Windows.Input;
namespace GalaSoft.MvvmLight.Command
{
	/// <summary>
	/// A generic command whose sole purpose is to relay its functionality to other
	/// objects by invoking delegates. The default return value for the CanExecute
	/// method is 'true'. This class allows you to accept command parameters in the
	/// Execute and CanExecute callback methods.
	/// </summary>
	/// <typeparam name="T">The type of the command parameter.</typeparam>
	public class RelayCommand<T> : ICommand
	{
		private readonly WeakAction<T> _execute;
		private readonly WeakFunc<T, bool> _canExecute;
		/// <summary>
		/// Occurs when changes occur that affect whether the command should execute.
		/// </summary>
		public event EventHandler CanExecuteChanged
		{
			add
			{
				if (this._canExecute != null)
				{
					CommandManager.RequerySuggested += value;
				}
			}
			remove
			{
				if (this._canExecute != null)
				{
					CommandManager.RequerySuggested -= value;
				}
			}
		}
		/// <summary>
		/// Initializes a new instance of the RelayCommand class that 
		/// can always execute.
		/// </summary>
		/// <param name="execute">The execution logic.</param>
		/// <exception cref="T:System.ArgumentNullException">If the execute argument is null.</exception>
		public RelayCommand(Action<T> execute) : this(execute, null)
		{
		}
		/// <summary>
		/// Initializes a new instance of the RelayCommand class.
		/// </summary>
		/// <param name="execute">The execution logic.</param>
		/// <param name="canExecute">The execution status logic.</param>
		/// <exception cref="T:System.ArgumentNullException">If the execute argument is null.</exception>
		public RelayCommand(Action<T> execute, Func<T, bool> canExecute)
		{
			if (execute == null)
			{
				throw new ArgumentNullException("execute");
			}
			this._execute = new WeakAction<T>(execute);
			if (canExecute != null)
			{
				this._canExecute = new WeakFunc<T, bool>(canExecute);
			}
		}
		/// <summary>
		/// Raises the <see cref="E:GalaSoft.MvvmLight.Command.RelayCommand`1.CanExecuteChanged" /> event.
		/// </summary>
		public void RaiseCanExecuteChanged()
		{
			CommandManager.InvalidateRequerySuggested();
		}
		/// <summary>
		/// Defines the method that determines whether the command can execute in its current state.
		/// </summary>
		/// <param name="parameter">Data used by the command. If the command does not require data 
		/// to be passed, this object can be set to a null reference</param>
		/// <returns>true if this command can be executed; otherwise, false.</returns>
		public bool CanExecute(object parameter)
		{
			if (this._canExecute == null)
			{
				return true;
			}
			if (!this._canExecute.IsStatic && !this._canExecute.IsAlive)
			{
				return false;
			}
			if (parameter == null && typeof(T).IsValueType)
			{
				return this._canExecute.Execute(default(T));
			}
			return this._canExecute.Execute((T)((object)parameter));
		}
		/// <summary>
		/// Defines the method to be called when the command is invoked. 
		/// </summary>
		/// <param name="parameter">Data used by the command. If the command does not require data 
		/// to be passed, this object can be set to a null reference</param>
		public virtual void Execute(object parameter)
		{
			object val = parameter;
			if (parameter != null && parameter.GetType() != typeof(T) && parameter is IConvertible)
			{
				val = Convert.ChangeType(parameter, typeof(T), null);
			}
			if (this.CanExecute(val) && this._execute != null && (this._execute.IsStatic || this._execute.IsAlive))
			{
				if (val == null)
				{
					if (typeof(T).IsValueType)
					{
						this._execute.Execute(default(T));
						return;
					}
					this._execute.Execute((T)((object)val));
					return;
				}
				else
				{
					this._execute.Execute((T)((object)val));
				}
			}
		}
	}
}
