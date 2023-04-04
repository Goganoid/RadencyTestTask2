# RadencyTestTask2
## [Task 2](https://docs.google.com/document/d/1ZbKie3rOOmcNqeoN1W3Cvtz8MaEbkUQYKsAzt4nSnDs/edit?usp=sharing)
## [Task 3](https://docs.google.com/document/d/13xa9gmevlFB3AvkGFXqF9MhDPAcuFntN8xaaYxp_4qM/edit?usp=sharing)
## [Additional tasks](https://docs.google.com/document/d/1KU-eHubGFQCq_AJdL2QCsrQLR3wRUHCfvBEg_retbD8/edit?usp=sharing)
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
