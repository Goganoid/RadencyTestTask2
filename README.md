# RadencyTestTask2
You can import a postman collection from `postman.json`
## Starting the server
```
cd API/
dotnet run --launch-profile http 
```
## Available genres
```
{
        "Detective",
        "Action",
        "Sci-fi",
        "Fantasy",
        "Science"
}
```
## Notes
Save book request validates base64 image, you can try it using this [converter](https://www.base64-image.de/). However, there is an option to insert an empty string `""`.
