# Minimal.Auth
*`Minimal.Auth`* demonstrated to use `Minimal API` in `.NET 6` and `Authentication` with simple minimal requirement code. The project contains two parts of cause: `Minimal API` and `Minimal Authentication`.

I have an article written in Chinese to show minimal authentication logic. The code uses `Controller` as `Authentication Web API`. 

[Blazor极简登录模型](https://www.cnblogs.com/charset/p/14362066.html "Blazor 极简登录模型")	

When `.NET 6` launched up, I am eager to migrate controller web APIs to `Minimal API` which brings easy-to-use to us. One can write an extension class to wrap all details and throws controllers away.

## `Minimal API`

3 APIs are required. All path strings can be customized.

#### /Auth/Login 

#### /Auth/Logout

#### /Auth/CurrentUser

## `Authentication`

Authentication logic does not change because we just implement `AuthenticationStateProvider`.

**WARNING** Most of the project code is only for demo and not for product use. 