namespace Minimal.Auth.Models.Auth {
    public class LoginRequest {
        public string UserId { get; set; }
        public string Passwd { get; set; }
    }

    public class LoginResponse {
        public bool IsSuccess { get; set; }
        public string Token { get; set; }
    }

    public class LogoutRequest {
        public string UserId { get; set; }
        public string Token { get; set; }
    }

    public class LogoutResponse {
        public bool IsSuccess { get; set; }
    }

    public class CommonResponse {
        public bool IsSuccess { get; set; }
        public string Message { get; set; }
    }

    public class CurrentUser {
        public bool IsAuthenticated { get; set; }
        public Dictionary<string, string> Claims { get; set; }
        public List<string> Roles { get; set; }
        public bool IsExpired { get; set; }

        public void Reset() {
            IsAuthenticated = false;
            Claims.Clear();
            Roles.Clear();
        }
    }

    public class UserStore {
        public string UserId { get; set; }
        public string Password { get; set; }
        public List<string> Roles { get; set; }
        public bool IsOnline { get; set; }
        public string Token { get; set; }
    }
    public enum State {
        Initial = 0,
        Active = 1,
        Deleted = 2
    }

    public class UserInfo {
        public string UserId { get; set; }
        public string UserName { get; set; }
        public string Title { get; set; }
        public State State { get; set; }
    }

    public class Role {
        public string Id { get; set; }
        public string Name { get; set; }
    }

    public class UserRole {
        public string StaffId { get; set; }
        public string RoleId { get; set; }
    }
    public enum OpAction {
        Create = 0, Edit = 1, Delete = 2
    }

    public class UserAction {
        public string UserId { get; set; }
        public string Name { get; set; }
        public OpAction Operation { get; set; }
        public string[] Roles { get; set; }
    }

    public class UserRoleManageData {
        public Dictionary<string, UserInfo> Users { get; set; }
        public Dictionary<string, Role> Roles { get; set; }
        public Dictionary<string, string[]> Assignments { get; set; }
    }

}
