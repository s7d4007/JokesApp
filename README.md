# 🎭 ProgrammerJokes

A dynamic, full-stack web application built with **ASP.NET Core MVC** where developers can share, discover, and rate programming humor. This project focuses on secure data handling, relational database management, and modern UI/UX principles.

---

## 🚀 Key Features

* **Secure User Authentication:** Implemented via ASP.NET Core Identity with customized Scaffolding for a seamless login/registration experience.
* **Persistent Voting System:** A "One Vote Per User" logic utilizing a relational `UserJokeVotes` table to maintain data integrity.
* **Dynamic Search:** Server-side filtering allowing users to find jokes by keywords.
* **Content Ownership:** Role-based logic ensuring only the joke creator can Edit or Delete their specific entries.
* **Creative UI:** Responsive design featuring a custom hero section, interactive hover effects, and a stylized "Joke of the Day" randomizer.
* **Custom Theming:** Integrated Bootswatch themes and bespoke CSS for a professional, cohesive look.

## 🛠️ Tech Stack

* **Framework:** .NET 8.0 (ASP.NET Core MVC)
* **Database:** Entity Framework Core (SQL Server / SQLite)
* **Frontend:** Bootstrap 5, Bootstrap Icons, Custom CSS
* **Authentication:** ASP.NET Core Identity
* **Version Control:** Git & GitHub

## 📂 Project Structure Highlights

- **Models/**: Defines the `Joke` and `UserJokeVote` entities with Data Annotations for validation.
- **Controllers/**: Contains the `JokesController` (CRUD + Search logic) and `HomeController` (Randomized content logic).
- **Views/**: Stylized Razor pages using partial views and layouts for DRY (Don't Repeat Yourself) code.
- **Data/**: Managed migrations and Database Context.

## ⚙️ Setup & Installation

1.  **Clone the repo:**
    ```bash
    git clone [https://github.com/s7d4007/JokesApp.git]
    ```
2.  **Navigate to project folder:**
    ```bash
    cd JokesApp
    ```
3.  **Update Database:**
    Open the Package Manager Console in Visual Studio and run:
    ```bash
    Update-Database
    ```
4.  **Run the application:**
    Press `F5` in Visual Studio.

## 📈 Learning Outcomes

This project was a deep dive into the MVC pattern. I successfully tackled:
- Managing **Many-to-Many** logic via cross-reference tables.
- Resolving **Nullable Reference Type** warnings to ensure code safety.
- Customizing **Identity Areas** to remove unused external providers.
- Building a **consistent UX** through global CSS variables and responsive grids.

---
📫 **Connect with me:** https://www.linkedin.com/in/souvikdutta7/