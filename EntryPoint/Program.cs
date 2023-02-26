using ProductRepositoryScripts = SqlGeneratedCode.SqlScripts.Oracle.ProductRepository;

var myQuery = ProductRepositoryScripts.GetProductById;

// Prints "SELECT * FROM TABLE_PRODUCTS WHERE Id = @Id;"
Console.WriteLine(myQuery);