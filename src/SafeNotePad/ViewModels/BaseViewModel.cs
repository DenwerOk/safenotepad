using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Reflection;
using System.Windows;
using GalaSoft.MvvmLight.Command;
using SafeNotePad.Facade;
using SafeNotePad.Views;

namespace SafeNotePad.ViewModels
{
    class BaseViewModel : GalaSoft.MvvmLight.ViewModelBase, IBaseViewModel, IDataErrorInfo
    {
        public event EventHandler OnCancel;

        public event EventHandler<EventArgs<bool>> OnClose;

        private ServiceContext _serviceContext;

        private ServiceContext _serviceContextWithErrorHandler;

        private string _caption;

        private bool _enableValidationHandlers;

        public bool CloseToExit { get; set; }

        public string Error
        {
            get
            {
                return null;
            }
        }

        public string this[string columnName]
        {
            get
            {
                if (_propertiesChanged.Contains(columnName))
                {
                    var info = GetType().GetProperty(columnName);
                    return GetError(info);
                }

                return null;
            }
        }

        public RelayCommand OkCommand { get; set; }

        public RelayCommand CancelCommand { get; set; }

        public ServiceContext ServiceContext
        {
            get
            {
                if (_serviceContext == null)
                {
                    _serviceContext = CreateServiceContext();
                }

                return _serviceContext;
            }
        }

        public ServiceContext ServiceContextWithErrorHandler
        {
            get
            {
                if (_serviceContextWithErrorHandler == null)
                {
                    _serviceContextWithErrorHandler = ServiceContext.Clone();
                    _serviceContextWithErrorHandler.ErrorHandler += (o, e) => HandleError(e.Value);
                }

                return _serviceContextWithErrorHandler;
            }
        }

        private readonly List<string> _propertiesChanged = new List<string>();
        private string _validationSummaryError;

        public IBaseChildWindow<BaseViewModel> ChildWindowView
        {
            get
            {
                return (IBaseChildWindow<BaseViewModel>) View;
            }
        }

        public IBaseUserControl<BaseViewModel> UserControlView
        {
            get
            {
                return (IBaseUserControl<BaseViewModel>) View;
            }
        }

        public IBaseControl View { get; set; }

        public string Caption
        {
            get
            {
                return _caption;
            }
            set
            {
                _caption = value;
                RaisePropertyChanged(() => Caption);
            }
        }

        public string ValidationSummaryError
        {
            get
            {
                return _validationSummaryError;
            }
            set
            {
                _validationSummaryError = value;
                RaisePropertyChanged(() => ValidationSummaryError);
            }
        }

        public BaseViewModel()
        {
            OkCommand = new RelayCommand(OnOkCommand, CanOk);
            CancelCommand = new RelayCommand(OnCanceled);
            PropertyChanged += OnPropertyChanged;
        }

        protected virtual ServiceContext CreateServiceContext()
        {
            return new ServiceContext();
        }

        private void OnPropertyChanged(object sender, PropertyChangedEventArgs propertyChangedEventArgs)
        {
            if (!_enableValidationHandlers)
            {
                return;
            }

            _propertiesChanged.Add(propertyChangedEventArgs.PropertyName);
        }

        protected void SetValidationSummaryError(string error)
        {
            ValidationSummaryError = error;
        }

        protected void ClearValidationSummaryErrors()
        {
            ValidationSummaryError = null;
        }

        protected void HandleError(Exception ex)
        {
            SetValidationSummaryError(ex.Message);
        }

        protected void Hide()
        {
            ChildWindowView.Hide();
        }

        protected virtual bool CanOk()
        {
            return true;
        }

        protected void Show()
        {
            ChildWindowView.Show();
        }

        protected void Show(IBaseChildWindow<IBaseViewModel> dialog)
        {
            if (ChildWindowView != null)
            {
                ChildWindowView.Show(dialog);
            }
        }

        protected bool? ShowDialog(IBaseChildWindow<IBaseViewModel> dialog)
        {
            if (ChildWindowView != null)
            {
                return ChildWindowView.ShowDialog(dialog);
            }

            return null;
        }

        protected void SetValidationSummary(IEnumerable<string> errors)
        {
            ClearValidationSummaryErrors();
            foreach (var error in errors)
            {
                SetValidationSummaryError(error);
            }
        }

        protected virtual string GetError(PropertyInfo info)
        {
            var custProperty = (ValidationAttribute)Attribute.GetCustomAttributes(info, typeof(ValidationAttribute), true).FirstOrDefault();
            if (custProperty != null && !custProperty.IsValid(info.GetValue(this)))
            {
                return custProperty.ErrorMessage;
            }

            return null;
        }

        private bool IsValid(PropertyInfo info)
        {
            if (GetError(info) != null)
            {
                info.SetValue(this, info.GetValue(this));
                return false;
            }

            return true;
        }

        public virtual bool IsValid()
        {
            var props = GetType().GetProperties();
            return props.Aggregate(true, 
                (current, propertyInfo) => current & IsValid(propertyInfo));
        }

        public void EnableValidationHandlers()
        {
            _enableValidationHandlers = true;
        }

        protected virtual void OnCanceled()
        {
            Cancel();
        }

        private void OnOkCommand()
        {
            if (IsValid())
            {
                OnOk();
            }
        }

        protected virtual void OnOk()
        {
            
        }

        protected void ShowError(string message)
        {
            if (ChildWindowView != null)
            {
                ChildWindowView.ShowErrorMessage(message);
            }
        }

        public virtual void Cleanup()
        {
            
        }

        public void Cancel()
        {
            if (OnCancel != null)
            {
                OnCancel(this, EventArgs.Empty);
            }
        }

        public void CloseOk()
        {
            Close(true);
        }

        public void Close(bool result = false)
        {
            if (OnClose != null)
            {
                OnClose(this, new EventArgs<bool>(result));
            }
        }

        public void CloseInstanly()
        {
            CloseToExit = true;
            Close();
        }
    }
}
