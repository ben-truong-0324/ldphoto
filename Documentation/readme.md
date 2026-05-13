# LDPhotography Web App

A Razor-based ASP.NET Core MVC application serving as the portfolio and storefront for LD Landscape Photography. 

## Architecture
- **Framework:** ASP.NET Core MVC (C#)
- **Frontend Framework:** Bootstrap 5, Custom CSS (`site.css`)
- **Testing:** xUnit

## Structure Breakdown
1. **`_Layout.cshtml`**: Contains the global Navbar, Footer, Shopping Cart UI, and external asset links (Google Fonts, FontAwesome). 
2. **`Index.cshtml`**: The landing page. Uses Razor syntax to iterate over the `Portfolios` model to dynamically render the "Four Seasons" grid.
3. **Models**:
   - `Portfolio`: Handles the 4 seasons data (Spring, Summer, Autumn, Winter).
   - `LandscapePrint`: Handles the specific prints available for sale (Oregon Trip, Seasonal Travel, Autumn Series).
   - `HomeViewModel`: A wrapper class to pass both lists cleanly into the `Index.cshtml` view.

## Setup & Running
1. Open the solution in Visual Studio or VS Code.
2. Ensure you have the .NET 6.0 SDK (or later) installed.
3. Move the provided inline `<style>` CSS block from your original HTML into `wwwroot/css/site.css`.
4. Run `dotnet restore` to fetch dependencies.
5. Run `dotnet run` to start the local web server.

## Running Tests
Navigate to the `LDPhotography.Tests` directory and execute:
```bash
dotnet test




dotnet dev-certs https --clean
dotnet dev-certs https --trust
ngrok http https://localhost:44359 --host-header="localhost:44359"