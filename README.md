# Survey API

A C# ASP.NET Core Web API that allows creation and magement of surveys with questions and responses.

---

## 🚀 Getting Started

### Prerequisites
- [.NET 8 SDK](https://dotnet.microsoft.com/download)
- [Visual Studio 2022](https://visualstudio.microsoft.com/) (or VS Code)

---

## 🏗 How to Build and Run

1. **Open the solution**
   - Double-click the `.sln` file (e.g., `SurveyAPI.sln`).
   - Visual Studio will open the project.

2. **Build the solution**
   - From the top menu, choose:  
     `Build → Build Solution` (or press **Ctrl + Shift + B**)

3. **Run the API**
   - Click the green **▶ Run** button or press **F5**.
   - The API will launch and open Swagger automatically in your browser.

4. **Access the API**
   - Swagger UI: [https://localhost:5001/swagger](https://localhost:5001/swagger)
   - HTTP Endpoint: `http://localhost:5000/api/...`
   - HTTPS Endpoint: `https://localhost:5001/api/...`

### Scoring 
Scoring is handled through the GetSurveyScore endpoint by calculating the total of all numeric responses in a survey.

### Architectural Decisions
The project structure uses controllers to handle HTTP requests and responses, models that contain the data structure and business logic
and data that holds SurveyContext for database access. All endpoints follow RESTful principles.
