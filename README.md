# Repository Pattern: Simple, Asynchronous and Generic
I wrote it for myself but who knows where it would appear...

## Benefits
1. Asynchronous (``IAsyncEnumerable``, ``Task``)
2. Generic
3. EF-connected (EF5)
4. Server-size pagination enabled
5. Usage explanation through tests

## Usage and restrictions
``git clone`` | Ctrl+C => Ctrl+V
  
No restrictions for you. Take it, modify, use!
  
*Just set reference to this Github page*

## Repository files
1. Interface: ``GenericRepository/Application/Interfaces/IRepository.cs``
2. Implementation: ``GenericRepository/Application/Implementations/Repository.cs``

## Test
Run ``dotnet test``

### ...with Docker:
Run ``docker build -t yanzaan/net5repositorytest . & docker run -it yanzaan/net5repositorytest``

### ...with Docker-Compose:
Run ``docker-compose up --build``

## Contribute
#### Here is waiting room for pulls...
Now repository needs selecting | ordering additions and test cases