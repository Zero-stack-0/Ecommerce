Ecommerce Web Application

An Ecommerce web application that allows users to browse products, add items to their cart, and place orders. This project is built using ASP.NET Core for the backend API and Angular or React for the frontend (as applicable), with features for handling products, categories, user authentication, and order management.

🚀 Features

Product Catalog: View and search through a list of products with categories, descriptions, and pricing.
Shopping Cart: Add, update, or remove products from the cart, with stock and quantity validations.
Discounts & Pricing: Products support dynamic discounts, with final prices calculated at checkout.
User Authentication: Secure login and registration for users, including JWT authentication.
Order Management: Submit orders and view order history after placing them.
Admin Panel (optional): Manage products, view sales analytics, and handle user issues.
🛠️ Technologies

Backend:

ASP.NET Core (.NET 8) for the API
Entity Framework Core for database management
SQL Server as the database (or any supported database of your choice)
Frontend:

Vanilla Javascript for API integration for listing API and Flowbite for HTML and css components
Others:

Cookie Authentication
Repository Pattern for Data Access
💻 Installation & Setup

Prerequisites
.NET SDK (v6.0 or higher)
Node.js (for frontend)
SQL Server or any supported database
Clone the repository
bash
Copy code
git clone https://github.com/Zero-stack-0/Ecommerce.git
cd Ecommerce
Backend Setup
Navigate to the backend project folder:
bash
Copy code
cd Ecommerce/WebService
Set up your database connection string in appsettings.json.
Run database migrations:
bash
Copy code
dotnet ef database update
Cd webservice
dotnet run

Access the application:
Backend API: http://localhost:5000
🧩 Project Structure

bash
Copy code
├── WebService            # Backend (ASP.NET Core)
│   ├── Controllers       # API Controllers
    ├── Viewa             # View with js and ASP model
├── Entities              # Entity Models
├── Data                  # Data Access Layer (Repository Pattern)
├── Services              # Business Logic Layer
├── DTOs                  # Data Transfer Objects