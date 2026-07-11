# NXT23_AST4_SWD5_S3_HomeCare

Home Care Nursing Service System - Software Development Project

## 🏥 About the Project

A web-based platform for managing home nursing care services. Built with **ASP.NET Core MVC** and **SQL Server**.

### Features
- 👩‍⚕️ Nurse Management (Registration, Documents, Availability, Subscriptions)
- 🧑‍🤝‍🧑 Patient Management (Request Care, Track Status, Complaints)
- 📋 Care Request Workflow (Create, Assign, Offer, Accept)
- 💳 Payment Module
- ⭐ Rating & Review System
- 🔁 Recurring Requests
- 🆘 SOS Events
- 🛡️ Admin Dashboard (Full Control Panel)

---

## 🛠️ Prerequisites

- [Visual Studio 2022](https://visualstudio.microsoft.com/) (with ASP.NET workload)
- [SQL Server Express](https://www.microsoft.com/en-us/sql-server/sql-server-downloads) (or any SQL Server edition)
- [SQL Server Management Studio (SSMS)](https://learn.microsoft.com/en-us/sql/ssms/download-sql-server-management-studio-ssms)
- .NET 8.0 SDK

---

## 🗄️ Database Setup

A database backup file is included in the `Database/` folder so you can run the project with sample data.

### Steps to Restore the Database:

1. Open **SQL Server Management Studio (SSMS)**
2. Connect to your SQL Server instance
3. Right-click on **Databases** → **Restore Database...**
4. Select **Device** → Click **...** → **Add**
5. Browse to the `Database/NursingPlatformDb.bak` file from this repository
6. Click **OK** → **OK** to restore
7. Update the **connection string** in `appsettings.json` to match your SQL Server instance:

```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Data Source=YOUR_SERVER_NAME\\SQLEXPRESS;Initial Catalog=NursingPlatformDb;Integrated Security=True;Encrypt=False"
  }
}
```

> **Note:** Replace `YOUR_SERVER_NAME\\SQLEXPRESS` with your actual SQL Server instance name.

---

## 🚀 How to Run

1. Clone the repository:
   ```bash
   git clone https://github.com/ayaayman1234/NXT23_AST4_SWD5_S3_HomeCare.git
   ```
2. Open the solution file `NursingCarePlatform.Web.slnx` in Visual Studio
3. Restore the database (see Database Setup above)
4. Update the connection string in `appsettings.json`
5. Build and Run the project (press `F5`)

---

## 👥 Team

NXT23 - AST4 - SWD5 - S3

---

## 📝 License

This project is for educational purposes.
