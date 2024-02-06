using SchoolBusWPF.Data;
using System.Collections;
using System.ComponentModel;
using System.Windows.Input;

namespace SchoolBusWPF.ViewModels
{
    public abstract class ViewModelBase : INotifyPropertyChanged, INotifyDataErrorInfo
	{
		public readonly SchoolBusDBContext _dbContext = new();

		private bool _isPopupOpen;
		public bool IsPopupOpen
		{
			get { return _isPopupOpen; }
			set
			{
				_isPopupOpen = value;
				OnPropertyChanged(nameof(IsPopupOpen));
			}
		}

        private bool _isUpdate;
        public bool IsUpdate
        {
            get { return _isUpdate; }
            set
            {
                _isUpdate = value;
                OnPropertyChanged(nameof(IsUpdate));
            }
        }

        public event PropertyChangedEventHandler? PropertyChanged;

		protected void OnPropertyChanged(string propertyName)
		{
			PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
		}

        //

        //public readonly Dictionary<string, List<string>> Errors = [];
        //public bool HasErrors => Errors.Count > 0;

        //public event EventHandler<DataErrorsChangedEventArgs>? ErrorsChanged;

        //public IEnumerable GetErrors(string? propertyName)
        //{
        //	if(propertyName is null)
        //		throw new Exception();

        //	if (Errors.TryGetValue(propertyName, out List<string>? errors))
        //		return errors;
        //	else
        //		return Enumerable.Empty<string>();
        //}

        //protected void ClearErrors(string propertyName)
        //{
        //	if (!Errors.ContainsKey(propertyName))
        //		return;

        //	Errors.Remove(propertyName);
        //	ErrorsChanged?.Invoke(this, new DataErrorsChangedEventArgs(propertyName));
        //}

        //public void ClearAllErrors()
        //{
        //	foreach (var propertyName in Errors.Keys.ToList())
        //	{
        //		ClearErrors(propertyName);
        //	}
        //}

        //public void Validate(string propertyName, object? propertyValue)
        //{
        //	if (!IsPopupOpen)
        //	{
        //		ClearAllErrors();
        //		return;
        //	}

        //	var results = new List<ValidationResult>();
        //	Validator.TryValidateProperty(propertyValue, new ValidationContext(this) { MemberName = propertyName }, results);

        //	if (results.Count != 0)
        //	{
        //		if (!Errors.ContainsKey(propertyName))
        //			Errors[propertyName] = [];

        //		foreach (var error in results.Select(r => r.ErrorMessage).ToList())
        //		{
        //			if (error is null)
        //				continue;

        //			if (!Errors[propertyName].Contains(error))
        //			{
        //				Errors[propertyName].Add(error);
        //				ErrorsChanged?.Invoke(this, new DataErrorsChangedEventArgs(propertyName));
        //			}
        //		}
        //	}
        //	else
        //		ClearErrors(propertyName);
        //}

        //

        private readonly Dictionary<string, List<string>> _errors = [];

        public bool HasErrors => _errors.Count != 0;

        public event EventHandler<DataErrorsChangedEventArgs>? ErrorsChanged;

        public IEnumerable GetErrors(string? propertyName)
        {
            if (string.IsNullOrEmpty(propertyName) || !_errors.TryGetValue(propertyName, out List<string>? value))
                return Enumerable.Empty<string>();

            return value;
        }

        protected virtual void OnErrorsChanged(string propertyName)
        {
            ErrorsChanged?.Invoke(this, new DataErrorsChangedEventArgs(propertyName));
        }

        protected void AddError(string propertyName, string error)
        {
            if (!_errors.ContainsKey(propertyName))
                _errors[propertyName] = [];

            if (!_errors[propertyName].Contains(error))
            {
                _errors[propertyName].Add(error);
                OnErrorsChanged(propertyName);
            }
        }

        protected void ClearErrors(string propertyName)
        {
            if (!_errors.ContainsKey(propertyName))
                return;

            _errors.Remove(propertyName);
            OnErrorsChanged(propertyName);
        }

        protected void ClearAllErrors()
        {
            foreach (var propertyName in _errors.Keys.ToList())
            {
                ClearErrors(propertyName);
            }
        }

        public void ValidateProperty(string propertyName, object? propertyValue, string validationMessage)
        {
            ClearErrors(propertyName);

            if (string.IsNullOrEmpty(propertyValue?.ToString()))
                AddError(propertyName, validationMessage);
        }

        //

        public ICommand OpenPopupCommand { get; set; }
        public ICommand SaveChangesCommand { get; set; }
        public ICommand ClosePopupCommand { get; set; }
        public ICommand UpdateEntityCommand { get; set; }
        public ICommand DeleteEntityCommand { get; set; }

		public abstract bool CanSaveChanges(object obj);

        public abstract void SaveChanges(object obj);

		public abstract void OpenPopup(object obj);

		public abstract void ClosePopup(object obj);

		public abstract void UpdateEntity(object obj);

		public abstract void DeleteEntity(object obj);
    }
}
