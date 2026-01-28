using System;
using System.Threading.Tasks;

    public class UserSession
    {
        public int? CurrentUserId { get; private set; }
        public bool IsAuthenticated => CurrentUserId.HasValue;

        public event Action OnChange;

        public void SetUser(int userId)
        {
            CurrentUserId = userId;
            NotifyStateChanged();
        }

        public void ClearUser()
        {
            CurrentUserId = null;
            NotifyStateChanged();
        }

        private void NotifyStateChanged() => OnChange?.Invoke();
    }
