# 🕌 Avondale Islamic Centre Management System

A web-based management system for the **Avondale Islamic Centre**, built with **ASP.NET Core 8 MVC** and **Entity Framework Core**.  
This project is designed to support the Centre’s daily operations — including managing bookings, classes, donations, notices, reports, students, and teachers — in a secure and user-friendly way.

---

## 📌 Features

- **Authentication & Authorization**
  - ASP.NET Core Identity integration
  - Role-based access (Admin, Teacher, Member)

- **Bookings Management**
  - Reserve halls with start and end times
  - Link bookings to registered users

- **Classes & Students**
  - Create and manage classes
  - Assign teachers and track enrolled students

- **Teachers**
  - Manage teacher profiles and contact details

- **Donations**
  - Record donor details, donation amounts, and payment methods
  - Track donation types for reporting

- **Notices**
  - Post announcements to the community
  - Track created/updated times and users

- **Reports**
  - Submit and track internal reports
  - Link reports to users

- **Halls**
  - Manage hall information (name, capacity, availability)

- **Sorting, Filtering & Pagination**
  - Search records (e.g., by name, title, or description)
  - Sort by common fields (e.g., date, name, amount)
  - Paginate long lists for better performance and usability

---

## 🛠️ Tech Stack

- **Backend:** ASP.NET Core 8 MVC  
- **Frontend:** Razor Views, Bootstrap  
- **Database:** Microsoft SQL Server (with Entity Framework Core ORM)  
- **Authentication:** ASP.NET Core Identity  
- **Tools:** Visual Studio 2022 / Visual Studio Code  

---

## 🚀 Getting Started

### 1. Clone the repository

```bash
git clone https://github.com/ac121627/AvondaleIslamicCentre.git
cd AvondaleIslamicCentre
