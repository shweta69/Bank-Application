
# 🏦 Bank Management System (.NET Core API)

A simple and secure Bank Management System built with **ASP.NET Core Web API**, **Entity Framework Core**, and **JWT Authentication**. It allows both **Users** and **Admins** to manage bank accounts with role-based access.

---

## ⚙️ Technologies Used

- **ASP.NET Core Web API**
- **Entity Framework Core (Code First)**
- **JWT Authentication**
- **SQL Server**
- **C# 10**
- **.NET 6.0**

---

## 👤 Roles

- **User**
  - Register/Login
  - Create Account
  - Deposit, Withdraw, Check Balance
  - Edit Profile / Change Password

- **Admin**
  - Create Account (for any user)
  - Delete User / Account
  - View All Users (with account count)
  - View Error Logs
  - Edit Self / Change Password

---

## 🧪 Testing the API (Postman/Swagger)

1. **Register User**  
   `POST /api/UserRegistration/register`  
   *(Auto-generates 6-digit Customer ID)*

2. **Login User/Admin**  
   `POST /api/UserRegistration/login?customerId={id}&password={pwd}`  
   → Returns JWT Token

3. **Authorize Requests**  
   Add JWT in Postman under `Authorization > Bearer Token`

4. **Test User Actions**  
   `GET /api/user/check-balance/{accountId}`  
   `POST /api/user/deposit`  
   ...

5. **Test Admin Actions**  
   `GET /api/admin/all-users`  
   `DELETE /api/admin/delete-user/{customerId}`  
   ...

---

## 🔐 Authentication

- JWT Token generated on login
- Role-based protection using `[Authorize(Roles = "Admin")]` or `"User"`
- Claims used: `Name`, `Role`, `CustomerId`

---

## ✅ Features

- Passwords are securely hashed using `PasswordHasher<T>`
- `CustomerId` and `AccountId` are auto-generated (6-digit)
- Centralized **Exception Handling Middleware** for consistent API error responses
- Error logs (optional) stored in `logs/errorlog.txt`

---

## 👩‍💻 Who Can Use This?

- Beginners learning .NET Web API
- Developers building secure, role-based systems

---

## 📬 Contact

Feel free to fork, test, and contribute! 😊
