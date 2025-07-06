using System;
using System.Collections.Generic;
using System.Linq;

namespace BeerContest.Web.Services
{
    public enum ToastType
    {
        Success,
        Error,
        Warning,
        Info
    }

    public class ToastMessage
    {
        public string Id { get; set; } = Guid.NewGuid().ToString();
        public string Message { get; set; } = string.Empty;
        public ToastType Type { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.Now;
        public bool IsVisible { get; set; } = true;
    }

    public class ToastService
    {
        private readonly List<ToastMessage> _toasts = new();
        
        public event Action? OnToastChanged;

        public IReadOnlyList<ToastMessage> Toasts => _toasts.AsReadOnly();

        public void ShowSuccess(string message)
        {
            ShowToast(message, ToastType.Success);
        }

        public void ShowError(string message)
        {
            ShowToast(message, ToastType.Error);
        }

        public void ShowWarning(string message)
        {
            ShowToast(message, ToastType.Warning);
        }

        public void ShowInfo(string message)
        {
            ShowToast(message, ToastType.Info);
        }

        private void ShowToast(string message, ToastType type)
        {
            var toast = new ToastMessage
            {
                Message = message,
                Type = type
            };

            _toasts.Add(toast);
            OnToastChanged?.Invoke();

            // Auto-remove after 5 seconds
            _ = Task.Delay(5000).ContinueWith(_ => RemoveToast(toast.Id));
        }

        public void RemoveToast(string id)
        {
            var toast = _toasts.FirstOrDefault(t => t.Id == id);
            if (toast != null)
            {
                _toasts.Remove(toast);
                OnToastChanged?.Invoke();
            }
        }

        public void ClearAll()
        {
            _toasts.Clear();
            OnToastChanged?.Invoke();
        }
    }
}
