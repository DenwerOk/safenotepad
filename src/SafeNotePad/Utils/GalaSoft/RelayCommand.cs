using GalaSoft.MvvmLight.Helpers;
using System;
using System.Windows.Input;
namespace GalaSoft.MvvmLight.Command
{
	/// <summary>
	/// A command whose sole purpose is to relay its functionality to other
	/// objects by invoking delegates. The default return value for the CanExecute
	/// method is 'true'.  This class does not allow you to accept command parameters in the
	/// Execute and CanExecute callback methods.
	/// </summary>
	public class RelayCommand : ICommand
	{
		private readonly WeakAction _execute;
		private readonly WeakFunc<bool> _canExecute;
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
		public RelayCommand(Action execute) : this(execute, null)
		{
		}
		/// <summary>
		/// Initializes a new instance of the RelayCommand class.
		/// </summary>
		/// <param name="execute">The execution logic.</param>
		/// <param name="canExecute">The execution status logic.</param>
		/// <exception cref="T:System.ArgumentNullException">If the execute argument is null.</exception>
		public RelayCommand(Action execute, Func<bool> canExecute)
		{
			if (execute == null)
			{
				throw new ArgumentNullException("execute");
			}
			this._execute = new WeakAction(execute);
			if (canExecute != null)
			{
				this._canExecute = new WeakFunc<bool>(canExecute);
			}
		}
		/// <summary>
		/// Raises the <see cref="E:GalaSoft.MvvmLight.Command.RelayCommand.CanExecuteChanged" /> event.
		/// </summary>
		public void RaiseCanExecuteChanged()
		{
			CommandManager.InvalidateRequerySuggested();
		}
		/// <summary>
		/// Defines the method that determines whether the command can execute in its current state.
		/// </summary>
		/// <param name="parameter">This parameter will always be ignored.</param>
		/// <returns>true if this command can be executed; otherwise, false.</returns>
		public bool CanExecute(object parameter)
		{
			return this._canExecute == null || ((this._canExecute.IsStatic || this._canExecute.IsAlive) && this._canExecute.Execute());
		}
		/// <summary>
		/// Defines the method to be called when the command is invoked. 
		/// </summary>
		/// <param name="parameter">This parameter will always be ignored.</param>
		public virtual void Execute(object parameter)
		{
			if (this.CanExecute(parameter) && this._execute != null && (this._execute.IsStatic || this._execute.IsAlive))
			{
				this._execute.Execute();
			}
		}
	}
}
