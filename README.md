# ASP.NET Core Web API Controller Templates

Customizable Razor templates for ASP.NET Core Web API controllers

## Prerequisites

Install package and add ASP.NET Web API Templates:
- .NET SDK 2.0 or greater

Use the template to generate Web API controllers:
- Visual Studio 2017

Run and/or re-create the sample:
- SQL Server LocalDb
- SQL Server Management Studio

## Installation

To install the templates package open a terminal and run:

    ```
    dotnet new -i "AspNetCore.WebApi.Templates::*"
    ```

## Usage

To install templates in an ASP.NET Core Web API project, open a terminal at the project root and run:

    ```
    dotnet new webapi-templates
    ```

A **Templates** folder will be added containing a **ControllerGenerator** folder with Razor templates that can be customized to generate Web API controllers.

> Note: The Razor templates have been adapted from those supplied with Visual Studio and represent best practices for 

You can add Web API controllers using these templates either from Visual Studio or the terminal.

## Sample Steps

Follow these step to re-create the sample from scratch.

1. Open SQL Server Management Studio and create a new database named NorthwindSlim.
    - Download the script: <http://bit.ly/northwindslim>
    - Extract and run the script.

2. Create a new ASP.NET Core Web API project.
    - Remove ValuesController.cs from the Controllers folder.

3. Add the following NuGet packages:

    ```
    Microsoft.EntityFrameworkCore.SqlServer
    Microsoft.EntityFrameworkCore.Tools
    Microsoft.VisualStudio.Web.CodeGeneration.Design
    ```

4. Add the EF Core command line tools as a .NET CLI tool.
    - Edit the csproj file to add Microsoft.EntityFrameworkCore.Tools.DotNet
      as a DotNetCliToolReference, as in the following snippet.

    ```xml
    <ItemGroup>
        <DotNetCliToolReference Include="Microsoft.VisualStudio.Web.CodeGeneration.Tools" Version="2.0.0" />
        <DotNetCliToolReference Include="Microsoft.EntityFrameworkCore.Tools.DotNet" Version="2.0.0" />
    </ItemGroup>
    ```

    - Then open a termina and run `dotnet restore`.

5. Reverse engineer context and entity classes from the NorthwindSlim database.

    > Note: This sample uses a database-first approach, but you can also use a code-first approach by using EF Core code migrations.

    - Run the following command to generate context and entity classes.

    ```
    dotnet ef dbcontext scaffold "Data Source=(localdb)\MSSQLLocalDB; Initial Catalog=NorthwindSlim; Integrated Security=True" Microsoft.EntityFrameworkCore.SqlServer -o Models -c NorthwindSlimContext -f
    ```

    - You can also run an equivalent command from the Package Manager Console within Visual Studio.

    ```
    Scaffold-DbContext "Data Source=(localdb)\MSSQLLocalDB; Initial Catalog=NorthwindSlim; Integrated Security=True" Microsoft.EntityFrameworkCore.SqlServer -OutputDir Models -Context NorthwindSlimContext -Force
    ```

6. Configure the web app to use the NorthwindSlim database.

    - Add a connection string to **appsettings.json**.

    ```json
    "ConnectionStrings": {
        "NorthwindContext": "Data Source=(localdb)\\MSSQLLocalDB;initial catalog=NorthwindSlim;Integrated Security=True; MultipleActiveResultSets=True"
    }
    ```

    - Update the `ConfigureServices` method in **Startup.cs** to accommodate cyclical references and register the DbContext class.

    ```csharp
    public void ConfigureServices(IServiceCollection services)
    {
        services.AddMvc()
            .AddJsonOptions(options =>
                options.SerializerSettings.PreserveReferencesHandling = PreserveReferencesHandling.All);
        var connectionString = Configuration.GetConnectionString("NorthwindContext");
        services.AddDbContext<NorthwindSlimContext>(options => options.UseSqlServer(connectionString));
    }
    ```

7. Add a partial `NorthwindSlimContext` class that includes a constructor accepting `DbContextOptions`.
    - Add the partial class to the **Models** folder and namespace, but name the file with an Ex suffix.

    ```csharp
    namespace TemplatesSample.Models
    {
        public partial class NorthwindSlimContext
        {
            public NorthwindSlimContext(DbContextOptions<NorthwindSlimContext> options) : base(options) { }
        }
    }
    ```

    - If you wish you can remove the `OnConfiguring`method from the generated `NorthwindSlimContext` class.

8. If you execute `dotnet new` from a terminal at the project root, you'll see a list of available templates, including **ASP.NET Core Web API Templates**, which you installed earlier.
    - Add custom templates to the project by running:

    ```
    dotnet new webapi-templates
    ```

    - You will see a **Templates** folder appear in the project containing a **ControllerGenerator** folder with razor template files that you can customize.

9. Add a Values controler with read/write actions.
    - Right-click the **Controllers** folder and select **Add Controller**.
    - From the dialog select _API Controller with read/write actions_ and name it **ValuesController**.
        + Press **Ctrl+F5** to run the Web API. You should see values JSON displayed in the browser.
        + You can use a REST client such as Postman to submit POST, PUT and DELETE requests.

10. Add a Products API controller with actions using Entity Framework.
    - Select _Product_ for the model class and _NorthwindSlimContext_ for the data context class.
    - Use Postman to issue requests to the Products controller.

    ```
    GET: http://localhost:53225/api/products
    GET: http://localhost:53225/api/products/1
    ```

    - Send a POST request.

    ```
    POST: http://localhost:53225/api/products
    ```

    ```json
    {
    "productName": "Chocolato",
    "categoryId": 1,
    "unitPrice": 23,
    "discontinued": false,
    "rowVersion": "AAAAAAAAD6E="
    }
    ```

    - You should receive a 201 response with the following body:

    ```json
    {
        "$id": "156",
        "productId": 1078,
        "productName": "Chocolato",
        "categoryId": 1,
        "unitPrice": 23,
        "discontinued": false,
        "rowVersion": "AAAAAAAAF3E=",
        "category": null,
        "orderDetail": {
            "$id": "157",
            "$values": []
        }
    }
    ```

    - Submit a PUT request supplying the new product id, but changing the product name. You shoud receive a response with the udpated product.

    ```
    PUT: http://localhost:53225/api/products/1078
    ```

    ```json
    {
        "productId": 1078,
        "productName": "Chocolato - Updated",
        "categoryId": 1,
        "unitPrice": 23,
        "discontinued": false,
        "rowVersion": "AAAAAAAAF3E="
    }
    ```

    - Lastly, submit a DELETE request.

    ```
    DELETE: http://localhost:53225/api/products/1078
    ```

    - Submit a GET request to verify the product has been deleted because the response status code is 404.

    ```
    GET: http://localhost:53225/api/products/1078
    ```
    