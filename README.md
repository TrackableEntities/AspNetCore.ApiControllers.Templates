# ASP.NET Core Web API Controller Templates

Customizable Razor templates for ASP.NET Core Web API controllers

## Prerequisites

Install package and add ASP.NET Web API Templates:
- .NET SDK 2.2 or greater

Use the template to generate Web API controllers:
- Visual Studio 2017 v 15.9.4 or greater

Run and/or re-create the sample:
- SQL Server LocalDb
- SQL Server Management Studio

## Installation

To install the templates package open a terminal and run:
- For the latest _stable_ version:

    ```
    dotnet new -i "AspNetCore.WebApi.Templates::*"
    ```

To uninstall the templates package open a terminal and run:

```
dotnet -u "AspNetCore.WebApi.Templates"
```

## Usage

To install templates in an ASP.NET Core Web API project, open a terminal at the project root and run:

```
dotnet new webapi-templates
```

A **Templates** folder will be added containing a **ControllerGenerator** folder with Razor templates that can be customized to generate Web API controllers.

> **Important**: You must explicitly set the **Build Action** of each .cshtml file to `None` in order to build the project. 

You can add Web API controllers using these templates either from Visual Studio or the terminal.

## Sample Steps

Follow these step to re-create the sample from scratch.

1. Open SQL Server Management Studio and create a new database named NorthwindSlim.
    - Download the script: <http://bit.ly/northwindslim>
    - Extract and run the script.

2. Create a new ASP.NET Core Web API project.
    - Remove ValuesController.cs from the Controllers folder.

3. Add the following NuGet package:

    ```
    Microsoft.EntityFrameworkCore.SqlServer
    ```

4. Reverse engineer context and entity classes from the NorthwindSlim database.

    > Note: This sample uses a database-first approach, but you can also use a code-first approach by using EF Core code migrations.

    - Run the following command to generate context and entity classes.

    ```
    dotnet ef dbcontext scaffold "Data Source=(localdb)\MSSQLLocalDB; Initial Catalog=NorthwindSlim; Integrated Security=True" Microsoft.EntityFrameworkCore.SqlServer -o Models -c NorthwindSlimContext -f
    ```

    - You can also run an equivalent command from the Package Manager Console within Visual Studio.

    ```
    Scaffold-DbContext "Data Source=(localdb)\MSSQLLocalDB; Initial Catalog=NorthwindSlim; Integrated Security=True" Microsoft.EntityFrameworkCore.SqlServer -OutputDir Models -Context NorthwindSlimContext -Force
    ```

    - If you wish you can remove the `OnConfiguring`method from the generated `NorthwindSlimContext` class.

5. Configure the web app to use the NorthwindSlim database.

    - Add a connection string to **appsettings.json**.

    ```json
    "ConnectionStrings": {
        "NorthwindSlimContext": "Data Source=(localdb)\\MSSQLLocalDB;initial catalog=NorthwindSlim;Integrated Security=True; MultipleActiveResultSets=True"
    }
    ```

    - Update the `ConfigureServices` method in **Startup.cs** to accommodate cyclical references and register the DbContext class.

    ```csharp
    public void ConfigureServices(IServiceCollection services)
    {
        services.AddMvc()
            .SetCompatibilityVersion(CompatibilityVersion.Version_2_2)
            .AddJsonOptions(options =>
                options.SerializerSettings.PreserveReferencesHandling = PreserveReferencesHandling.All);
        var connectionString = Configuration.GetConnectionString(nameof(NorthwindSlimContext));
        services.AddDbContext<NorthwindSlimContext>(options => options.UseSqlServer(connectionString));
    }
    ```

6. If you execute `dotnet new` from a terminal at the project root, you'll see a list of available templates, including **ASP.NET Core Web API Templates**, which you installed earlier.
    - Add custom templates to the project by running:

    ```
    dotnet new webapi-templates
    ```

    - You will see a **Templates** folder appear in the project containing a **ControllerGenerator** folder with razor template files that you can customize.
    - Open the Templates/ControllerGenerator folder and view properties for the first .cshtml file in the list, then set the **Build Action** of each .cshtml file to `None`.

7. Add a Values controler with read/write actions using Visual Studio.
    - Right-click the **Controllers** folder and select **Add Controller**.
    - From the dialog select _API Controller with read/write actions_ and name it **ValuesController**.
        + Press **Ctrl+F5** to run the Web API. You should see values JSON displayed in the browser.
        + You can use a REST client such as Postman to submit POST, PUT and DELETE requests.
    ```
    GET: http://localhost:53225/api/values
    GET: http://localhost:53225/api/values/5
    POST: http://localhost:53225/api/values
        - JSON Body: "value3"
    PUT: http://localhost:53225/api/values/5
        - JSON Body: "value4" 
    DELETE: http://localhost:53225/api/values/5
    ```
8.  To add a controller from a terminal without using Visual Studio you can execute the `dotnet aspnet-codegenerator controller` command.
    - First add the following to the .csproj file (find latest version on nuget.org):

    ```xml
    <ItemGroup>
        <DotNetCliToolReference Include="Microsoft.VisualStudio.Web.CodeGeneration.Tools" Version="2.0.4" />
    </ItemGroup>
    ```
    - Then execute `dotnet restore` from the command line.
    - Run `dotnet aspnet-codegenerator --help` for a list of options.
    - To add a Values controller (`-f` overwrites previous file):

    ```
    dotnet aspnet-codegenerator controller -name Values -api -actions -outDir Controllers -f
    ```

9.  Add a Products API controller with actions using Entity Framework.
    - You can use Visual Studio to add the controller, selecting _Product_ for the model class and _NorthwindSlimContext_ for the data context class.

    - To generate the Products controller from a terminal run:

    ```
    dotnet aspnet-codegenerator controller -name ProductsController -api -m Product -dc NorthwindSlimContext -outDir Controllers -f
    ```

10. Use Postman to issue requests to the Products controller.

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
        "discontinued": false
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
    
