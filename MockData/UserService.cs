using Minimal.Auth.Models.Auth;
using System.Collections.Concurrent;

namespace Minimal.Auth.MockData {
    /// <summary>
    /// 注意: 本类仅仅是演示作用. 您必须使用安全等级更高的代码来构建登录/登出行为代码.
    /// </summary>
    public class UserService {
        private readonly ConcurrentDictionary<string, UserStore> userStore;
        private readonly ConcurrentDictionary<string, UserInfo> userInfos;
        private readonly ConcurrentDictionary<string, UserStore> tokenManager;
        public UserService() {
            userStore = new ConcurrentDictionary<string, UserStore>() {
                ["Adam"] = new UserStore() {
                    Password = "123456",
                    UserId = "Adam",
                    Roles = new List<string> { "0001", "0002" },
                    IsOnline = false
                },
                ["Betty"] = new UserStore() {
                    Password = "000000",
                    UserId = "Betty",
                    Roles = new List<string> { "0000", "0001" },
                    IsOnline = false
                }
            };

            tokenManager = new ConcurrentDictionary<string, UserStore>();

            userInfos = new ConcurrentDictionary<string, UserInfo>() {
                ["Adam"] = new UserInfo() {
                    UserName = "亚当", UserId = "Adam", Title = "Employee"
                },
                ["Betty"] = new UserInfo() {
                    UserId = "Betty", UserName = "贝蒂", Title = "Root User"
                }
            };
        }

        public LoginResponse Login(LoginRequest request) {
            var response = new LoginResponse();
            if (userStore.ContainsKey(request.UserId) && userStore[request.UserId].Password == request.Passwd) {
                response.IsSuccess = true;
                response.Token = Guid.NewGuid().ToString();
                tokenManager.TryAdd(response.Token, userStore[request.UserId]);
                userStore[request.UserId].IsOnline = true;
            }
            return response;
        }

        public UserStore? ValidateToken(string token) => tokenManager.ContainsKey(token) ? userStore[tokenManager[token].UserId] : null;

        public List<string> GetUserRoles(string userId) => userStore.ContainsKey(userId) ? userStore[userId].Roles : new List<string>();

        public UserInfo? GetUserInfo(string userId) => userInfos.ContainsKey(userId) ? userInfos[userId] : null;

        public LogoutResponse Logout(LogoutRequest request) {
            var response = new LogoutResponse();
            if (userStore.ContainsKey(request.UserId) && tokenManager.ContainsKey(request.Token)) {
                response.IsSuccess = true;
                tokenManager.TryRemove(request.Token, out var _);
                userStore[request.UserId].IsOnline = false;
            }
            return response;
        }

        public CommonResponse ExecuteUserAction(UserAction action) {
            var response = new CommonResponse();
            return action.Operation switch {
                OpAction.Create or OpAction.Edit or OpAction.Delete => throw new NotImplementedException("这个逻辑很简单, 但是纸太小写不下了."),
                _ => response,
            };
        }

        public UserRoleManageData GetUserRoleManageData() {
            return new UserRoleManageData {
                Users = userInfos.Values.ToDictionary(key => key.UserId, value => value),
                Assignments = userStore.Values.ToDictionary(key => key.UserId, value => value.Roles.ToArray())
            };
        }
    }
}
