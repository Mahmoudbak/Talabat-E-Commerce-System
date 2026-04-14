🛒 Talabat E-Commerce System

An enterprise-level E-Commerce Backend built with ASP.NET Core following Clean Architecture principles and Domain-Driven Design (DDD) patterns.
🚀 Key Features

    Authentication & Authorization: Secure identity management using Identity Framework and JWT Tokens.

    Product Management: Advanced filtering, sorting, and pagination for product catalogs.

    Shopping Basket: Fully functional Redis-integrated basket for high performance (or In-memory for dev).

    Order Module: Complex order processing including status tracking and payment integration.

    Payment Integration: Supports online payments via Stripe API.

🏗 Architecture & Patterns

    Clean Architecture: Separation of concerns (Core, Infrastructure, Web API).

    CQRS Pattern: Separating Read and Write operations for better scalability.

    Repository & Specification Pattern: To decouple data access logic and build reusable queries.

    Dependency Injection: For loosely coupled and testable code.

    Global Error Handling: Using custom middleware for consistent API responses.

🛠 Tech Stack

    Backend: ASP.NET Core Web API 8.0

    Database: Entity Framework Core (SQL Server)

    Caching: Redis

    Security: JWT, ASP.NET Identity

    Tools: Swagger UI, AutoMapper, Fluent Validation
