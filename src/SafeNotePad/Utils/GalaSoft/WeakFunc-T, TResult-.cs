using System;
namespace GalaSoft.MvvmLight.Helpers
{
	/// <summary>
	/// Stores an Func without causing a hard reference
	/// to be created to the Func's owner. The owner can be garbage collected at any time.
	/// </summary>
	/// <typeparam name="T">The type of the Func's parameter.</typeparam>
	/// <typeparam name="TResult">The type of the Func's return value.</typeparam>
	public class WeakFunc<T, TResult> : WeakFunc<TResult>, IExecuteWithObjectAndResult
	{
		private Func<T, TResult> _staticFunc;
		/// <summary>
		/// Gets or sets the name of the method that this WeakFunc represents.
		/// </summary>
		public override string MethodName
		{
			get
			{
				if (this._staticFunc != null)
				{
					return this._staticFunc.Method.Name;
				}
				return base.Method.Name;
			}
		}
		/// <summary>
		/// Gets a value indicating whether the Func's owner is still alive, or if it was collected
		/// by the Garbage Collector already.
		/// </summary>
		public override bool IsAlive
		{
			get
			{
				if (this._staticFunc == null && base.Reference == null)
				{
					return false;
				}
				if (this._staticFunc != null)
				{
					return base.Reference == null || base.Reference.IsAlive;
				}
				return base.Reference.IsAlive;
			}
		}
		/// <summary>
		/// Initializes a new instance of the WeakFunc class.
		/// </summary>
		/// <param name="func">The Func that will be associated to this instance.</param>
		public WeakFunc(Func<T, TResult> func) : this((func == null) ? null : func.Target, func)
		{
		}
		/// <summary>
		/// Initializes a new instance of the WeakFunc class.
		/// </summary>
		/// <param name="target">The Func's owner.</param>
		/// <param name="func">The Func that will be associated to this instance.</param>
		public WeakFunc(object target, Func<T, TResult> func)
		{
			if (func.Method.IsStatic)
			{
				this._staticFunc = func;
				if (target != null)
				{
					base.Reference = new WeakReference(target);
				}
				return;
			}
			base.Method = func.Method;
			base.FuncReference = new WeakReference(func.Target);
			base.Reference = new WeakReference(target);
		}
		/// <summary>
		/// Executes the Func. This only happens if the Func's owner
		/// is still alive. The Func's parameter is set to default(T).
		/// </summary>
		/// <returns>The result of the Func stored as reference.</returns>
		public new TResult Execute()
		{
			return this.Execute(default(T));
		}
		/// <summary>
		/// Executes the Func. This only happens if the Func's owner
		/// is still alive.
		/// </summary>
		/// <param name="parameter">A parameter to be passed to the action.</param>
		/// <returns>The result of the Func stored as reference.</returns>
		public TResult Execute(T parameter)
		{
			if (this._staticFunc != null)
			{
				return this._staticFunc(parameter);
			}
			object funcTarget = base.FuncTarget;
			if (this.IsAlive && base.Method != null && base.FuncReference != null && funcTarget != null)
			{
				return (TResult)((object)base.Method.Invoke(funcTarget, new object[]
				{
					parameter
				}));
			}
			return default(TResult);
		}
		/// <summary>
		/// Executes the Func with a parameter of type object. This parameter
		/// will be casted to T. This method implements <see cref="M:GalaSoft.MvvmLight.Helpers.IExecuteWithObject.ExecuteWithObject(System.Object)" />
		/// and can be useful if you store multiple WeakFunc{T} instances but don't know in advance
		/// what type T represents.
		/// </summary>
		/// <param name="parameter">The parameter that will be passed to the Func after
		/// being casted to T.</param>
		/// <returns>The result of the execution as object, to be casted to T.</returns>
		public object ExecuteWithObject(object parameter)
		{
			T parameterCasted = (T)((object)parameter);
			return this.Execute(parameterCasted);
		}
		/// <summary>
		/// Sets all the funcs that this WeakFunc contains to null,
		/// which is a signal for containing objects that this WeakFunc
		/// should be deleted.
		/// </summary>
		public new void MarkForDeletion()
		{
			this._staticFunc = null;
			base.MarkForDeletion();
		}
	}
}
